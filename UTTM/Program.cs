using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System.Configuration;
using System.Text;
using UTTM.Business;
using UTTM.Business.Interfaces;
using UTTM.Context;
using UTTM.Infra.Interfaces;

var builder = WebApplication.CreateBuilder(args);
#region Builder
var Configuration = builder.Configuration;

// Add services to the container.
var jwtIssuer = Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = Configuration.GetSection("Jwt:Key").Get<string>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });
builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped(p => new UniversityBusiness(p.GetRequiredService<UttmDbContext>()));
builder.Services.AddScoped(p => new MajorBusiness(p.GetRequiredService<UttmDbContext>(), p.GetRequiredService<UniversityBusiness>()));
builder.Services.AddScoped(p => new UserBusiness(p.GetRequiredService<UttmDbContext>(), p.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped(p => new SocietyBusiness(p.GetRequiredService<UttmDbContext>(), p.GetRequiredService<UserBusiness>(), p.GetRequiredService<MajorBusiness>()));
builder.Services.AddScoped(p => new StudentBusiness(p.GetRequiredService<UttmDbContext>(), p.GetRequiredService<SocietyBusiness>(), p.GetRequiredService<UserBusiness>()));
builder.Services.AddScoped(p => new SettingBusiness(p.GetRequiredService<UttmDbContext>(), p.GetRequiredService<UserBusiness>()));
builder.Services.AddScoped(p => new ProfessorBusiness(p.GetRequiredService<UttmDbContext>(), p.GetRequiredService<UserBusiness>(), p.GetRequiredService<SocietyBusiness>()));
builder.Services.AddScoped(p => new EventBusiness(p.GetRequiredService<UttmDbContext>(), p.GetRequiredService<SocietyBusiness>()));

//builder.Services.AddControllersWithViews();
builder.Services.AddLogging();
var serviceProvider = builder.Services.BuildServiceProvider();
//var logger = serviceProvider.GetService<ILogger<TestModelsController>>();
//builder.Services.AddSingleton(typeof(ILogger), logger);

builder.Services.AddEntityFrameworkSqlServer().AddDbContext<UttmDbContext>(config =>
{
    config.UseSqlServer(Configuration.GetConnectionString("Default"));
}, ServiceLifetime.Singleton);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme() { In = ParameterLocation.Header, Name = "Authorization", Type = SecuritySchemeType.ApiKey });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:8080",
                                              "http://localhost:44302",
                                              "http://localhost:4200");
                      });
});

#endregion

var app = builder.Build();
#region App
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UTTM APIs");

});

app.UseHttpsRedirection();


app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TestModels}/{action=Index}/{id?}");

app.Run();
#endregion
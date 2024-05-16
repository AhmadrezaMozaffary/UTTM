using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UTTM.Context;

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

builder.Services.AddControllers();
// Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services.AddLogging();
var serviceProvider = builder.Services.BuildServiceProvider();
//var logger = serviceProvider.GetService<ILogger<TestModelsController>>();
//builder.Services.AddSingleton(typeof(ILogger), logger);

builder.Services.AddEntityFrameworkSqlServer().AddDbContext<UttmDbContext>(config =>
{
    config.UseSqlServer(Configuration.GetConnectionString("Default"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();
app.UseAuthorization();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TestModels}/{action=Index}/{id?}");

app.Run();
#endregion
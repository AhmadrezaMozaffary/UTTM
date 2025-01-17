﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UTTM.Business.Interfaces;
using UTTM.Context;
using UTTM.Controllers.Api;
using UTTM.Infra;
using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class UserBusiness : BusinessBase, IUserBusiness
    {
        private IConfiguration _config;

        public UserBusiness(UttmDbContext ctx, IConfiguration config) : base(ctx)
        {
            _config = config;
        }

        public async Task<string> Login(LoginRequest user)
        {
            if (!UserExists(user.UserName)) { throw new Exception("نام کاربری وجود ندارد"); };

            User dbUser = await ctx.User.FirstAsync<User>(_user => _user.UserName == user.UserName);

            if (dbUser != null && (dbUser.Password == HashPassword(user.Password)))
            {
                return GetToken(dbUser);
            }
            else
            {
                throw new Exception("اطلاعات وارد شده صحیح نمیباشد");
            }
        }

        public async Task<int> CreateUser(SignUpViewModel req)
        {
            if (UserExists(req.UserName))
            {
                throw new Exception("نام کاربری قبلا انتخاب شده است");
            }

            DateTime currentDate = DateTime.Now;
            User _newUser = new()
            {
                Id = 0,
                UserName = req.UserName,
                Password = HashPassword(req.Password),
                Role = req.Role,
                CreatedAt = currentDate,
                LastUpdatedAt = currentDate
            };

            await ctx.User.AddAsync(_newUser);
            Save();

            return _newUser.Id;
        }

        public bool UserExists(string username)
        {
            return ctx.User.Any(u => u.UserName == username);
        }

        public bool UserExists(int userId)
        {
            return ctx.User.Any(u => u.Id == userId);
        }

        public bool UserCanBeTypeOf(UserRole targetRole, int userId)
        {
            UserRole? role = ctx.User.First(u => u.Id == userId).Role;

            return role == targetRole;
        }

        public string GetToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
              //  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, GetUserRole((int)user.Role))
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }

        private string GetUserRole(int targetRole)
        {
            var roleMapperDict = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToDictionary(ur => (int)ur, ur => ur.ToString());

            return roleMapperDict[targetRole];
        }

        public string HashPassword(string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                    sBuilder.Append(data[i].ToString("x2"));

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
    }
}

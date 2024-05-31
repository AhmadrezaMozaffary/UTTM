using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;

namespace UTTM.Business
{
    public class UserBusiness : BusinessBase
    {
        public UserBusiness(UttmDbContext ctx) : base(ctx)
        {
        }

        public bool UserExists(string username)
        {
            return Ctx.User.Any(u => u.UserName == username);
        }

        public bool UserExists(int userId)
        {
            return Ctx.User.Any(u => u.Id == userId);
        }

        public bool UserCanBeTypeOf(UserRole targetRole, int userId)
        {
            UserRole? role = Ctx.User.First(u => u.Id == userId).Role;

            return role == targetRole;
        }

        public string GetToken(IConfiguration _config, User user)
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

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

        public bool UserCanBeTypeOf(UserRole targetRole,int userId)
        {
            UserRole? role = Ctx.User.First(u => u.Id == userId).Role;

            return role == targetRole;
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

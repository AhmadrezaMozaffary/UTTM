using Microsoft.EntityFrameworkCore;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;

namespace UTTM.Business
{
    public class StudentBusiness : BusinessBase

    {
        public StudentBusiness(UttmDbContext ctx) : base(ctx)
        {
        }

        public bool CurrentStudentExists(int userId)
        {
            return Ctx.Student.Where(s => s.UserId == userId).Any();
        }

        public Task<Student?> GetStudent(int id)
        {
            return Ctx.Student.FirstOrDefaultAsync(s => s.Id == id);
        }

    }
}

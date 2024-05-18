using Microsoft.EntityFrameworkCore;
using UTTM.Context;
using UTTM.Infra;
using UTTM.Models;

namespace UTTM.Business
{
    public class ProfessorBusiness : BusinessBase
    {
        public ProfessorBusiness(UttmDbContext ctx) : base(ctx)
        {
        }

        public bool CurrentProfessorExists(int userId)
        {
            return Ctx.Professor.Where(s => s.UserId == userId).Any();
        }

        public Task<Professor?> GetProfessor(int id)
        {
            return Ctx.Professor.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

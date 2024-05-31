using Microsoft.AspNetCore.Mvc;
using UTTM.Context;

namespace UTTM.Infra
{
    public class BusinessBase
    {
        protected readonly UttmDbContext ctx;

        public BusinessBase(UttmDbContext _ctx)
        {
            ctx = _ctx;
        }

        [NonAction]
        public void Save()
        {
            ctx.SaveChanges();
        }
    }
}

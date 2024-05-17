using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTTM.Context;

namespace UTTM.Infra
{
    public class UttmController : ControllerBase
    {
        public UttmDbContext Ctx { get; private set; }

        public UttmController(UttmDbContext context)
        {
            Ctx = context;
        }

        [NonAction]
        public void Save()
        {
            Ctx.SaveChanges();
        }

    }
}

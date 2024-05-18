using UTTM.Context;

namespace UTTM.Infra
{
    public class BusinessBase
    {
        public UttmDbContext Ctx { get; private set; }

        public BusinessBase(UttmDbContext ctx)
        {
            Ctx = ctx;
        }
    }
}

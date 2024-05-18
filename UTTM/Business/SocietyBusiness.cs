using UTTM.Context;
using UTTM.Infra;

namespace UTTM.Business
{
    public class SocietyBusiness : BusinessBase
    {
        public SocietyBusiness(UttmDbContext ctx) : base(ctx)
        {
        }


        public bool SocietyExists(int societyId)
        {
            return Ctx.Society.Any(s => s.Id == societyId);
        }

        public bool SocietyExistsForThisMajor(int majorId)
        {
            return Ctx.Society.Any(s => s.MajorId == majorId);
        }
    }
}

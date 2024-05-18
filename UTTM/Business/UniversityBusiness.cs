using UTTM.Context;
using UTTM.Infra;

namespace UTTM.Business
{
    public class UniversityBusiness : BusinessBase
    {
        public UniversityBusiness(UttmDbContext ctx) : base(ctx)
        {
        }

        public bool UniversityExists(string uniName)
        {
            return Ctx.University.Any(u => u.Name == uniName);
        }


        public bool UniversityExists(int uniId)
        {
            return Ctx.University.Any(u => u.Id == uniId);
        }
    }
}

using UTTM.Context;
using UTTM.Infra;
using UTTM.Models.ViewModels;

namespace UTTM.Business
{
    public class MajorBusiness : BusinessBase
    {
        public MajorBusiness(UttmDbContext ctx) : base(ctx)
        {
        }

        public bool MajorExists(int majorId)
        {
            return Ctx.Major.Any(m => m.Id == majorId);
        }

        public bool MajorExists(MajorViewModel req)
        {
            bool exists = Ctx.Major.Where(m => m.UniversityId == req.UniversityId).Any(m => m.Title == req.Title);

            return exists;
        }

    }
}

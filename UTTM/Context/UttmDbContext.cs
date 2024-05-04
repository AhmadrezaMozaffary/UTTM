using Microsoft.EntityFrameworkCore;

namespace UTTM.Context
{
    public class UttmDbContext : DbContext
    {
        public UttmDbContext(DbContextOptions<UttmDbContext> options) : base(options)
        {
        }
    }
}

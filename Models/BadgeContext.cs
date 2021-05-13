using Microsoft.EntityFrameworkCore;

namespace GamificationApi.Models
{
    public class BadgeContext : DbContext
    {
        public BadgeContext(DbContextOptions<BadgeContext> options)
            : base(options)
        {
        }

        public DbSet<Badge> BadgeItems { get; set; }
    }
}

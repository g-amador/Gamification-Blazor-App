using Microsoft.EntityFrameworkCore;

namespace GamificationApi.Models.Context
{
    public class RuleContext : DbContext
    {
        public RuleContext(DbContextOptions<RuleContext> options)
            : base(options)
        {
        }

        public DbSet<Rule> RuleItems { get; set; }
    }
}

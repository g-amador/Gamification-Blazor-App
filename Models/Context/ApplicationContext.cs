using Microsoft.EntityFrameworkCore;
using GamificationApi.Models;

namespace GamificationApi.Models.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Application> ApplicationItems { get; set; }

        public DbSet<GamificationApi.Models.Event> Event { get; set; }
    }
}

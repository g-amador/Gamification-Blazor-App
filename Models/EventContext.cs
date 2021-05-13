using Microsoft.EntityFrameworkCore;

namespace GamificationApi.Models
{
    public class EventContext : DbContext
    {
        public EventContext(DbContextOptions<EventContext> options)
            : base(options)
        {
        }

        public DbSet<Event> EventItems { get; set; }
    }
}

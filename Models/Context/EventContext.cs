using Microsoft.EntityFrameworkCore;

namespace GamificationApi.Models.Context
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

using Microsoft.EntityFrameworkCore;

namespace GamificationApi.Models.Context
{
    public class PlayerContext : DbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> options)
            : base(options)
        {
        }

        public DbSet<Player> PlayerItems { get; set; }
    }
}

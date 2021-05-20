using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GamificationAPI.Models;
using GamificationApi.Models;

namespace GamificationAPI.Data
{
    public class GamificationAPIContext : DbContext
    {
        public GamificationAPIContext (DbContextOptions<GamificationAPIContext> options)
            : base(options)
        {
        }

        public DbSet<GamificationAPI.Models.Employee> Employee { get; set; }

        public DbSet<GamificationApi.Models.Application> Application { get; set; }

        public DbSet<GamificationApi.Models.Badge> Badge { get; set; }

        public DbSet<GamificationApi.Models.Event> Event { get; set; }

        public DbSet<GamificationApi.Models.Player> Player { get; set; }

        public DbSet<GamificationApi.Models.Rule> Rule { get; set; }
    }
}

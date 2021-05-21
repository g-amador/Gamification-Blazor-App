using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GamificationAPI.Models;

namespace GamificationAPI.Data
{
    public class GamificationAPIContext : DbContext
    {
        public GamificationAPIContext (DbContextOptions<GamificationAPIContext> options)
            : base(options)
        {
        }

        public DbSet<GamificationAPI.Models.Employee> Employee { get; set; }

        #region GamificationAPI DbSets
        public DbSet<GamificationAPI.Models.Application> Application { get; set; }

        public DbSet<GamificationAPI.Models.Badge> Badge { get; set; }

        public DbSet<GamificationAPI.Models.Event> Event { get; set; }

        public DbSet<GamificationAPI.Models.Player> Player { get; set; }

        public DbSet<GamificationAPI.Models.Rule> Rule { get; set; }
        #endregion

    }
}

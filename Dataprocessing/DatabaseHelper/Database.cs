using DatabaseHelper.Models;
using Microsoft.EntityFrameworkCore;
using System;

/*
 * 
 *  Some of this code is thanks to https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core-example.html
 *  
 */

namespace DatabaseHelper
{
    public class Database : DbContext
    {
        public DbSet<TerrorismEvent> Gtd { get; set; }
        public DbSet<SpotifyTrendingSong> SpotifyData { get; set; }
        public DbSet<YoutubeTrendingVideo> Youtube { get; set; }
        public DbSet<AuthKey> AuthKeys { get; set; }

        private string server;
        private string database;
        private string user;
        private string password;

        public Database(string server, string database, string user, string password)
        {
            this.server = server;
            this.database = database;
            this.user = user;
            this.password = password;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL($"server={server};database={database};user={user};password={password}; convert zero datetime=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Entity for Global Terrorism Database.
            modelBuilder.Entity<TerrorismEvent>().HasKey(model => model.eventid);

            // Entity for spotify top lists.
            modelBuilder.Entity<SpotifyTrendingSong>().HasKey(model => new { model.Position, model.Date, model.Region });

            // Entity for Youtube top lists.
            modelBuilder.Entity<YoutubeTrendingVideo>().HasKey(model => new { model.VideoId, model.TrendingDate, model.CountryCode });
        }

        /// <summary>
        /// Lazy way to fix handling multiple connections
        /// </summary>
        /// <returns></returns>
        public Database NewConnection()
        {
            return new Database(this.server, this.database, this.user, this.password);
        }
    }
}

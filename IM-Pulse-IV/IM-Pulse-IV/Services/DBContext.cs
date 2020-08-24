using IM_Pulse_IV.Models.Main;
using IM_Pulse_IV.Models.RandomDataGenerator;
using Microsoft.EntityFrameworkCore;

namespace IM_Pulse_IV.Services
{
    public class DBContext : DbContext
    {
        private static bool _isCreated = false;
        public DBContext()
        {
            if (!_isCreated)
            {
                _isCreated = true;
                Database.EnsureCreated();
            }
        }
        public DbSet<UniqueTextParameter> UniqueTextParameters { get; set; }
        public DbSet<CommandParameter> CommandParameters { get; set; }
        public DbSet<RandomDataStats> RandomDataStats { get; set; }
        public DbSet<ReadData> ReadData { get; set; }

        public void DeleteDatabase()
        {
            Database.EnsureDeleted();
            _isCreated = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = App.ListOfAllPaths[(int)App.FileNames.DB];
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UniqueTextParameterDBConfig());
            modelBuilder.ApplyConfiguration(new CommandParameterDBConfig());
            modelBuilder.ApplyConfiguration(new RandomDataStatsDBConfig());
            modelBuilder.ApplyConfiguration(new ReadDataDBConfig());
        }

        public void ResetDB() => Database.EnsureDeleted();
    }
}

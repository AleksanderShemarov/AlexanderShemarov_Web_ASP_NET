using AlexanderShemarov.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlexanderShemarov.UI.Data
{
    public class TempDbContext : DbContext
    {
        public DbSet<Trains> TrainsAPI { get; set; }
        public DbSet<TrainTypes> TrainTypesAPI { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("");
        }
    }
}

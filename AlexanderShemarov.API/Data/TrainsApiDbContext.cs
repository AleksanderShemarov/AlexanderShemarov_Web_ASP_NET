using AlexanderShemarov.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlexanderShemarov.API.Data
{
    public class TrainsApiDbContext : DbContext
    {
        public TrainsApiDbContext(DbContextOptions<TrainsApiDbContext> options) : base(options)
        {
            
        }
        public DbSet<Trains> TrainsAPI { get; set; }
        public DbSet<TrainTypes> TrainTypesAPI { get; set; }
    }
}

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<CarFeaturesEntity> FeaturesBaseEntities { get; set; }
    public DbSet<CarImagesEntity> ImagesEntities { get; set; }
    public DbSet<MainClassificationEntity> ClassificationEntities { get; set; }
    public DbSet<UserAccountEntity> AccountEntities { get; set; }
    public DbSet<UserFavoritesEntity> AccountFavoritesEntities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        /*
         * GetExecutingAssembly ifadesi, başka Class ifadelerinde "IEntityTypeConfiguration" ifadesini implement edilip edilmediğine bakar ve edilmişse çeker.
         */
        base.OnModelCreating(modelBuilder);
    }
}
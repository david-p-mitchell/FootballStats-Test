using AFCStatsApp.Models.Player;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AFCStatsApp.Db;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PlayerModel> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PlayerModel>(entity =>
        {
            entity.HasKey(p => p.PlayerId);
            entity.Property(p => p.PlayerName)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(p => p.Position)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(20);
            entity.Property(p => p.JerseyNumber)
                  .IsRequired();
            entity.Property(p => p.GoalsScored)
                  .HasDefaultValue(0);
        });
    }
}

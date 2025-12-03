using Microsoft.EntityFrameworkCore;
using Biograf.Models;

namespace Biograf.Data
{
    /// <summary>
    /// DbContext för biograf-applikationen
    /// </summary>
    public class BiografContext : DbContext
    {
        public BiografContext(DbContextOptions<BiografContext> options) : base(options)
        {
        }

        public DbSet<Film> Filmer { get; set; }
        public DbSet<Salong> Salonger { get; set; }
        public DbSet<Forestallning> Forestallningar { get; set; }
        public DbSet<Bokning> Bokningar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurera Film
            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Titel).IsRequired().HasMaxLength(200);
                entity.Property(f => f.Genre).IsRequired().HasMaxLength(100);
                entity.Property(f => f.Beskrivning).IsRequired().HasMaxLength(2000);
                entity.Property(f => f.BildUrl).HasMaxLength(500);
                entity.Property(f => f.Pris).HasColumnType("decimal(18,2)");
            });

            // Konfigurera Salong
            modelBuilder.Entity<Salong>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasIndex(s => s.Salongsnummer).IsUnique();
            });

            // Konfigurera Forestallning med relationer
            modelBuilder.Entity<Forestallning>(entity =>
            {
                entity.HasKey(f => f.Id);
                
                entity.HasOne(f => f.Film)
                    .WithMany(film => film.Forestallningar)
                    .HasForeignKey(f => f.FilmId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Salong)
                    .WithMany(s => s.Forestallningar)
                    .HasForeignKey(f => f.SalongId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Konfigurera Bokning med relationer och unikt bokningsnummer
            modelBuilder.Entity<Bokning>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.HasIndex(b => b.Bokningsnummer).IsUnique();
                
                // En föreställning kan inte ha samma stolnummer bokat två gånger
                entity.HasIndex(b => new { b.ForestallningId, b.Stolnummer }).IsUnique();

                entity.Property(b => b.Bokningsnummer).IsRequired().HasMaxLength(20);
                entity.Property(b => b.KundNamn).IsRequired().HasMaxLength(200);
                entity.Property(b => b.KundEmail).IsRequired().HasMaxLength(200);

                entity.HasOne(b => b.Forestallning)
                    .WithMany(f => f.Bokningar)
                    .HasForeignKey(b => b.ForestallningId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

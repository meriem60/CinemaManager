using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CinemaManager_meriem.Models.Cinema;

public partial class CinemaContext : DbContext
{
    public CinemaContext()
    {
    }

    public CinemaContext(DbContextOptions<CinemaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:CinemaCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Movie__3214EC0778BDD527");

            entity.ToTable("Movie");

            entity.Property(e => e.Genre).HasMaxLength(30);
            entity.Property(e => e.Title).HasMaxLength(30);

            entity.HasOne(d => d.Producer).WithMany(p => p.Movies)
                .HasForeignKey(d => d.ProducerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movie__ProducerI__267ABA7A");
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producer__3214EC072464EE7F");

            entity.ToTable("Producer");

            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Nationality).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

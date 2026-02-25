using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hastane.Models;

public partial class HastaneContext : DbContext
{

    public HastaneContext(DbContextOptions<HastaneContext> options)
        : base(options)
    {
    }
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Program.cs içinde yapılandırıldığı için burası boş
    }*/

    public virtual DbSet<Doktor> Doktors { get; set; }

    public virtual DbSet<Hastum> Hasta { get; set; }

    public virtual DbSet<Kayıt> Kayıts { get; set; }

    public virtual DbSet<OnlineRandevu> OnlineRandevus { get; set; }

    public virtual DbSet<Randevu> Randevus { get; set; }

    public virtual DbSet<Tedavi> Tedavis { get; set; }

    
        

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doktor>(entity =>
        {
            entity.ToTable("Doktor");

            entity.Property(e => e.Alan)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Randevuları)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Soyisim).HasMaxLength(50);
            entity.Property(e => e.İsim).HasMaxLength(50);
        });

        modelBuilder.Entity<Hastum>(entity =>
        {
            entity.HasKey(e => e.Tc);

            entity.Property(e => e.Soyisim)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.İsim)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Şikayet)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Kayıt>(entity =>
        {
            entity.ToTable("Kayıt");

            entity.Property(e => e.KayıtId).ValueGeneratedNever();
            entity.Property(e => e.RandevuFk).HasColumnName("RandevuFK");
            entity.Property(e => e.YönlendirmeFişi)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.RandevuFkNavigation).WithMany(p => p.Kayıts)
                .HasForeignKey(d => d.RandevuFk)
                .HasConstraintName("FK_Kayıt_OnlineRandevu");
        });

        modelBuilder.Entity<OnlineRandevu>(entity =>
        {
            entity.ToTable("OnlineRandevu");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Doktor).WithMany(p => p.OnlineRandevus)
                .HasForeignKey(d => d.DoktorId)
                .HasConstraintName("FK_OnlineRandevu_Doktor");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.OnlineRandevu)
                .HasForeignKey<OnlineRandevu>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnlineRandevu_Hasta");
        });

        modelBuilder.Entity<Randevu>(entity =>
        {
            entity.ToTable("Randevu");

            entity.Property(e => e.DoktorFk)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DoktorFK");
            entity.Property(e => e.HastaBilgisiFk)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HastaBilgisiFK");
            entity.Property(e => e.Poliklinik)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ŞikayetFk)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ŞikayetFK");
        });

        modelBuilder.Entity<Tedavi>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tedavi");

            entity.Property(e => e.Ilaç).HasMaxLength(50);
            entity.Property(e => e.Tedavi1)
                .HasMaxLength(100)
                .HasColumnName("Tedavi");
            entity.Property(e => e.TedaviId).HasColumnName("TedaviID");

            entity.HasOne(d => d.TedaviNavigation).WithMany()
                .HasForeignKey(d => d.TedaviId)
                .HasConstraintName("FK_Tedavi_Hasta");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

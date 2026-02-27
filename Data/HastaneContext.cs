using System;
using System.Collections.Generic;
using MyApiProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MyApiProject.Data;

public partial class HastaneContext : DbContext
{
    public HastaneContext()
    {
    }

    public HastaneContext(DbContextOptions<HastaneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doktor> Doktors { get; set; }

    public virtual DbSet<Hastum> Hasta { get; set; }

    public virtual DbSet<Kayıt> Kayıts { get; set; }

    public virtual DbSet<OnlineRandevu> OnlineRandevus { get; set; }

    public virtual DbSet<Randevu> Randevus { get; set; }

    public virtual DbSet<Tedavi> Tedavis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
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
            entity.HasKey(e => e.Id).HasName("PK_Hasta_1");

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
            entity.Property(e => e.DoktorName)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.DoktorSurname)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.HastaName)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.HastaSurname)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.HastaŞikayet).HasMaxLength(100);

            entity.HasOne(d => d.Doktor).WithMany(p => p.OnlineRandevus)
                .HasForeignKey(d => d.DoktorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
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
            entity.ToTable("Tedavi");

            entity.Property(e => e.TedaviId)
                .ValueGeneratedNever()
                .HasColumnName("TedaviID");
            entity.Property(e => e.Ilaç).HasMaxLength(50);
            entity.Property(e => e.Tedavi1)
                .HasMaxLength(100)
                .HasColumnName("Tedavi");

            entity.HasOne(d => d.TedaviNavigation).WithOne(p => p.Tedavi)
                .HasForeignKey<Tedavi>(d => d.TedaviId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tedavi_Hasta");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

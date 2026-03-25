using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyApiProject.Models;

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

    public virtual DbSet<Tedavi> Tedavis { get; set; }


    public virtual DbSet<Zaman> Zamen { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Doktor>(entity =>
        {
            entity.ToTable("Doktor");

            entity.Property(e => e.AccessToken)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Alan)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Eposta)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RefreshTokenEndDate).HasColumnType("datetime");
            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Soyisim)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.İsim)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Hastum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Hasta_1");

            entity.Property(e => e.AccessToken)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Eposta)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RefreshTokenEndDate).HasColumnType("datetime");
            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Soyisim)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.İsim)
                .IsRequired()
                .HasMaxLength(50)
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

            entity.Property(e => e.DoktorId).HasColumnName("DoktorID");
            entity.Property(e => e.DoktorName)
                .IsRequired()
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.DoktorSurname)
                .IsRequired()
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.HastaId).HasColumnName("HastaID");
            entity.Property(e => e.HastaName)
                .IsRequired()
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.HastaSurname)
                .IsRequired()
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.HastaŞikayet)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.Doktor).WithMany(p => p.OnlineRandevus)
                .HasForeignKey(d => d.DoktorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnlineRandevu_Doktor");

            entity.HasOne(d => d.Hasta).WithMany(p => p.OnlineRandevus)
                .HasForeignKey(d => d.HastaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnlineRandevu_Hasta1");
        });

        modelBuilder.Entity<Tedavi>(entity =>
        {
            entity.ToTable("Tedavi");

            entity.Property(e => e.TedaviId).HasColumnName("TedaviID");
            entity.Property(e => e.DoktorId).HasColumnName("DoktorID");
            entity.Property(e => e.HastaId).HasColumnName("HastaID");
            entity.Property(e => e.Recete)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Tanı)
                .IsRequired()
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Tedavi1)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Tedavi");

            entity.HasOne(d => d.Doktor).WithMany(p => p.Tedavis)
                .HasForeignKey(d => d.DoktorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tedavi_Doktor");

            entity.HasOne(d => d.Hasta).WithMany(p => p.Tedavis)
                .HasForeignKey(d => d.HastaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tedavi_Hasta");
        });


        modelBuilder.Entity<Zaman>(entity =>
        {
            entity.ToTable("Zaman");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DoktorId).HasColumnName("DoktorID");
            entity.Property(e => e.Zaman1)
                .HasColumnType("xml")
                .HasColumnName("Zaman");

            entity.HasOne(d => d.Doktor).WithMany(p => p.Zaman)
                .HasForeignKey(d => d.DoktorId)
                .HasConstraintName("FK_Zaman_Doktor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

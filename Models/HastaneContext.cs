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

    public virtual DbSet<Randevu> Randevus { get; set; }

    public virtual DbSet<Tedavi> Tedavis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-3GQK4RG\\SQLEXPRESS;Database=Hastane;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doktor>(entity =>
        {
            entity.ToTable("Doktor");

            entity.Property(e => e.Alan)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Eposta)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.RandevuId)
                .IsUnicode(false)
                .HasColumnName("RandevuID");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Soyisim)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.İsim)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Hastum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Hasta_1");

            entity.Property(e => e.Eposta)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RandevuId)
                .IsUnicode(false)
                .HasColumnName("RandevuID");
            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Soyisim)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TedaviId).HasColumnName("TedaviID");
            entity.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.İsim)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Tedavi).WithMany(p => p.Hasta)
                .HasForeignKey(d => d.TedaviId)
                .HasConstraintName("FK_Hasta_Tedavi");
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

            entity.Property(e => e.TedaviId).HasColumnName("TedaviID");
            entity.Property(e => e.Ilaç)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Tedavi1)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Tedavi");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

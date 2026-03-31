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

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentToDoktor> AppointmentToDoktors { get; set; }

    public virtual DbSet<Doktor> Doktors { get; set; }

    public virtual DbSet<Hastum> Hasta { get; set; }

    public virtual DbSet<HospitalReceptionist> HospitalReceptionists { get; set; }

    public virtual DbSet<Ilac> Ilacs { get; set; }

    public virtual DbSet<IlcaToRecete> IlcaToRecetes { get; set; }

    public virtual DbSet<Kayıt> Kayıts { get; set; }

    public virtual DbSet<OnlineRandevu> OnlineRandevus { get; set; }

    public virtual DbSet<Recete> Recetes { get; set; }

    public virtual DbSet<Tedavi> Tedavis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC07039A92A5");

            entity.ToTable("Appointment");

            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
        });

        modelBuilder.Entity<AppointmentToDoktor>(entity =>
        {
            entity.ToTable("Appointment_To_Doktor");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AppointmentFk).HasColumnName("AppointmentFK");
            entity.Property(e => e.DoktorFk).HasColumnName("DoktorFK");

            entity.HasOne(d => d.AppointmentFkNavigation).WithMany(p => p.AppointmentToDoktors)
                .HasForeignKey(d => d.AppointmentFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_To_Doktor_Appointment");

            entity.HasOne(d => d.DoktorFkNavigation).WithMany(p => p.AppointmentToDoktors)
                .HasForeignKey(d => d.DoktorFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_To_Doktor_Doktor");
        });

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

        modelBuilder.Entity<HospitalReceptionist>(entity =>
        {
            entity.ToTable("HospitalReceptionist");

            entity.Property(e => e.Id).HasColumnName("ID");
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
                .IsFixedLength();
            entity.Property(e => e.RefreshToken)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RefreshTokenEndDate).HasColumnType("datetime");
            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(50)
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

        modelBuilder.Entity<Ilac>(entity =>
        {
            entity.ToTable("Ilac");

            entity.Property(e => e.IlacId).HasColumnName("IlacID");
            entity.Property(e => e.IlacName)
                .IsRequired()
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("Ilac_Name");
            entity.Property(e => e.KullanımAlanı)
                .IsRequired()
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("Kullanım_Alanı");
        });

        modelBuilder.Entity<IlcaToRecete>(entity =>
        {
            entity.ToTable("Ilca_To_Recete");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IlcaFk).HasColumnName("IlcaFK");
            entity.Property(e => e.ReceteFk).HasColumnName("ReceteFK");

            entity.HasOne(d => d.IlcaFkNavigation).WithMany(p => p.IlcaToRecetes)
                .HasForeignKey(d => d.IlcaFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ilca_To_Recete_Ilac");

            entity.HasOne(d => d.ReceteFkNavigation).WithMany(p => p.IlcaToRecetes)
                .HasForeignKey(d => d.ReceteFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ilca_To_Recete_Recete");
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

        modelBuilder.Entity<Recete>(entity =>
        {
            entity.ToTable("Recete");

            entity.Property(e => e.ReceteId)
                .ValueGeneratedNever()
                .HasColumnName("ReceteID");
            entity.Property(e => e.GecerlilikTarihi).HasColumnName("Gecerlilik_Tarihi");
            entity.Property(e => e.Kullanım)
                .IsRequired()
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.ReceteNavigation).WithOne(p => p.Recete)
                .HasForeignKey<Recete>(d => d.ReceteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recete_Tedavi");
        });

        modelBuilder.Entity<Tedavi>(entity =>
        {
            entity.ToTable("Tedavi");

            entity.Property(e => e.TedaviId).HasColumnName("TedaviID");
            entity.Property(e => e.DoktorId).HasColumnName("DoktorID");
            entity.Property(e => e.HastaId).HasColumnName("HastaID");
            entity.Property(e => e.Tanı)
                .IsRequired()
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.Doktor).WithMany(p => p.Tedavis)
                .HasForeignKey(d => d.DoktorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tedavi_Doktor");

            entity.HasOne(d => d.Hasta).WithMany(p => p.Tedavis)
                .HasForeignKey(d => d.HastaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tedavi_Hasta");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

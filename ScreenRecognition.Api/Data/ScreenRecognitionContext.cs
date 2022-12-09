using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ScreenRecognition.Api.Models;

namespace ScreenRecognition.Api.Data;

public partial class ScreenRecognitionContext : DbContext
{
    public ScreenRecognitionContext()
    {
    }

    public ScreenRecognitionContext(DbContextOptions<ScreenRecognitionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Ocr> Ocrs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<Translator> Translators { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-BB20U5H;Database=ScreenRecognition;Trusted_Connection=True;User=fff;Password=05062003;TrustServerCertificate=True")
        .UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<History>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Id }).HasName("PK__History__74A9826E24D57797");

            entity.ToTable("History");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InputLanguageId).HasColumnName("InputLanguageID");
            entity.Property(e => e.OutputLanguageId).HasColumnName("OutputLanguageID");
            entity.Property(e => e.SelectedOcrid).HasColumnName("SelectedOCRID");
            entity.Property(e => e.SelectedTranslatorId).HasColumnName("SelectedTranslatorID");

            entity.HasOne(d => d.InputLanguage).WithMany(p => p.HistoryInputLanguages)
                .HasForeignKey(d => d.InputLanguageId)
                .HasConstraintName("FK__History__InputLa__38996AB5");

            entity.HasOne(d => d.OutputLanguage).WithMany(p => p.HistoryOutputLanguages)
                .HasForeignKey(d => d.OutputLanguageId)
                .HasConstraintName("FK__History__OutputL__398D8EEE");

            entity.HasOne(d => d.SelectedOcr).WithMany(p => p.Histories)
                .HasForeignKey(d => d.SelectedOcrid)
                .HasConstraintName("FK__History__Selecte__36B12243");

            entity.HasOne(d => d.SelectedTranslator).WithMany(p => p.Histories)
                .HasForeignKey(d => d.SelectedTranslatorId)
                .HasConstraintName("FK__History__Selecte__37A5467C");

            entity.HasOne(d => d.User).WithMany(p => p.Histories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__History__UserID__35BCFE0A");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Language__3214EC27DF08BC03");

            entity.ToTable("Language");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Ocralias)
                .HasMaxLength(50)
                .HasColumnName("OCRAlias");
            entity.Property(e => e.TranslatorAlias).HasMaxLength(50);
        });

        modelBuilder.Entity<Ocr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OCR__3214EC27C31CF7D9");

            entity.ToTable("OCR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC27CC13BC8C");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Name }).HasName("PK__Settings__70BF94E3D817A39A");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.InputLanguageId).HasColumnName("InputLanguageID");
            entity.Property(e => e.OutputLanguageId).HasColumnName("OutputLanguageID");
            entity.Property(e => e.SelectedOcrid).HasColumnName("SelectedOCRID");
            entity.Property(e => e.SelectedTranslatorId).HasColumnName("SelectedTranslatorID");

            entity.HasOne(d => d.InputLanguage).WithMany(p => p.SettingInputLanguages)
                .HasForeignKey(d => d.InputLanguageId)
                .HasConstraintName("FK__Settings__InputL__31EC6D26");

            entity.HasOne(d => d.OutputLanguage).WithMany(p => p.SettingOutputLanguages)
                .HasForeignKey(d => d.OutputLanguageId)
                .HasConstraintName("FK__Settings__Output__32E0915F");

            entity.HasOne(d => d.SelectedOcr).WithMany(p => p.Settings)
                .HasForeignKey(d => d.SelectedOcrid)
                .HasConstraintName("FK__Settings__Select__300424B4");

            entity.HasOne(d => d.SelectedTranslator).WithMany(p => p.Settings)
                .HasForeignKey(d => d.SelectedTranslatorId)
                .HasConstraintName("FK__Settings__Select__30F848ED");

            entity.HasOne(d => d.User).WithMany(p => p.Settings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Settings__UserID__2F10007B");
        });

        modelBuilder.Entity<Translator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Translat__3214EC272599350C");

            entity.ToTable("Translator");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC27CBEE364F");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleID__2C3393D0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

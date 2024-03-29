﻿using Microsoft.EntityFrameworkCore;
using ScreenRecognition.Api.Models.DbModels;

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

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Ocr> Ocrs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<Translator> Translators { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=FFF\\SQLEXPRESS;Database=ScreenRecognition;Trusted_Connection=False;User=fff;Password=05062003;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country__3214EC273B5A1FEB");

            entity.ToTable("Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Id }).HasName("PK__History__74A9826EB3CD60E4");

            entity.ToTable("History");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InputLanguageId).HasColumnName("InputLanguageID");
            entity.Property(e => e.OutputLanguageId).HasColumnName("OutputLanguageID");
            entity.Property(e => e.SelectedOcrid).HasColumnName("SelectedOCRID");
            entity.Property(e => e.SelectedTranslatorId).HasColumnName("SelectedTranslatorID");

            entity.HasOne(d => d.InputLanguage).WithMany(p => p.HistoryInputLanguages)
                .HasForeignKey(d => d.InputLanguageId)
                .HasConstraintName("FK__History__InputLa__3B75D760");

            entity.HasOne(d => d.OutputLanguage).WithMany(p => p.HistoryOutputLanguages)
                .HasForeignKey(d => d.OutputLanguageId)
                .HasConstraintName("FK__History__OutputL__3C69FB99");

            entity.HasOne(d => d.SelectedOcr).WithMany(p => p.Histories)
                .HasForeignKey(d => d.SelectedOcrid)
                .HasConstraintName("FK__History__Selecte__398D8EEE");

            entity.HasOne(d => d.SelectedTranslator).WithMany(p => p.Histories)
                .HasForeignKey(d => d.SelectedTranslatorId)
                .HasConstraintName("FK__History__Selecte__3A81B327");

            entity.HasOne(d => d.User).WithMany(p => p.Histories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__History__UserID__38996AB5");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Language__3214EC274C9EB977");

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
            entity.HasKey(e => e.Id).HasName("PK__OCR__3214EC275A99AFE7");

            entity.ToTable("OCR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC2713A8D91D");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Name }).HasName("PK__Settings__70BF94E31D947078");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.InputLanguageId).HasColumnName("InputLanguageID");
            entity.Property(e => e.OutputLanguageId).HasColumnName("OutputLanguageID");
            entity.Property(e => e.ResultColor).HasMaxLength(50);
            entity.Property(e => e.SelectedOcrid).HasColumnName("SelectedOCRID");
            entity.Property(e => e.SelectedTranslatorId).HasColumnName("SelectedTranslatorID");

            entity.HasOne(d => d.InputLanguage).WithMany(p => p.SettingInputLanguages)
                .HasForeignKey(d => d.InputLanguageId)
                .HasConstraintName("FK__Settings__InputL__34C8D9D1");

            entity.HasOne(d => d.OutputLanguage).WithMany(p => p.SettingOutputLanguages)
                .HasForeignKey(d => d.OutputLanguageId)
                .HasConstraintName("FK__Settings__Output__35BCFE0A");

            entity.HasOne(d => d.SelectedOcr).WithMany(p => p.Settings)
                .HasForeignKey(d => d.SelectedOcrid)
                .HasConstraintName("FK__Settings__Select__32E0915F");

            entity.HasOne(d => d.SelectedTranslator).WithMany(p => p.Settings)
                .HasForeignKey(d => d.SelectedTranslatorId)
                .HasConstraintName("FK__Settings__Select__33D4B598");

            entity.HasOne(d => d.User).WithMany(p => p.Settings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Settings__UserID__31EC6D26");
        });

        modelBuilder.Entity<Translator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Translat__3214EC27901CA492");

            entity.ToTable("Translator");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC27854CC5DE");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.NickName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__User__CountryID__2F10007B");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleID__2E1BDC42");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ViolationManagement.Models;

public partial class ViolationManagementContext : DbContext
{
    public ViolationManagementContext()
    {
    }

    public ViolationManagementContext(DbContextOptions<ViolationManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleAddRequest> VehicleAddRequests { get; set; }

    public virtual DbSet<Violation> Violations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json", true, true);
        var configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32A315757D");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.PlateNumber).HasMaxLength(15);
            entity.Property(e => e.SentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.PlateNumberNavigation).WithMany(p => p.Notifications)
                .HasPrincipalKey(p => p.PlateNumber)
                .HasForeignKey(d => d.PlateNumber)
                .HasConstraintName("FK__Notificat__Plate__4F7CD00D");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__4E88ABD4");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48E5E1D67B57");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.ImageUrl).HasColumnName("ImageURL");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.PlateNumber).HasMaxLength(15);
            entity.Property(e => e.ReportDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReporterId).HasColumnName("ReporterID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.VideoUrl).HasColumnName("VideoURL");
            entity.Property(e => e.ViolationType).HasMaxLength(50);

            entity.HasOne(d => d.PlateNumberNavigation).WithMany(p => p.Reports)
                .HasPrincipalKey(p => p.PlateNumber)
                .HasForeignKey(d => d.PlateNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__PlateNu__4316F928");

            entity.HasOne(d => d.ProcessedByNavigation).WithMany(p => p.ReportProcessedByNavigations)
                .HasForeignKey(d => d.ProcessedBy)
                .HasConstraintName("FK__Reports__Process__4222D4EF");

            entity.HasOne(d => d.Reporter).WithMany(p => p.ReportReporters)
                .HasForeignKey(d => d.ReporterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__Reporte__412EB0B6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC9B0D5B0E");

            entity.HasIndex(e => e.CitizenId, "UQ__Users__6E49FBEDFED33E41").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105344296EB5A").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CitizenId)
                .HasMaxLength(20)
                .HasColumnName("CitizenID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicles__476B54B2C03A2E47");

            entity.HasIndex(e => e.PlateNumber, "UQ__Vehicles__0369262400C86D9A").IsUnique();

            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.PlateNumber).HasMaxLength(15);

            entity.HasOne(d => d.Owner).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehicles__OwnerI__3C69FB99");
        });

        modelBuilder.Entity<VehicleAddRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__VehicleA__33A8519A243D51B9");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.PlateNumber).HasMaxLength(15);
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.VehicleAddRequestApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__VehicleAd__Appro__5535A963");

            entity.HasOne(d => d.Owner).WithMany(p => p.VehicleAddRequestOwners)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VehicleAd__Owner__5441852A");
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.ViolationId).HasName("PK__Violatio__18B6DC2868EE4C39");

            entity.Property(e => e.ViolationId).HasColumnName("ViolationID");
            entity.Property(e => e.FineAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FineDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaidStatus).HasDefaultValue(false);
            entity.Property(e => e.PlateNumber).HasMaxLength(15);
            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.ViolatorId).HasColumnName("ViolatorID");

            entity.HasOne(d => d.PlateNumberNavigation).WithMany(p => p.Violations)
                .HasPrincipalKey(p => p.PlateNumber)
                .HasForeignKey(d => d.PlateNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Violation__Plate__48CFD27E");

            entity.HasOne(d => d.Report).WithMany(p => p.Violations)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Violation__Repor__47DBAE45");

            entity.HasOne(d => d.Violator).WithMany(p => p.Violations)
                .HasForeignKey(d => d.ViolatorId)
                .HasConstraintName("FK__Violation__Viola__49C3F6B7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

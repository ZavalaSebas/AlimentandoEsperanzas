using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AlimentandoEsperanzas.Models;

public partial class AlimentandoesperanzasContext : DbContext
{
    public AlimentandoesperanzasContext()
    {
    }

    public AlimentandoesperanzasContext(DbContextOptions<AlimentandoesperanzasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actionlog> Actionlogs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<Donationtype> Donationtypes { get; set; }

    public virtual DbSet<Donor> Donors { get; set; }

    public virtual DbSet<Errorlog> Errorlogs { get; set; }

    public virtual DbSet<Idtype> Idtypes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Itemcategory> Itemcategories { get; set; }

    public virtual DbSet<Paymentmethod> Paymentmethods { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=alimentandoesperanzas;uid=root;password=Dquiros.19", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.3.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Actionlog>(entity =>
        {
            entity.HasKey(e => e.ActionLogId).HasName("PRIMARY");

            entity.ToTable("actionlog");

            entity.HasIndex(e => e.UserId, "FK_ACTION_USERS");

            entity.Property(e => e.ActionLogId).HasColumnName("ActionLogID");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Document).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Actionlogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ACTION_USERS");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Category1)
                .HasMaxLength(50)
                .HasColumnName("Category");
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PRIMARY");

            entity.ToTable("donation");

            entity.HasIndex(e => e.CategoryId, "FK_DONATION_CATEGORY");

            entity.HasIndex(e => e.DonorId, "FK_DONATION_DONOR");

            entity.HasIndex(e => e.PaymentMethodId, "FK_DONATION_PAYMENT");

            entity.HasIndex(e => e.DonationTypeId, "FK_DONATION_TYPE");

            entity.Property(e => e.DonationId).HasColumnName("DonationID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Comments).HasMaxLength(100);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DonationTypeId).HasColumnName("DonationTypeID");
            entity.Property(e => e.DonorId).HasColumnName("DonorID");
            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

            entity.HasOne(d => d.Category).WithMany(p => p.Donations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DONATION_CATEGORY");

            entity.HasOne(d => d.DonationType).WithMany(p => p.Donations)
                .HasForeignKey(d => d.DonationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DONATION_TYPE");

            entity.HasOne(d => d.Donor).WithMany(p => p.Donations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DONATION_DONOR");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Donations)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DONATION_PAYMENT");
        });

        modelBuilder.Entity<Donationtype>(entity =>
        {
            entity.HasKey(e => e.DonationTypeId).HasName("PRIMARY");

            entity.ToTable("donationtype");

            entity.Property(e => e.DonationTypeId).HasColumnName("DonationTypeID");
            entity.Property(e => e.DonationType1)
                .HasMaxLength(50)
                .HasColumnName("DonationType");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PRIMARY");

            entity.ToTable("donor");

            entity.HasIndex(e => e.IdentificationType, "FK_DONOR_TYPEID");

            entity.Property(e => e.DonorId).HasColumnName("DonorID");
            entity.Property(e => e.Comments).HasMaxLength(100);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.IdNumber).HasMaxLength(30);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.IdentificationTypeNavigation).WithMany(p => p.Donors)
                .HasForeignKey(d => d.IdentificationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DONOR_TYPEID");
        });

        modelBuilder.Entity<Errorlog>(entity =>
        {
            entity.HasKey(e => e.ErrorLogId).HasName("PRIMARY");

            entity.ToTable("errorlog");

            entity.HasIndex(e => e.UserId, "FK_ERROR_USERS");

            entity.Property(e => e.ErrorLogId).HasColumnName("ErrorLogID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Errorlogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ERROR_USERS");
        });

        modelBuilder.Entity<Idtype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("idtype");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("item");

            entity.HasIndex(e => e.Category, "FK_Category_Item");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Category_Item");
        });

        modelBuilder.Entity<Itemcategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("itemcategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<Paymentmethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PRIMARY");

            entity.ToTable("paymentmethod");

            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.PaymentMethod1)
                .HasMaxLength(50)
                .HasColumnName("PaymentMethod");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Role1)
                .HasMaxLength(50)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Role, "FK_USERS_ROLE");

            entity.HasIndex(e => e.IdentificationType, "FK_USER_TYPEID");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.IdNumber).HasMaxLength(30);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);

            entity.HasOne(d => d.IdentificationTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdentificationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_TYPEID");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USERS_ROLE");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.UserRolesId).HasName("PRIMARY");

            entity.ToTable("userroles");

            entity.HasIndex(e => e.RoleId, "FK_ROLE_USERROLE");

            entity.HasIndex(e => e.UserId, "FK_USERS_USERROLE");

            entity.Property(e => e.UserRolesId).HasColumnName("UserRolesID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany(p => p.Userroles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_ROLE_USERROLE");

            entity.HasOne(d => d.User).WithMany(p => p.Userroles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_USERS_USERROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

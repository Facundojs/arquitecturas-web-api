using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace entrega_final_arquitecturas_web.DAL.Models;

public partial class DbCtx : DbContext
{
    public DbCtx()
    {
    }

    public DbCtx(DbContextOptions<DbCtx> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Privilege> Privileges { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersPrivilege> UsersPrivileges { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07FFC0370D");

            entity.Property(e => e.Author).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Privilege>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Privileg__3214EC070DB96241");

            entity.HasIndex(e => e.Name, "UQ__Privileg__737584F69A869E3B").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07B8F64306");

            entity.ToTable("RefreshToken");

            entity.HasIndex(e => e.Token, "UQ__RefreshT__1EB4F8178B70BDC8").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__RefreshTo__UserI__6EF57B66");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07C6919006");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105343F85C4E8").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Salt).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<UsersPrivilege>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersPri__3214EC071BF2DA80");

            entity.HasIndex(e => new { e.UserId, e.PrivilegeId }, "UQ__UsersPri__CCB6BBA83FD97553").IsUnique();

            entity.HasOne(d => d.Privilege).WithMany(p => p.UsersPrivileges)
                .HasForeignKey(d => d.PrivilegeId)
                .HasConstraintName("FK__UsersPriv__Privi__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.UsersPrivileges)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UsersPriv__UserI__72C60C4A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

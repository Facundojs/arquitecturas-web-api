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
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC074C3D8300");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Books)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Books__UserId__49C3F6B7");
        });

        modelBuilder.Entity<Privilege>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Privileg__3214EC0756CB7EDB");

            entity.HasIndex(e => e.Description, "UQ__Privileg__4EBBBAC9E8C9E423").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07A6DA7DB5");

            entity.ToTable("RefreshToken");

            entity.HasIndex(e => e.Token, "UQ__RefreshT__1EB4F817E8F573C5").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__RefreshTo__UserI__4222D4EF");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07BBD8B952");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F284561E7F3C3C").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Salt).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
        });

        modelBuilder.Entity<UsersPrivilege>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersPri__3214EC07C1B84258");

            entity.HasIndex(e => new { e.UserId, e.PrivilegeId }, "UQ__UsersPri__CCB6BBA8D7FB9D82").IsUnique();

            entity.HasOne(d => d.Privilege).WithMany(p => p.UsersPrivileges)
                .HasForeignKey(d => d.PrivilegeId)
                .HasConstraintName("FK__UsersPriv__Privi__46E78A0C");

            entity.HasOne(d => d.User).WithMany(p => p.UsersPrivileges)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UsersPriv__UserI__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

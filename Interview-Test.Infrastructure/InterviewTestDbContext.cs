using Interview_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Interview_Test.Infrastructure;

public class InterviewTestDbContext : DbContext
{
    public InterviewTestDbContext(DbContextOptions<InterviewTestDbContext> options) : base(options)
    {
    }

    // 5 ตาราง: 3 master + 2 junction
    public DbSet<UserModel> Users { get; set; }
    public DbSet<RoleModel> Roles { get; set; }
    public DbSet<PermissionModel> Permissions { get; set; }
    public DbSet<UserRoleModel> UserRoles { get; set; }
    public DbSet<RolePermissionModel> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User ↔ Role (many-to-many)
        modelBuilder.Entity<UserRoleModel>(entity =>
        {
            entity.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();

            entity.HasOne(ur => ur.User)
                  .WithMany(u => u.UserRoles)
                  .HasForeignKey(ur => ur.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ur => ur.Role)
                  .WithMany(r => r.UserRoles)
                  .HasForeignKey(ur => ur.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Role ↔ Permission (many-to-many)
        modelBuilder.Entity<RolePermissionModel>(entity =>
        {
            entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();

            entity.HasOne(rp => rp.Role)
                  .WithMany(r => r.RolePermissions)
                  .HasForeignKey(rp => rp.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rp => rp.Permission)
                  .WithMany(p => p.RolePermissions)
                  .HasForeignKey(rp => rp.PermissionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // unique permission ใน master
        modelBuilder.Entity<PermissionModel>()
                    .HasIndex(p => p.Permission)
                    .IsUnique();
    }
}

// design-time factory (ใช้ตอน dotnet ef migrations เท่านั้น)
public class InterviewTestDbContextDesignFactory : IDesignTimeDbContextFactory<InterviewTestDbContext>
{
    public InterviewTestDbContext CreateDbContext(string[] args)
    {
        const string connectionString =
            "Server=localhost,1433;Database=InterviewTestDb;User=sa;Password=@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true";
        var optionsBuilder = new DbContextOptionsBuilder<InterviewTestDbContext>()
            .UseSqlServer(connectionString, opts => opts.CommandTimeout(600));

        return new InterviewTestDbContext(optionsBuilder.Options);
    }
}

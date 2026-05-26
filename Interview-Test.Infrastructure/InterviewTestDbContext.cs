using Interview_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Interview_Test.Infrastructure;

public class InterviewTestDbContext : DbContext
{
    public InterviewTestDbContext(DbContextOptions<InterviewTestDbContext> options) : base(options)
    {
    }

    public DbSet<UserModel> UserTb { get; set; }
    public DbSet<UserProfileModel> UserProfileTb { get; set; }
    public DbSet<RoleModel> RoleTb { get; set; }
    public DbSet<UserRoleMappingModel> UserRoleMappingTb { get; set; }
    public DbSet<PermissionModel> PermissionTb { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.HasOne(u => u.UserProfile)
                  .WithOne(p => p.User)
                  .HasForeignKey<UserProfileModel>("UserId")
                  .HasPrincipalKey<UserModel>(u => u.Id)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserRoleMappingModel>(entity =>
        {
            entity.HasOne(urm => urm.User)
                  .WithMany(u => u.UserRoleMappings)
                  .HasForeignKey("UserId")
                  .HasPrincipalKey(u => u.Id)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(urm => urm.Role)
                  .WithMany(r => r.UserRoleMappings)
                  .HasForeignKey("RoleId")
                  .HasPrincipalKey(r => r.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RoleModel>(entity =>
        {
            entity.HasMany(r => r.Permissions)
                  .WithOne(p => p.Role)
                  .HasForeignKey("RoleId")
                  .HasPrincipalKey(r => r.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

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

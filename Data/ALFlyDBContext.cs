using ALFly.Models;
using Microsoft.EntityFrameworkCore;

namespace ALFly.Data
{
    public class ALFlyDBContext : DbContext
    {
        public ALFlyDBContext(DbContextOptions<ALFlyDBContext> options) : base(options) { }

        public DbSet<Agents> Agents { get; set; } = default!;
        public DbSet<Permissions> Permissions { get; set; } = default!;
        public DbSet<AgentPermission> AgentPermission { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agents>()
                .Property(a => a.Role)
                .HasConversion<string>();

            modelBuilder.Entity<AgentPermission>()
                .HasKey(ap => new { ap.Id, ap.PermissionId });

            modelBuilder.Entity<AgentPermission>()
                .HasOne(ap => ap.Agent)
                .WithMany(a => a.AgentPermissions)
                .HasForeignKey(ap => ap.Id);

            modelBuilder.Entity<AgentPermission>()
                .HasOne(ap => ap.Permission)
                .WithMany(p => p.AgentPermissions)
                .HasForeignKey(ap => ap.PermissionId);

        }


    }
}

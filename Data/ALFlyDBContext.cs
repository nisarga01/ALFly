using ALFly.Models;
using Microsoft.EntityFrameworkCore;

namespace ALFly.Data
{
    public class ALFlyDBContext : DbContext
    {
        public ALFlyDBContext(DbContextOptions<ALFlyDBContext> options) : base(options) { }

        public DbSet<Agents> Agents{ get; set; } = default!;

    }
}

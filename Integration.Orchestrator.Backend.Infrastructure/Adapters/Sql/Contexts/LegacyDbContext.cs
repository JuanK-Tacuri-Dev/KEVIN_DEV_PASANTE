using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Integration.Orchestrator.Backend.Infrastructure.DataAccess.Sql.Contexts
{
    public partial class LegacyDbContext : DbContext
    {
        public LegacyDbContext()
        {
        }

        public LegacyDbContext(DbContextOptions<LegacyDbContext> options)
            : base(options)
        {
        }
              

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

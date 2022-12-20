using Microsoft.EntityFrameworkCore;
using Notino.Domain.Entities;
using Notino.Persistence.MSSQL.Repositories.Common;

namespace Notino.Persistence.MSSQL
{
    public class NotinoDbContext: BaseDbContext
    {
        public NotinoDbContext(DbContextOptions<NotinoDbContext> dbContextOptions) 
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(NotinoDbContext).Assembly);
           
        }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<DocumentTag> DocumentTags { get; set; }
    }
}

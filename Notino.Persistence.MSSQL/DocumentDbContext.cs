using Microsoft.EntityFrameworkCore;
using Notino.Domain;

namespace Notino.Persistence.MSSQL
{
    public class DocumentDbContext:DbContext
    {
        public DocumentDbContext(DbContextOptions<DocumentDbContext> dbContextOptions) 
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(DocumentDbContext).Assembly);
        }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<DocumentTag> DocumentTags { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Notino.Domain.Entities.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Notino.Persistence.MSSQL.Repositories.Common
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            foreach (var entry in base.ChangeTracker.Entries<IAuditableEntity>()
                .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                entry.Entity.LastModifiedDate = DateTime.Now;                

                if (entry.State == EntityState.Added)                
                    entry.Entity.DateCreated = DateTime.Now;                  
            }

            var result = await base.SaveChangesAsync();

            return result;
        }
    }
}

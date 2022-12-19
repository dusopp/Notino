using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

using System.Text;

namespace Notino.Persistence.MSSQL.Configurations.Entities
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Domain.Document>
    {
        public void Configure(EntityTypeBuilder<Domain.Document> builder)
        {
            builder.HasKey(d => new { d.Id, d.InternalId });
            builder.Property(x => x.InternalId).UseIdentityColumn();
        }
    }
}

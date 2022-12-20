using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notino.Domain.Entities;
using System;
using System.Collections.Generic;

using System.Text;

namespace Notino.Persistence.MSSQL.Configurations.Entities
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(d => new { d.Id, d.InternalId });
            builder.Property(x => x.InternalId).UseIdentityColumn();
        }
    }
}

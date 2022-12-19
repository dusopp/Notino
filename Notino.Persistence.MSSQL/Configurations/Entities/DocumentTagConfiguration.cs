using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Persistence.MSSQL.Configurations.Entities
{
    public class DocumentTagConfiguration : IEntityTypeConfiguration<DocumentTag>
    {
        public void Configure(EntityTypeBuilder<DocumentTag> builder)
        {
            builder.HasKey(i => new { i.DocumentId, i.DocumentInternalId, i.TagId });
        }
    }
}

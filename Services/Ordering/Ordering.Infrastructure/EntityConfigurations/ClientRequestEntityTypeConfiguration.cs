using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Idempotency;

namespace Ordering.Infrastructure.EntityConfigurations {
    
    class ClientRequestEntityTypeConfiguration : IEntityTypeConfiguration<ClientRequest> {
        public void Configure(EntityTypeBuilder<ClientRequest> builder) {
            builder.ToTable("requests", OrderingContext.DEFAULT_SCHEMA);

            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.Name)
                .IsRequired();

            builder.Property(cr => cr.Time)
                .IsRequired();        
        }
    }
}
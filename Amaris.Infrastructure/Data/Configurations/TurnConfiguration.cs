using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Amaris.Domain.Entities;

namespace Amaris.Infrastructure.Data.Configurations
{
    public class TurnConfiguration : IEntityTypeConfiguration<Turn>
    {
        public void Configure(EntityTypeBuilder<Turn> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Identification).IsRequired().HasMaxLength(20);
            builder.Property(t => t.TurnCode).IsRequired().HasMaxLength(50);
            builder.Property(t => t.Status).HasConversion<string>();
            builder.HasOne(t => t.Location)
                   .WithMany(s => s.Turns)
                   .HasForeignKey(t => t.IdLocation);
            builder.HasIndex(t => new { t.Identification, t.DateCreation });
        }
    }
}

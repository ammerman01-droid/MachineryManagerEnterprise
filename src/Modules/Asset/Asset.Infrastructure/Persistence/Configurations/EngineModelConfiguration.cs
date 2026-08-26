using Asset.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Asset.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="EngineModel"/> aggregate.</summary>
public sealed class EngineModelConfiguration : IEntityTypeConfiguration<EngineModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<EngineModel> builder)
    {
        builder.ToTable("EngineModel");

        builder.HasKey(em => em.Id);

        builder.Property(em => em.Id)
            .HasConversion(id => id.Value, value => EngineModelId.From(value))
            .ValueGeneratedNever();

        builder.Property(em => em.OrganizationId)
            .IsRequired();

        builder.Property(em => em.Name)
            .HasMaxLength(EngineModel.MaxNameLength)
            .IsRequired();

        builder.Property(em => em.Manufacturer)
            .HasMaxLength(EngineModel.MaxManufacturerLength)
            .IsRequired();
    }
}
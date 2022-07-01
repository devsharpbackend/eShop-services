namespace eShop.Services.CatalogAPI.Infrastructure.EntityConfigurations;

class CatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("CatalogType",schema:"Catalog");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .UseHiLo("catalog_type_hilo")
            .IsRequired();

        builder.Property(cb => cb.Type)
            .IsRequired()
            .HasMaxLength(100);


        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(b => b.DomainEvents);


        builder
           .Property<string>("_type")
           .UsePropertyAccessMode(PropertyAccessMode.Field)
           .HasColumnName("Type")
            .IsRequired(true)
            .HasMaxLength(100);
    }
}

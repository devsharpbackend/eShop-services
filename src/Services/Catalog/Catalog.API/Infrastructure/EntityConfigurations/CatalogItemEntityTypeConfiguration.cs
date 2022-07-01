


namespace eShop.Services.CatalogAPI.Infrastructure.EntityConfigurations;

class CatalogItemEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("Catalog",schema:"Catalog");
       
        builder.HasKey(x => x.Id);
        builder.Property(ci => ci.Id)
            .UseHiLo("catalog_hilo")
            .IsRequired();

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(b => b.DomainEvents);

      
        builder
           .Property<string>("_name")
           .UsePropertyAccessMode(PropertyAccessMode.Field)
           .HasColumnName("Name")
            .IsRequired(true)
            .HasMaxLength(50);

        builder
          .Property<string>("_description")
          .UsePropertyAccessMode(PropertyAccessMode.Field)
          .HasColumnName("Description")
           .IsRequired(false);

        builder
         .Property<string>("_pictureFileName")
         .UsePropertyAccessMode(PropertyAccessMode.Field)
         .HasColumnName("PictureFileName")
          .IsRequired(false);


        builder
        .Property<decimal>("_price")
        .UsePropertyAccessMode(PropertyAccessMode.Field)
        .HasColumnName("Price")
         .IsRequired(true);


        builder
     .Property<decimal>("_priceWithDiscount")
     .UsePropertyAccessMode(PropertyAccessMode.Field)
     .HasColumnName("PriceWithDiscount")
      .IsRequired(true);

        builder
    .Property<decimal>("_discount")
    .UsePropertyAccessMode(PropertyAccessMode.Field)
    .HasColumnName("Discount")
     .IsRequired(true);

        builder
     .Property<bool>("_isDiscount")
     .UsePropertyAccessMode(PropertyAccessMode.Field)
     .HasColumnName("IsDiscount")
      .IsRequired(true);


        builder
   .Property<int>("_catalogTypeId")
   .UsePropertyAccessMode(PropertyAccessMode.Field)
   .HasColumnName("CatalogTypeId")
    .IsRequired(true);

        builder
   .Property<int>("_availableStock")
   .UsePropertyAccessMode(PropertyAccessMode.Field)
   .HasColumnName("AvailableStock")
    .IsRequired(true);


        builder
  .Property<int>("_stockThreshold")
  .UsePropertyAccessMode(PropertyAccessMode.Field)
  .HasColumnName("StockThreshold")
   .IsRequired(true);

        builder
          .Property<int>("_maxStockThreshold")
          .UsePropertyAccessMode(PropertyAccessMode.Field)
          .HasColumnName("MaxStockThreshold")
           .IsRequired(true);

        builder.HasOne(ci => ci.CatalogType)
            .WithMany()
            .HasForeignKey("_catalogTypeId");
    }
}

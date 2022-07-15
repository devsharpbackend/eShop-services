


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
          .Property(ci => ci.Name)
          .HasField("_name")
          .UsePropertyAccessMode(PropertyAccessMode.Field)
          .HasColumnName("Name")
           .IsRequired(true)
           .HasMaxLength(50);

        //builder
        //   .Property<string>("_name")
        //   .UsePropertyAccessMode(PropertyAccessMode.Field)
        //   .HasColumnName("Name")
        //    .IsRequired(true)
        //    .HasMaxLength(50);

        builder
           
          .Property(p=>p.Description)
          .HasField("_description")
          .UsePropertyAccessMode(PropertyAccessMode.Field)
          .HasColumnName("Description")
          .IsRequired(false);

        builder.Property(p=>p.PictureFileName)
         .HasField("_pictureFileName")
         .UsePropertyAccessMode(PropertyAccessMode.Field)
         .HasColumnName("PictureFileName")
          .IsRequired(false);


        builder
         .Property(p=>p.Price).HasField("_price")
         .UsePropertyAccessMode(PropertyAccessMode.Field)
         .HasColumnName("Price")
         .IsRequired(true);


        builder.Property(p=>p.PriceWithDiscount).HasField("_priceWithDiscount")
         .UsePropertyAccessMode(PropertyAccessMode.Field)
         .HasColumnName("PriceWithDiscount")
          .IsRequired(true);

        builder
    .Property<decimal>("_discount")
    .UsePropertyAccessMode(PropertyAccessMode.Field)
    .HasColumnName("Discount")
     .IsRequired(true);

      builder.Property(p=>p.IsDiscount).HasField("_isDiscount")
     
     .UsePropertyAccessMode(PropertyAccessMode.Field)
     .HasColumnName("IsDiscount")
      .IsRequired(true);


        builder.Property(p=>p.CatalogTypeId).HasField("_catalogTypeId")
             .UsePropertyAccessMode(PropertyAccessMode.Field)
             .HasColumnName("CatalogTypeId")
              .IsRequired(true);

        builder.Property(p=>p.AvailableStock).HasField("_availableStock")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("AvailableStock")
             .IsRequired(true);


        builder.Property(p=>p.StockThreshold).HasField("_stockThreshold")
        .UsePropertyAccessMode(PropertyAccessMode.Field)
        .HasColumnName("StockThreshold")
         .IsRequired(true);

        builder.Property(p=>p.MaxStockThreshold).HasField("_maxStockThreshold")
          .UsePropertyAccessMode(PropertyAccessMode.Field)
          .HasColumnName("MaxStockThreshold")
           .IsRequired(true);

        builder.HasOne<CatalogType>()
            .WithMany()
            .HasForeignKey("CatalogTypeId");
    }
}

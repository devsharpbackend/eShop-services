


namespace eShop.Services.CatalogAPI.Infrastructure.EntityConfigurations;

public class SupplierItemEntityTypeConfiguration
    : IEntityTypeConfiguration<SupplierItem>
{
    public void Configure(EntityTypeBuilder<SupplierItem> builder)
    {
        builder.ToTable("SupplierItem", schema: "Supplier");

        builder.HasKey(x => x.Id);
        builder.Property(ci => ci.Id)
            .UseHiLo("supplierItem_hilo")
            .IsRequired();

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(b => b.DomainEvents);



        builder.Property(p=>p.CatalogItemId).HasField("_catalogItemId")
           .UsePropertyAccessMode(PropertyAccessMode.Field)
           .HasColumnName("CatalogItemId")
           .IsRequired(true);


        builder.Property(p=>p.RequestedNumber).HasField("_requestedNumber")
             .UsePropertyAccessMode(PropertyAccessMode.Field)
             .HasColumnName("RequestedNumber")
             .IsRequired(true);


        builder.Property(p=>p.SupplierId).HasField("_supplierId")
           .UsePropertyAccessMode(PropertyAccessMode.Field)
           .HasColumnName("SupplierId")
           .IsRequired(true);

        

        builder.HasOne<CatalogItem>()
          .WithMany()
          .HasForeignKey("CatalogItemId");

        builder.HasOne<Supplier>()
         .WithMany(p => p.SupplierItems)
         .HasForeignKey(p => p.SupplierId);


    }
}

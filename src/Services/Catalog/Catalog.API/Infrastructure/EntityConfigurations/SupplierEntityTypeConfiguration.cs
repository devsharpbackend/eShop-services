


namespace eShop.Services.CatalogAPI.Infrastructure.EntityConfigurations;

public class SupplierEntityTypeConfiguration
    : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {

        builder.ToTable("Supplier", schema: "Supplier");

        builder.HasKey(x => x.Id);
        builder.Property(ci => ci.Id)
            .UseHiLo("supplier_hilo")
            .IsRequired();

        builder.UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(b => b.DomainEvents);

        builder
       .Property(p=>p.CatalogTypeId)
       .HasField("_catalogTypeId")
       .UsePropertyAccessMode(PropertyAccessMode.Field)
       .HasColumnName("CatalogTypeId")
       .IsRequired(true);

       
        builder.Property(p=>p.SupplierName).HasField("_supplierName")
         .UsePropertyAccessMode(PropertyAccessMode.Field)
         .HasColumnName("SupplierName")
         .IsRequired(true);


        //When we have a Navigation as ReadOnly that points to the field, it should be written as shown below
        var navigation = builder.Metadata.FindNavigation(nameof(Supplier.SupplierItems));
        navigation.SetPropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        //builder.Property(p => p.SupplierItems).HasField("_supplieItems")
        //    .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        builder.HasOne<CatalogType>()
            .WithMany()
            .HasForeignKey("_catalogTypeId").OnDelete(DeleteBehavior.NoAction);



    }
}

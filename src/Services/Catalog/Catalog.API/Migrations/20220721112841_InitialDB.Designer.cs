﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eShop.Services.CatalogAPI.Infrastructure;

#nullable disable

namespace Catalog.API.Migrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20220721112841_InitialDB")]
    partial class InitialDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.HasSequence("catalog_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("catalog_type_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("supplier_hilo")
                .IncrementsBy(10);

            modelBuilder.HasSequence("supplierItem_hilo")
                .IncrementsBy(10);

            modelBuilder.Entity("eShop.Services.CatalogAPI.Domain.AggregatesModel.CatalogAggregate.CatalogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "catalog_hilo");

                    b.Property<int>("AvailableStock")
                        .HasColumnType("int")
                        .HasColumnName("AvailableStock");

                    b.Property<int>("CatalogTypeId")
                        .HasColumnType("int")
                        .HasColumnName("CatalogTypeId");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Description");

                    b.Property<bool>("IsDiscount")
                        .HasColumnType("bit")
                        .HasColumnName("IsDiscount");

                    b.Property<int>("MaxStockThreshold")
                        .HasColumnType("int")
                        .HasColumnName("MaxStockThreshold");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Name");

                    b.Property<string>("PictureFileName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PictureFileName");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Price");

                    b.Property<decimal>("PriceWithDiscount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("PriceWithDiscount");

                    b.Property<int>("StockThreshold")
                        .HasColumnType("int")
                        .HasColumnName("StockThreshold");

                    b.Property<int>("_catalogTypeId")
                        .HasColumnType("int");

                    b.Property<decimal>("_discount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Discount");

                    b.HasKey("Id");

                    b.HasIndex("_catalogTypeId");

                    b.ToTable("Catalog", "Catalog");
                });

            modelBuilder.Entity("eShop.Services.CatalogAPI.Domain.AggregatesModel.CatalogTypeAggregate.CatalogType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "catalog_type_hilo");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Type");

                    b.HasKey("Id");

                    b.ToTable("CatalogType", "Catalog");
                });

            modelBuilder.Entity("eShop.Services.CatalogAPI.Domain.AggregatesModel.SupplierAggregate.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "supplier_hilo");

                    b.Property<int>("CatalogTypeId")
                        .HasColumnType("int")
                        .HasColumnName("CatalogTypeId");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("SupplierName");

                    b.Property<int>("_catalogTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("_catalogTypeId");

                    b.ToTable("Supplier", "Supplier");
                });

            modelBuilder.Entity("eShop.Services.CatalogAPI.Domain.AggregatesModel.SupplierAggregate.SupplierItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "supplierItem_hilo");

                    b.Property<int>("CatalogItemId")
                        .HasColumnType("int")
                        .HasColumnName("CatalogItemId");

                    b.Property<int>("RequestedNumber")
                        .HasColumnType("int")
                        .HasColumnName("RequestedNumber");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int")
                        .HasColumnName("SupplierId");

                    b.Property<int>("_catalogItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.HasIndex("_catalogItemId");

                    b.ToTable("SupplierItem", "Supplier");
                });

            modelBuilder.Entity("eShop.Services.CatalogAPI.Domain.AggregatesModel.CatalogAggregate.CatalogItem", b =>
                {
                    b.HasOne("eShop.Services.CatalogAPI.Domain.AggregatesModel.CatalogTypeAggregate.CatalogType", null)
                        .WithMany()
                        .HasForeignKey("_catalogTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("eShop.Services.CatalogAPI.Domain.AggregatesModel.SupplierAggregate.Supplier", b =>
                {
                    b.HasOne("eShop.Services.CatalogAPI.Domain.AggregatesModel.CatalogTypeAggregate.CatalogType", null)
                        .WithMany()
                        .HasForeignKey("_catalogTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("eShop.Services.CatalogAPI.Domain.AggregatesModel.SupplierAggregate.SupplierItem", b =>
                {
                    b.HasOne("eShop.Services.CatalogAPI.Domain.AggregatesModel.SupplierAggregate.Supplier", null)
                        .WithMany("SupplierItems")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eShop.Services.CatalogAPI.Domain.AggregatesModel.CatalogAggregate.CatalogItem", null)
                        .WithMany()
                        .HasForeignKey("_catalogItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("eShop.Services.CatalogAPI.Domain.AggregatesModel.SupplierAggregate.Supplier", b =>
                {
                    b.Navigation("SupplierItems");
                });
#pragma warning restore 612, 618
        }
    }
}

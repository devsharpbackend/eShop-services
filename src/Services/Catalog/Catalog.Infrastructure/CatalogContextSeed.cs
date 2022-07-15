

namespace eShop.Services.Catalog.Infrastructure;

public class CatalogContextSeed
{
    public async Task MagirateAndSeedAsync(CatalogContext context, IWebHostEnvironment env, ILogger<CatalogContextSeed> logger)
    {
        await context.Database.MigrateAsync();

       
        var contentRootPath = env.ContentRootPath;
        var picturePath = env.WebRootPath;

        if (!context.CatalogTypes.Any())
        {
            await context.CatalogTypes.AddRangeAsync(SeedCatalogTypes());

            await context.SaveChangesAsync();
        }

        if (!context.CatalogItems.Any())
        {
            await context.CatalogItems.AddRangeAsync(SeedCatalogItems());

            await context.SaveChangesAsync();

            GetCatalogItemPictures(contentRootPath, picturePath);
        }

        if (!context.Suppliers.Any())
        {
            await context.Suppliers.AddRangeAsync(SeedSupplier());

            await context.SaveChangesAsync();
        }
    }

    private IEnumerable<CatalogType> SeedCatalogTypes()
    {
        return new List<CatalogType>()
        {
            new CatalogType("Mug") ,
            new CatalogType("T-Shirt") ,
            new CatalogType( "Sheet") ,
            new CatalogType( "USB Memory Stick") 
        };
    }
    private IEnumerable<Supplier> SeedSupplier()
    {
        return new List<Supplier>()
        {
            new Supplier(supplierName:"supplier1",catalogTypeId:2),
            new Supplier(supplierName:"supplier2",catalogTypeId:2),
        };
    }

    private IEnumerable<CatalogItem> SeedCatalogItems()
    {
        return new List<CatalogItem>()
        {
             new CatalogItem (name: ".NET Bot Black Hoodie",description:".NET Bot Black Hoodie",price:19.5M,isDiscount:true,pictureFileName: "1.png"
             ,catalogTypeId:2,availableStock:100,stockThreshold:5,maxStockThreshold:10000),
             new CatalogItem (name:".NET Black & White Mug",description:".NET Black & White Mug",price:12.5M,isDiscount:true,pictureFileName: "2.png"
             ,catalogTypeId:2,availableStock:100,stockThreshold:5,maxStockThreshold:10000),

             new CatalogItem (name: "Prism White T-Shirt",description: "Prism White T-Shirt",price:12.5M,isDiscount:true,pictureFileName: "3.png"
             ,catalogTypeId:2,availableStock:100,stockThreshold:5,maxStockThreshold:10000),

             new CatalogItem (name: ".NET Foundation T-shirt",description: ".NET Foundation T-shirt",price:13.5M,isDiscount:true,pictureFileName: "4.png"
             ,catalogTypeId:2,availableStock:100,stockThreshold:5,maxStockThreshold:10000),

              new CatalogItem (name: ".Roslyn Red Sheet",description: "Roslyn Red Sheet",price:8.5M,isDiscount:true,pictureFileName: "5.png"
             ,catalogTypeId:2,availableStock:100,stockThreshold:5,maxStockThreshold:10000),

                new CatalogItem (name: ".NET Blue Hoodie",description: ".NET Blue Hoodie",price:8.5M,isDiscount:true,pictureFileName: "6.png"
             ,catalogTypeId:2,availableStock:100,stockThreshold:5,maxStockThreshold:10000),


           
        };
    }

    private void GetCatalogItemPictures(string contentRootPath, string picturePath)
    {
        if (picturePath != null)
        {
            DirectoryInfo directory = new DirectoryInfo(picturePath);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            string zipFileCatalogItemPictures = Path.Combine(contentRootPath, "Setup", "CatalogItems.zip");
            ZipFile.ExtractToDirectory(zipFileCatalogItemPictures, picturePath);
        }
    }

}


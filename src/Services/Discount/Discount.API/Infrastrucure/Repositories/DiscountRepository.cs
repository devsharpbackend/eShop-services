namespace  eShop.Services.Discoun.DiscountAPIe.Repositories;


public class DiscountRepository : IDiscountRepository
{
   // private readonly IConfiguration _configuration;
    private readonly IOptionsSnapshot<DiscountSetting> _options;
    public DiscountRepository(IOptionsSnapshot<DiscountSetting> options)
    {
        this._options = options ?? throw new ArgumentNullException(nameof(options));

    }
    public async Task<CatalogDiscount> GetDiscount(int CatalogId)
    {            
        using var connection = new NpgsqlConnection(_options.Value.ConnectionString);

        var coupon = await connection.QueryFirstOrDefaultAsync<CatalogDiscount>
            ("SELECT * FROM CatalogDiscount WHERE CatalogId = @CatalogId and IsActive=true", new { CatalogId = CatalogId });

        if (coupon == null)
            return new CatalogDiscount {Name="", CatalogId = 0, Amount = 0, Description = "No Discount Desc" };

        return coupon;
    }

    public async Task<bool> CreateDiscount(CatalogDiscount coupon)
    {
        using var connection = new NpgsqlConnection(_options.Value.ConnectionString);

        var affected =
            await connection.ExecuteAsync
                ("INSERT INTO CatalogDiscount (CatalogId,Name, Description, Amount,IsActive) VALUES (@CatalogId,@Name, @Description, @Amount,@IsActive)",
                        new { CatalogId = coupon.CatalogId, Name= coupon.Name, Description = coupon.Description, Amount = coupon.Amount,
                        IsActive=coupon.IsActive
                        });

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> UpdateDiscount(CatalogDiscount coupon)
    {
        using var connection = new NpgsqlConnection(_options.Value.ConnectionString);

        var affected = await connection.ExecuteAsync
                ("UPDATE CatalogDiscount SET CatalogId=@CatalogId,Name=@Name, Description = @Description, Amount = @Amount,IsActive=@IsActive WHERE Id = @Id",
                        new { CatalogId = coupon.CatalogId, IsActive=coupon.IsActive, Name=coupon.Name, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteDiscount(int Id)
    {
        using var connection = new NpgsqlConnection(_options.Value.ConnectionString);

        var affected = await connection.ExecuteAsync("DELETE FROM CatalogDiscount WHERE Id = @Id",
            new { Id = Id });

        if (affected == 0)
            return false;

        return true;
    }
}

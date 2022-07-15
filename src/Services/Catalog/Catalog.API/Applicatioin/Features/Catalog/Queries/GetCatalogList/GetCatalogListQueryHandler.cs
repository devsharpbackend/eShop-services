




namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Queries.GetCatalogList;

public class GetCatalogListQueryHandler : IRequestHandler<GetCatalogListQuery, CatalogListVM>
{
    private readonly IOptionsSnapshot<CatalogSetting> options;

    public GetCatalogListQueryHandler(IOptionsSnapshot<CatalogSetting> options)
    {
        this.options = options?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<CatalogListVM> Handle(GetCatalogListQuery request, CancellationToken cancellationToken)
    {
        using (var connection = new SqlConnection(options.Value.ConnectionString))
        {
            connection.Open();

            CatalogListVM catalogListVM = new CatalogListVM();
            
            catalogListVM.TotalCount
                = await connection.QuerySingleAsync<long>
                (@"Select  Count(*)  From  Catalog.Catalog  ");
          
            
            catalogListVM.Data= await connection.QueryAsync<CatalogListItemVM>(@"select cat.IsDiscount, cat.Name,cat.Id,cat.Description,cat.Price,cat.AvailableStock,type.Type as CatalogTypeName
                    from [Catalog].Catalog as cat inner join 
                    [Catalog].CatalogType as type on cat.CatalogTypeId=type.Id
                    order by cat.Name
               OFFSET @PageIndex ROWS FETCH NEXT @PageCount ROWS ONLY ", new { request.PageCount,request.PageIndex });

            // get discount from discount microservice


            foreach (var catalog in catalogListVM.Data)
            {
                if(catalog.IsDiscount)
                {
                    var discount=await GetDiscounts(catalog.Id);
                    if(discount?.Amount>0)
                    {
                        catalog.Discount = discount.Amount;
                    }
                }
            }

            return catalogListVM;
        }
    }
    public async Task<DiscountItemDto?> GetDiscounts(int catalogId)
    {
        using HttpClient client = new HttpClient();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation");


        var streamTask = client.GetStreamAsync($"http://discount-api/api/Discount/{catalogId}");
        var discount = await JsonSerializer.DeserializeAsync<DiscountItemDto>(await streamTask);


        return discount;

    }

    public class DiscountItemDto
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }

}

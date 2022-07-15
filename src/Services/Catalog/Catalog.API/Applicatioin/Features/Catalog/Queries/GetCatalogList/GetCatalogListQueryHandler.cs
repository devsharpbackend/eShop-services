


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
          
            
            catalogListVM.Data= await connection.QueryAsync<CatalogListItemVM>(@"select cat.Name,cat.Description,cat.Price,cat.AvailableStock,type.Type as CatalogTypeName
                    from [Catalog].Catalog as cat inner join 
                    [Catalog].CatalogType as type on cat.CatalogTypeId=type.Id
                    order by cat.Name
               OFFSET @PageIndex ROWS FETCH NEXT @PageCount ROWS ONLY ", new { request.PageCount,request.PageIndex });

            return catalogListVM;
        }
    }
}



public class CatalogService : CatalogGrpc.Catalog.CatalogBase
{
    private readonly IMediator _mediator;

    public CatalogService(IMediator mediator)
    {
        _mediator = mediator;
    }



    public async override Task<PaginatedItemsResponse> GetItemsByIds(CatalogItemsIdsRequest request, global::Grpc.Core.ServerCallContext context)
    {

        var list = await _mediator.Send(
          new GetCatalogListQuery { PageCount = 2000, PageIndex = 1 });

        var numIds = request.Ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));


        var idsToSelect = numIds
            .Select(id => id.Value);


        list.Data = list.Data.Where(p => idsToSelect.Contains(p.Id));

        var response = new PaginatedItemsResponse();

        response.Count = list.TotalCount;

        foreach (var item in list.Data)
        {
            response.Data.Add(new CatalogItemResponse
            {
                AvailableStock = item.AvailableStock,
                CatalogTypeName = item.CatalogTypeName,
                Description = item.Description,
                Discount = (double)item.Discount,
                Id = item.Id,
                IsDiscount = item.IsDiscount,
                Name = item.Name,
                Price = (double)item.Price

            });
        }

        return response;

    }

    public override async Task<CatalogItemResponse> GetItemById(CatalogItemRequest request, ServerCallContext context)
    {
        var item = await _mediator.Send(new GetCatalogQuery { CatalogId = request.Id });

        return new CatalogItemResponse
        {
            AvailableStock = item.AvailableStock,
            CatalogTypeName = "",
            Description = item.Description,
            Discount = (double)item.Discount,
            Id = item.ID,
            IsDiscount = item.IsDiscount,
            Name = item.Name,
            Price = (double)item.Price,
            PictureUrl = item.PictureFileName
        };

    }
    public async override Task<PaginatedItemsResponse> GetItems(CatalogItemsRequest request, global::Grpc.Core.ServerCallContext context)
    {

        var list = await _mediator.Send(
          new GetCatalogListQuery { PageCount = request.PageCount, PageIndex = request.PageIndex });

        var response = new PaginatedItemsResponse();

        response.Count = list.TotalCount;

        foreach (var item in list.Data)
        {
            response.Data.Add(new CatalogItemResponse
            {
                AvailableStock = item.AvailableStock,
                CatalogTypeName = item.CatalogTypeName,
                Description = item.Description,
                Discount = (double)item.Discount,
                Id = item.Id,
                IsDiscount = item.IsDiscount,
                Name = item.Name,
                Price = (double)item.Price

            });
        }

        return response;

    }


    public async override Task<CreateCatalogResponse> RegisterCatalogItem(CreateCatalogRequest request, global::Grpc.Core.ServerCallContext context)
    {
        var command = new CreateCatalogCommand(name: request.Name, description: request.Description,
            price: (decimal)request.Price, isDiscount: request.IsDiscount, pictureFileName: request.PictureFileName
            , catalogTypeId: request.CatalogTypeId, availableStock: request.AvailableStock,
            stockThreshold: request.StockThreshold, maxStockThreshold: request.MaxStockThreshold);


        int id = await _mediator.Send(command);


        return new CreateCatalogResponse { Id = id };
    }
}
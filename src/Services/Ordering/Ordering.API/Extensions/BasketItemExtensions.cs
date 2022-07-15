namespace eShop.Services.Ordering.OrderingAPI.Extensions;

using System.Collections.Generic;
using static eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands.CreateOrderCommand;
public static class BasketItemExtensions
{
    //public static IEnumerable<OrderItemDTO> ToOrderItemsDTO(this IEnumerable<BasketItemIntegrationCommand> basketItems)
    //{
    //    foreach (var item in basketItems)
    //    {
    //        yield return item.ToOrderItemDTO();
    //    }
    //}

    //public static OrderItemDTO ToOrderItemDTO(this BasketItemIntegrationCommand item)
    //{
    //    return new OrderItemDTO()
    //    {
    //        ProductId = item.ProductId,
    //        ProductName = item.ProductName,
    //        PictureUrl = item.PictureUrl,
    //        UnitPrice = item.UnitPrice,
    //        Units = item.Quantity
    //    };
    //}
}

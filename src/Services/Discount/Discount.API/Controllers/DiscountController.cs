
using eShop.BuildingBlocks.Event.CommonEvent.Responses;
using System.Text.RegularExpressions;

namespace eShop.Services.Discount.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _repository;
    //private readonly IZeroMqPublisher _zeroMqPublisher;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IRequestClient<DiscountCreatedIntegrationCommand> _discountCreatedClient;
    public DiscountController(IDiscountRepository repository, IPublishEndpoint publishEndpoint,
        IRequestClient<DiscountCreatedIntegrationCommand> discountCreatedClient
        //, ISendEndpoint sendEndpoint
        /*IZeroMqPublisher zeroMqPublisher*/)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));      
        //_zeroMqPublisher = zeroMqPublisher ?? throw new ArgumentNullException(nameof(zeroMqPublisher));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _discountCreatedClient = discountCreatedClient ?? throw new ArgumentNullException(nameof(discountCreatedClient));
      //  _sendEndpoint = sendEndpoint ?? throw new ArgumentNullException(nameof(sendEndpoint));
    }

    [HttpGet("{catalogId}", Name = "GetDiscount")]
    [ProducesResponseType(typeof(CatalogDiscount), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CatalogDiscount>> GetDiscount(int catalogId)
    {
        var discount = await _repository.GetDiscount(catalogId);
        return Ok(discount);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CatalogDiscount), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CatalogDiscount>> CreateDiscount([FromBody] CatalogDiscount discount,[FromServices] ISendEndpoint sendEndpoint)
    {
        //await _repository.CreateDiscount(discount);
        // _zeroMqPublisher.Publish(new DiscountCreatedIntegrationEvent(discount.CatalogId, discount.Amount));
        // await _publishEndpoint.Publish(new DiscountCreatedIntegrationEvent(discount.CatalogId, discount.Amount));

        //var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:"+nameof(DiscountCreatedIntegrationCommand)));

        ///await _sendEndpoint.Send(new DiscountCreatedIntegrationCommand(discount.CatalogId, discount.Amount));

        var response = await _discountCreatedClient.GetResponse<CreateDiscountValidResponse, JsonErrorResponse>(
             new DiscountCreatedIntegrationCommand(discount.CatalogId, discount.Amount));

        if (response.Is(out Response<JsonErrorResponse> responseA))
        {
            // Remove Discount
            var objectResult = new ObjectResult(responseA.Message);
            objectResult.StatusCode = responseA.Message.StatusCode;
            return objectResult;
        };

        return CreatedAtRoute("GetDiscount", new { catalogId = discount.CatalogId }, discount);
    }
}
public class QueueNames
{
    public const string SagaOrderPayment = "withdraw-customer-credit";
    private const string rabbitUri = "queue:";
    public static Uri GetMessageUri(string key)
    {
        return new Uri(rabbitUri + key.PascalToKebabCaseMessage());
    }
    public static Uri GetActivityUri(string key)
    {
        var kebabCase = key.PascalToKebabCaseActivity();
        if (kebabCase.EndsWith('-'))
        {
            kebabCase = kebabCase.Remove(kebabCase.Length - 1);
        }
        return new Uri(rabbitUri + kebabCase + '_' + "execute");
    }
}
public static class StringExtensions
{
    public static string PascalToKebabCaseMessage(this string value)
    {
        return pascalToKebabCase(value, "message");
    }
    public static string PascalToKebabCaseActivity(this string value)
    {
        return pascalToKebabCase(value, "activity");
    }
    private static string pascalToKebabCase(string value, string postfix)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var result = Regex.Replace(
            value,
            "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
            "-$1",
            RegexOptions.Compiled)
            .Trim()
            .ToLower();

        var segments = result.Split('-');
        if (segments[segments.Length - 1] != postfix)
            return result;
        return result.Substring(0, result.Length - 8);
    }
}
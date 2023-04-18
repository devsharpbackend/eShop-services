namespace eShop.Services.Basket.BasketAPI.Domain.Exceptions;

public class BasketDomainException : Exception
{
    public BasketDomainException()
    { }

    public BasketDomainException(string message)
        : base(message)
    { }

    public BasketDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}


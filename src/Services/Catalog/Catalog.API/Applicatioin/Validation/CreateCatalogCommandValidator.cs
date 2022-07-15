
namespace eShop.Services.CatalogAPI.Catalog.API.Applicatioin.Validation;

public class CreateCatalogCommandValidator : AbstractValidator<CreateCatalogCommand>
{
    public CreateCatalogCommandValidator(ILogger<CreateCatalogCommandValidator> logger)
    {
        RuleFor(command => command.Name).NotNull().NotEmpty().WithMessage("name is empty");
        RuleFor(command => command.Description).NotEmpty().WithMessage("Description is empty");
        RuleFor(command => command.AvailableStock).GreaterThan(0).WithMessage("AvailableStock GreaterThan 0");

        RuleFor(command => command.MaxStockThreshold).Must((cmd, value) => {
            return cmd.AvailableStock <= value;
        })
            .WithMessage("maxStockThreshold not must be less than availableStock");


        logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
    }
}

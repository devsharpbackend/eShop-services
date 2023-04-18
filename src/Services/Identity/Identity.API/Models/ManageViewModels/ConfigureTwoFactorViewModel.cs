namespace eShop.Services.IdentityAPI.Models.ManageViewModels;


public record ConfigureTwoFactorViewModel
{
    public string SelectedProvider { get; init; }

    public ICollection<SelectListItem> Providers { get; init; }
}

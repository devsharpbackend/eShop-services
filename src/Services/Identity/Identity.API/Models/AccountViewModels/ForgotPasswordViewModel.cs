namespace eShop.Services.IdentityAPI.Models.AccountViewModels;


public record ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }
}

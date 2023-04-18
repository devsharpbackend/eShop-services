
namespace eShop.Services.IdentityAPI.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
 
    public string CardNumber { get; set; }
  
    public string SecurityNumber { get; set; }
  
    [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration should match a valid MM/YY value")]
    public string Expiration { get; set; }
  
    public string CardHolderName { get; set; }
    public int CardType { get; set; }
   
    public string Street { get; set; }
    
    public string City { get; set; }
  
    public string State { get; set; }
 
    public string Country { get; set; }
   
    public string ZipCode { get; set; }
    
    public string Name { get; set; }
 
    public string LastName { get; set; }

}

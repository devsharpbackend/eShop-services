
namespace eShop.Services.IdentityAPI.Data;


public class ApplicationDbContextSeed
{
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

    public async Task MagirateAndSeedAsync(ApplicationDbContext context, IWebHostEnvironment env,
        ILogger<ApplicationDbContextSeed> logger, IOptions<AppSettings> settings, int? retry = 0)
   {
        await context.Database.MigrateAsync();

        try
        {
            var contentRootPath = env.ContentRootPath;
            var webroot = env.WebRootPath;

            // Register Roles
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole { ConcurrencyStamp = Guid.NewGuid().ToString("D"), Name = "admin", NormalizedName = "admin" });
                context.Roles.Add(new IdentityRole { ConcurrencyStamp = Guid.NewGuid().ToString("D"), Name = "registered", NormalizedName = "registered" });
                context.Roles.Add(new IdentityRole { ConcurrencyStamp = Guid.NewGuid().ToString("D"), Name = "guest", NormalizedName = "guest" });
                await context.SaveChangesAsync();
            }
            // Register Users
            if (!context.Users.Any())
            {
                var user = new ApplicationUser()
                    {
                        CardHolderName = "DemoUser",
                        CardNumber = "4012888888881881",
                        CardType = 1,
                        City = "Redmond",
                        Country = "U.S.",
                        Email = "demouser@microsoft.com",
                        Expiration = "12/25",
                        Id = Guid.NewGuid().ToString(),
                        LastName = "DemoLastName",
                        Name = "DemoUser",
                        PhoneNumber = "1234567890",
                        UserName = "demouser@microsoft.com",
                        ZipCode = "98052",
                        State = "WA",
                        Street = "15703 NE 61st Ct",
                        SecurityNumber = "535",
                        NormalizedEmail = "DEMOUSER@MICROSOFT.COM",
                        NormalizedUserName = "DEMOUSER@MICROSOFT.COM",
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                    };
                user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

                // Register Age Cliam For User
                var userClaims = new IdentityUserClaim<string>[] {            
                         new IdentityUserClaim<string> {ClaimType="age",ClaimValue="14",UserId= user.Id }, };

                context.Users.Add(user);

                // Register Age User Roles
                context.UserRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = context.Roles.SingleOrDefault(p => p.Name == "admin").Id });
                context.UserRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = context.Roles.SingleOrDefault(p => p.Name == "registered").Id });
                context.UserClaims.AddRange(userClaims);
                 
                var userGuest = new ApplicationUser()
                   {
                       CardHolderName = "",
                       CardNumber = "",
                       CardType = 1,
                       City = "",
                       Country = "U.S.",
                       PhoneNumber = "",
                       ZipCode = "",
                       State = "",
                       Street = "",
                       SecurityNumber = "",
                       NormalizedEmail = "",
                       NormalizedUserName = "",
                       Email = "guest@guest.com",
                       Expiration = "",
                       Id = Guid.NewGuid().ToString(),
                       LastName = "guest",
                       Name = "guest",
                       UserName = "guest@guest.com",
                       SecurityStamp = Guid.NewGuid().ToString("D"),
                   };

                userGuest.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");
              

                context.Users.Add(userGuest);
                context.UserRoles.Add(new IdentityUserRole<string> { UserId = userGuest.Id, RoleId = context.Roles.SingleOrDefault(p => p.Name == "guest").Id });

                await context.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));
        }
    }



    private IEnumerable<ApplicationUser> GetDefaultUser()
    {

        var user1 =
        new ApplicationUser()
        {
            CardHolderName = "DemoUser",
            CardNumber = "4012888888881881",
            CardType = 1,
            City = "Redmond",
            Country = "U.S.",
            Email = "demouser@microsoft.com",
            Expiration = "12/25",
            Id = Guid.NewGuid().ToString(),
            LastName = "DemoLastName",
            Name = "DemoUser",
            PhoneNumber = "1234567890",
            UserName = "demouser@microsoft.com",
            ZipCode = "98052",
            State = "WA",
            Street = "15703 NE 61st Ct",
            SecurityNumber = "535",
            NormalizedEmail = "DEMOUSER@MICROSOFT.COM",
            NormalizedUserName = "DEMOUSER@MICROSOFT.COM",
            SecurityStamp = Guid.NewGuid().ToString("D"),
        };
        user1.PasswordHash = _passwordHasher.HashPassword(user1, "Pass@word1");

        var userClaimRole = new IdentityUserClaim<string>[] {
           new IdentityUserClaim<string> {ClaimType= JwtClaimTypes.Role,ClaimValue="role",UserId= user1.Id },
           new IdentityUserClaim<string> {ClaimType= JwtClaimTypes.Role,ClaimValue="regsitered",UserId= user1.Id },
           new IdentityUserClaim<string> {ClaimType= JwtClaimTypes.Expiration,ClaimValue=user1.Expiration,UserId= user1.Id },
           new IdentityUserClaim<string> {ClaimType= JwtClaimTypes.Address,ClaimValue=$"Country:{user1.Country} City: {user1.City}",UserId= user1.Id },
        };


        return new List<ApplicationUser>()
        {
            user1
        };
    }



}

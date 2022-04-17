using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PetFinder.Models;

namespace PetFinder.Data
{
    public class AppClaimsPrincipalFactory 
    : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole> {
        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IOptions<IdentityOptions> options
        ) : base(userManager, roleManager, options) { }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user) {
            var principal = await base.CreateAsync(user);
            if (!string.IsNullOrWhiteSpace(user.FirstName)) {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.GivenName, user.FirstName)
                });
            }
            if (!string.IsNullOrWhiteSpace(user.LastName)) {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.Surname, user.LastName)
                });
            }
            if (!string.IsNullOrWhiteSpace(user.City)){
                ((ClaimsIdentity)principal.Identity).AddClaims(new [] {
                    new Claim(ClaimTypes.Locality, user.City)
                });
            }
            if (!Enum.IsDefined(typeof(States), user.State)) {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.StateOrProvince, Enum.GetName(typeof(States), ((int)user.State)))
                });
            }
            return principal;
        }
    }
}
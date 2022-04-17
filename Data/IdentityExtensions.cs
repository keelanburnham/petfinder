using System.Security.Claims;
using System.Security.Principal;

namespace PetFinder.Data {
    public static class IdentityExtensions {
        public static string FirstName(this IIdentity identity) {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.GivenName);
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string LastName(this IIdentity identity) {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Surname);
            return (claim != null) ? claim.Value : string.Empty;   
        }

        public static string City(this IIdentity identity) {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Locality);
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string State(this IIdentity identity) {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.StateOrProvince);
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}
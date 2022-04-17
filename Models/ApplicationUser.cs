using Microsoft.AspNetCore.Identity;

namespace PetFinder.Models {
    public class ApplicationUser : IdentityUser {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        public States State { get; set; }
    }
}
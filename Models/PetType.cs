using System.ComponentModel.DataAnnotations;

namespace PetFinder.Models
{
    public class PetType
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Pet Type")]
        public string Name { get; set; }
    }
}

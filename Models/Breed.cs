using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetFinder.Models
{
    public class Breed
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PetType")]
        public int PetTypeId { get; set; }

        public string Name { get; set; }

        public virtual PetType PetType { get; set; }
    }
}
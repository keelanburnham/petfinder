using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace PetFinder.Models
{
    public class Pet
    {
        public Pet()
        {
            IsAdopted = false;
            IsDeleted = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Adoption Status")]
        public bool IsAdopted { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public string Disability { get; set; }

        [Required]
        [Display(Name = "Pet Type")]
        [ForeignKey("PetType")]
        public int PetTypeId { get; set; }

        [Required]
        [ForeignKey("Breed")]
        public int BreedId { get; set; }

        public virtual PetType PetType { get; set; }
        public virtual Breed Breed { get; set; }

        public string UserId{ get; set;}

        [Display(Name = "Owner")]
        public virtual ApplicationUser User { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public IFormFile ImageFile { get; set; }
    }
}

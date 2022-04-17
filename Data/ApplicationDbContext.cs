using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetFinder.Models;

namespace PetFinder.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pet { get; set; }
        public DbSet<Breed> Breed { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        // public DbSet<ApplicationUser> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PetType>().HasData(
                new PetType { Id = 1, Name = "Cat" },
                new PetType { Id = 2, Name = "Dog" }
            );
            builder.Entity<Breed>().HasData(
                new Breed { Id = 1, Name = "Abyssian", PetTypeId = 1},
                new Breed { Id = 2, Name = "Maine Coon", PetTypeId = 1},
                new Breed { Id = 3, Name = "Ragdoll", PetTypeId = 1},
                new Breed { Id = 4, Name = "American Bulldog", PetTypeId = 2},
                new Breed { Id = 5, Name = "Chihuahua", PetTypeId = 2},
                new Breed { Id = 6, Name = "German Sheperd", PetTypeId = 2}
            );
        }
    }
}

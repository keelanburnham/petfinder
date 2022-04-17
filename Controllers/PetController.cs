using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PetFinder.Data;
using PetFinder.Models;

namespace PetFinder.Controllers
{
    public class PetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostEnvironment _hostEnvironment;

        public PetController(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            IHostEnvironment hostEnvironment
        ) {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Pet
        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null) ViewData["currentUserId"] = user.Id.ToString();
            var pets = _context.Pet
                .Include(p => p.Breed)
                .Include(p => p.PetType)
                .Include(p => p.User)
                .Where(p => p.IsDeleted == false && p.IsAdopted == false);
            return View(await pets.ToListAsync());
        }

        // GET: Pet/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var pet = await _context.Pet
                .Include(p => p.Breed)
                .Include(p => p.PetType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id
            );
            if (pet == null) return NotFound();
            return View(pet);
        }

        // GET: Pet/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["BreedId"] = new SelectList(_context.Breed, "Id", "Name");
            ViewData["PetTypeId"] = new SelectList(_context.PetTypes, "Id", "Name");
            return View();
        }

        // POST: Pet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,IsAdopted,IsDeleted,Disability,PetTypeId,BreedId,ImageFile")] Pet pet,
            string exampleTextbox
        ) {
            if (ModelState.IsValid)
            {
                var wwwRootPath = _hostEnvironment.ContentRootPath + "/wwwroot";
                string fileName = Path.GetFileNameWithoutExtension(pet.ImageFile.FileName);
                string extension = Path.GetExtension(pet.ImageFile.FileName);
                pet.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create)) {
                    await pet.ImageFile.CopyToAsync(fileStream);
                }
                var user = await _userManager.GetUserAsync(HttpContext.User);
                pet.UserId = user.Id;
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreedId"] = new SelectList(
                _context.Breed, 
                "Id", 
                "Name", 
                pet.BreedId
            );
            ViewData["PetTypeId"] = new SelectList(
                _context.PetTypes, 
                "Id", 
                "Name", 
                pet.PetTypeId
            );
            return View(pet);
        }

        // GET: Pet/Edit/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var pet = await _context.Pet.FindAsync(id);
            if (pet == null) return NotFound();
            if (user.Id != pet.UserId) return Unauthorized();
            ViewData["BreedId"] = new SelectList(
                _context.Breed, 
                "Id", 
                "Name", 
                pet.BreedId
            );
            ViewData["PetTypeId"] = new SelectList(
                _context.PetTypes, 
                "Id", 
                "Name", 
                pet.PetTypeId
            );
            return View(pet);
        }

        // POST: Pet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(
            int id, 
            [Bind("Id,Name,IsAdopted,IsDeleted,Disability,PetTypeId,BreedId,UserId,ImageFile")] Pet pet
        ) {
            if (id != pet.Id) return NotFound();
            if (ModelState.IsValid) {
                try {
                    var wwwRootPath = _hostEnvironment.ContentRootPath + "/wwwroot";
                    string fileName = Path.GetFileNameWithoutExtension(pet.ImageFile.FileName);
                    string extension = Path.GetExtension(pet.ImageFile.FileName);
                    pet.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create)) {
                        await pet.ImageFile.CopyToAsync(fileStream);
                    }
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!PetExists(pet.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreedId"] = new SelectList(
                _context.Breed, 
                "Id", 
                "Name", 
                pet.BreedId
            );
            ViewData["PetTypeId"] = new SelectList(
                _context.PetTypes, 
                "Id", 
                "Name", 
                pet.PetTypeId
            );
            return View(pet);
        }

        // GET: Pet/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();
            var pet = await _context.Pet
                .Include(p => p.Breed)
                .Include(p => p.PetType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id
            );
            if (pet == null) return NotFound();
            return View(pet);
        }

        // POST: Pet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var pet = await _context.Pet.FindAsync(id);
            var image = Path.Combine(
                _hostEnvironment.ContentRootPath + "/wwwroot",
                "images",
                pet.ImageName
            );
            if (System.IO.File.Exists(image)) {
                System.IO.File.Delete(image);
                pet.ImageFile = null;
                pet.ImageName = null;
            }
            pet.IsDeleted = true;
            _context.Pet.Update(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id) {
            return _context.Pet.Any(e => e.Id == id);
        }
    }
}

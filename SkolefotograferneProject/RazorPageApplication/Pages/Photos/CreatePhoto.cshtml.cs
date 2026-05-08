using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.Photos
{
    public class CreatePhotoModel : PageModel
    {
        private IRepositoryAsync<Photo> _repo;
        private IRepositoryAsync<Photographer> _photographerRepo;
        private IRepositoryAsync<Student> _studentRepo;

        [BindProperty]
        public Photo NewPhoto { get; set; }

        [BindProperty]
        [Required]
        public int PhotographerId { get; set; }

        [BindProperty]
        [Required]
        public int StudentId { get; set; }

        public CreatePhotoModel(IRepositoryAsync<Photo> photoRepository, IRepositoryAsync<Photographer> photographerRepository, IRepositoryAsync<Student> studentRepository)
        {
            _repo = photoRepository;
            _photographerRepo = photographerRepository;
            _studentRepo = studentRepository;

        }
        public void OnGet()
        {
            NewPhoto = new Photo();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid || PhotographerId < 1 || StudentId < 1)
            {
                return Page();
            }
            NewPhoto.Photographer = await _photographerRepo.GetAsync(PhotographerId);
            NewPhoto.Student = await _studentRepo.GetAsync(StudentId);
            try
            {
                await _repo.CreateAsync(NewPhoto);
                return RedirectToPage("Index");
            }

            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();

            }

        }
    }
}

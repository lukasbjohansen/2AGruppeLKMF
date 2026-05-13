using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public string PhotographerId { get; set; }

        [BindProperty]
        [Required]
        public int StudentId { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> PhotographerSelect { get; set; }

        public CreatePhotoModel(IRepositoryAsync<Photo> photoRepository, IRepositoryAsync<Photographer> photographerRepository, IRepositoryAsync<Student> studentRepository)
        {
            _repo = photoRepository;
            _photographerRepo = photographerRepository;
            _studentRepo = studentRepository;

        }
        public async Task OnGet()
        {
            NewPhoto = new Photo();

            List<Photographer> photographers = await _photographerRepo.GetAllAsync();
            PhotographerSelect = photographers.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Name} - {p.Mail}" });
        }

        public async Task<IActionResult> OnPost()
        {
            //if (!ModelState.IsValid || PhotographerId < 1 || StudentId < 1)
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                NewPhoto.Photographer = await _photographerRepo.GetAsync(Convert.ToInt32(PhotographerId));
                NewPhoto.Student = await _studentRepo.GetAsync(StudentId);
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

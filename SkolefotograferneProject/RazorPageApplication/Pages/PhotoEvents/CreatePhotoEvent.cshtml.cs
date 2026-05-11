using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.PhotoEvents
{
    public class CreatePhotoEventModel : PageModel
    {
        private readonly IRepositoryAsync<PhotoEvent> _repo;
        private readonly IRepositoryAsync<Photographer> _photographerRepo;
        private readonly IRepositoryAsync<SchoolClass> _schoolClassRepo;

        [BindProperty]
        public PhotoEvent NewPhotoEvent { get; set; }

        [BindProperty]
        [Required]
        public int PhotographerId { get; set; }

        [BindProperty]
        [Required]
        public int SchoolClassId { get; set; }

        public CreatePhotoEventModel(IRepositoryAsync<PhotoEvent> repo,
                                     IRepositoryAsync<Photographer> photographerRepo,
                                     IRepositoryAsync<SchoolClass> schoolClassRepo)
        {
            _repo = repo;
            _photographerRepo = photographerRepo;
            _schoolClassRepo = schoolClassRepo;
        }

        public void OnGet()
        {
            NewPhotoEvent = new PhotoEvent();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            NewPhotoEvent.Photographer = await _photographerRepo.GetAsync(PhotographerId);
            NewPhotoEvent.SchoolClass = await _schoolClassRepo.GetAsync(SchoolClassId);
            try
            {
                await _repo.CreateAsync(NewPhotoEvent);
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

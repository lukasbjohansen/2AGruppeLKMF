using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.PhotoEvents
{
    public class EditPhotoEventModel : PageModel
    {
        private readonly IRepositoryAsync<PhotoEvent> _photoEventRepo;
        private readonly IRepositoryAsync<Photographer> _photographerRepo;
        private readonly IRepositoryAsync<SchoolClass> _schoolClassRepo;

        [BindProperty]
        public PhotoEvent? PhotoEventToUpdate { get; set; }
        [BindProperty]
        [Required]
        public int PhotographerId { get; set; }
        [BindProperty]
        public int SchoolClassId { get; set; }

        public EditPhotoEventModel(IRepositoryAsync<PhotoEvent> photoEventRepo,
                                   IRepositoryAsync<Photographer> photographerRepo,
                                   IRepositoryAsync<SchoolClass> schoolClassRepo)
        {
            _photoEventRepo = photoEventRepo;
            _photographerRepo = photographerRepo;
            _schoolClassRepo = schoolClassRepo;

        }

        public async Task OnGet(int id)
        {
            PhotoEventToUpdate = await _photoEventRepo.GetAsync(id);
            PhotographerId = PhotoEventToUpdate.Photographer.Id;
            SchoolClassId = PhotoEventToUpdate.SchoolClass.Id;
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                PhotoEventToUpdate.Photographer = await _photographerRepo.GetAsync(PhotographerId);
                PhotoEventToUpdate.SchoolClass = await _schoolClassRepo.GetAsync(SchoolClassId);
                await _photoEventRepo.UpdateAsync(PhotoEventToUpdate);
                return RedirectToPage("Index");
            }
            catch (SqlException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.PhotoEvents
{
    public class EditPhotoEventModel : PageModel
    {
        #region Instance Fields
        private readonly IRepositoryAsync<PhotoEvent> _photoEventRepo;
        private readonly IRepositoryAsync<Photographer> _photographerRepo;
        private readonly IRepositoryAsync<SchoolClass> _schoolClassRepo;
        #endregion

        #region Properties
        [BindProperty]
        public PhotoEvent? PhotoEventToUpdate { get; set; }
        [BindProperty]
        [Required]
        public int PhotographerId { get; set; }
        [BindProperty]
        public int SchoolClassId { get; set; }

        [BindProperty]
        [Required]
        public IEnumerable<SelectListItem> PhotographerSelect { get; set; }

        [BindProperty]
        [Required]
        public IEnumerable<SelectListItem> SchoolClassSelect { get; set; }
        #endregion

        #region Constructors
        public EditPhotoEventModel(IRepositoryAsync<PhotoEvent> photoEventRepo,
                                   IRepositoryAsync<Photographer> photographerRepo,
                                   IRepositoryAsync<SchoolClass> schoolClassRepo)
        {
            _photoEventRepo = photoEventRepo;
            _photographerRepo = photographerRepo;
            _schoolClassRepo = schoolClassRepo;
        }
        #endregion

        #region Methods
        public async Task OnGet(int id)
        {
            PhotoEventToUpdate = await _photoEventRepo.GetAsync(id);
            PhotographerId = PhotoEventToUpdate.Photographer.Id;
            SchoolClassId = PhotoEventToUpdate.SchoolClass.Id;
            await UpdateSelectList();
        }

        private async Task UpdateSelectList()
        {

            List<Photographer> photographers = await _photographerRepo.GetAllAsync();
            List<SchoolClass> schoolClasses = await _schoolClassRepo.GetAllAsync();
            PhotographerSelect = photographers.Select(p => new SelectListItem
            {
                Value = Convert.ToString(p.Id),
                Text = $"{p.Id} - {p.Name} - {p.Mail}"
            });
            SchoolClassSelect = schoolClasses.Select(p => new SelectListItem
            {
                Value = Convert.ToString(p.Id),
                Text = $"{p.Id} - {p.Name} - {p.School.Name}"
            });
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                await UpdateSelectList();
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
                await UpdateSelectList();
                return Page();
            }
        }
        #endregion
    }
}

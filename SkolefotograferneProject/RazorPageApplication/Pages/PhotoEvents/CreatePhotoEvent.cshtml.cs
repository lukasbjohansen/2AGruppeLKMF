using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.PhotoEvents
{
    public class CreatePhotoEventModel : PageModel
    {
        #region Instance Fields
        private readonly IRepositoryAsync<PhotoEvent> _repo;
        private readonly IRepositoryAsync<Photographer> _photographerRepo;
        private readonly IRepositoryAsync<SchoolClass> _schoolClassRepo;
        #endregion

        #region Properties
        [BindProperty]
        public PhotoEvent NewPhotoEvent { get; set; }

        [BindProperty]
        [Required]
        public int PhotographerId { get; set; }

        [BindProperty]
        [Required]
        public int SchoolClassId { get; set; }

        [BindProperty]
        [Required]
        public IEnumerable<SelectListItem> PhotographerSelect { get; set; }

        [BindProperty]
        [Required]
        public IEnumerable<SelectListItem> SchoolClassSelect { get; set; }
        #endregion

        #region Constructors
        public CreatePhotoEventModel(IRepositoryAsync<PhotoEvent> repo,
                                     IRepositoryAsync<Photographer> photographerRepo,
                                     IRepositoryAsync<SchoolClass> schoolClassRepo)
        {
            _repo = repo;
            _photographerRepo = photographerRepo;
            _schoolClassRepo = schoolClassRepo;
        }
        #endregion

        #region Methods
        public async Task OnGet()
        {
            NewPhotoEvent = new PhotoEvent();
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

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid || PhotographerId < 1 || SchoolClassId < 1)
            {
                await UpdateSelectList();
                return Page();
            }
            try
            {
                NewPhotoEvent.Photographer = await _photographerRepo.GetAsync(PhotographerId);
                NewPhotoEvent.SchoolClass = await _schoolClassRepo.GetAsync(SchoolClassId);
                //NewPhotoEvent.Photographer.Id = PhotographerId;
                //NewPhotoEvent.SchoolClass.Id = SchoolClassId;
                await _repo.CreateAsync(NewPhotoEvent);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                await UpdateSelectList();
                return Page();
            }
        }
        #endregion
    }
}

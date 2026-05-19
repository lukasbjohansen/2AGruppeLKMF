using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPageApplication.Pages.Photos
{
    public class EditPhotoModel : PageModel
    {
        private IRepositoryAsync<Photo> _repo;
        private IRepositoryAsync<Photographer> _photographerRepo;
        private IRepositoryAsync<Student> _studentRepo;

        [BindProperty]
        public Photo PhotoToUpdate { get; set; }

        [BindProperty]
        [Required]
        public string PhotographerId { get; set; }

        [BindProperty]
        [Required]
        public string StudentId { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> PhotographerSelect { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> StudentSelect { get; set; }

        public EditPhotoModel(IRepositoryAsync<Photo> repo, IRepositoryAsync<Photographer> photographerRepository, IRepositoryAsync<Student> studentRepository)
        {
            _repo = repo;
            _photographerRepo = photographerRepository;
            _studentRepo = studentRepository;
        }

        public async Task OnGet(int id)
        {
            PhotoToUpdate = await _repo.GetAsync(id);
            PhotographerId = Convert.ToString(PhotoToUpdate.Photographer.Id);
            StudentId = Convert.ToString(PhotoToUpdate.Student.Id);

            List<Photographer> photographers = await _photographerRepo.GetAllAsync();
            PhotographerSelect = photographers.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Name} - {p.Username}" });
            List<Student> students = await _studentRepo.GetAllAsync();
            StudentSelect = students.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Name} - {p.SchoolClass.Name} - {p.SchoolClass.Year}" });
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                PhotoToUpdate.Photographer = await _photographerRepo.GetAsync(Convert.ToInt32(PhotographerId));
                PhotoToUpdate.Student = await _studentRepo.GetAsync(Convert.ToInt32(StudentId));
                await _repo.UpdateAsync(PhotoToUpdate);
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

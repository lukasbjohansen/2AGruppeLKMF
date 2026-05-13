using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public int PhotographerId { get; set; }

        [BindProperty]
        [Required]
        public int StudentId { get; set; }

        public EditPhotoModel(IRepositoryAsync<Photo> repo, IRepositoryAsync<Photographer> photographerRepository, IRepositoryAsync<Student> studentRepository)
        {
            _repo = repo;
            _photographerRepo = photographerRepository;
            _studentRepo = studentRepository;
        }

        public async Task OnGet(int id)
        {
            PhotoToUpdate = await _repo.GetAsync(id);
            PhotographerId = PhotoToUpdate.Photographer.Id;
            StudentId = PhotoToUpdate.Student.Id;
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid || PhotographerId < 1 || StudentId < 1)
            {
                return Page();
            }

            try
            {
                PhotoToUpdate.Photographer = await _photographerRepo.GetAsync(PhotographerId);
                PhotoToUpdate.Student = await _studentRepo.GetAsync(StudentId);
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

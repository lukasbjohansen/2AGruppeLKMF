using Microsoft.AspNetCore.Hosting;
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

        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public Photo NewPhoto { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        [BindProperty]
        [Required]
        public string PhotographerId { get; set; }

        [BindProperty]
        [Required]
        public string StudentId { get; set; }

        [BindProperty]
        public DateTime Date { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> PhotographerSelect { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> StudentSelect { get; set; }

        public CreatePhotoModel(IRepositoryAsync<Photo> photoRepository, IRepositoryAsync<Photographer> photographerRepository, IRepositoryAsync<Student> studentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _repo = photoRepository;
            _photographerRepo = photographerRepository;
            _studentRepo = studentRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task OnGet()
        {
            NewPhoto = new Photo();

            List<Photographer> photographers = await _photographerRepo.GetAllAsync();
            PhotographerSelect = photographers.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Name} - {p.Mail}" });
            List<Student> students = await _studentRepo.GetAllAsync();
            StudentSelect = students.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Name} - {p.SchoolClass.Name} - {p.SchoolClass.Year}" });
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await OnGet();
                return Page();
            }

            if (Photo != null)

                if (Photo.FileName != null)
                {
                    //string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/Photos", Photo.FileName);
                    NewPhoto.FilePath = await ProcessUploadedFile();
                    //System.IO.File.Delete(filePath); //Hvis der allerede er et foto, slettes det og erstattes
                }

            try
            {
                NewPhoto.Photographer = await _photographerRepo.GetAsync(Convert.ToInt32(PhotographerId));
                NewPhoto.Student = await _studentRepo.GetAsync(Convert.ToInt32(StudentId));
                await _repo.CreateAsync(NewPhoto);
                return RedirectToPage("Index");

            }

            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                await OnGet();
                return Page();
            }


        }

        private async Task<string> ProcessUploadedFile()
        {
            string uniqueFileName = null;
            if (Photo != null)
            {
                Student student = await _studentRepo.GetAsync(Convert.ToInt32(StudentId));
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/Photos");
                uniqueFileName = student.Name + Date.ToString() + "_" + Photo.FileName; //Genererer et unikt ID til vores billede
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }


}

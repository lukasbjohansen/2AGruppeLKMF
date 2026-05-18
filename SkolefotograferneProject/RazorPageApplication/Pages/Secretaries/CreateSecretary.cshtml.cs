using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using RazorPageApplication.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace RazorPageApplication.Pages.Secretaries
{
    public class CreateSecretaryModel : PageModel
    {
        private IRepositoryAsync<Secretary> _secretaryRepo;
        private IRepositoryAsync<School> _schoolRepo;
        
        [BindProperty]
        public Secretary NewSecretary { get; set; }

        [BindProperty]
        [Required]
        public int SchoolID { get; set; }

        

        [BindProperty]
        public IEnumerable<SelectListItem> SchoolSelect { get; set; }
        public CreateSecretaryModel(IRepositoryAsync<Secretary> secretaryRepo, IRepositoryAsync<School> schoolRepo)
        {
            _secretaryRepo = secretaryRepo;
            _schoolRepo = schoolRepo;
        }
        public async Task OnGet()
        {
           NewSecretary = new Secretary();
            List<School> schools = await _schoolRepo.GetAllAsync();
            SchoolSelect = schools.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Name} - {p.PostalCode}" });

        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await OnGet();
                return Page();
            }
            try
            {
                NewSecretary.School = await _schoolRepo.GetAsync(Convert.ToInt32(SchoolID));
                NewSecretary.School = await _schoolRepo.GetAsync(SchoolID);
                await _secretaryRepo.CreateAsync(NewSecretary);
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

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
        #region Instance fields
        //Instance field til at kunne bruge klassernes repo class
        private IRepositoryAsync<Secretary> _secretaryRepo;
        private IRepositoryAsync<School> _schoolRepo;
        #endregion
        #region Properties
        //Properties til at gemme og sæt værdier ind 
        [BindProperty]
        public Secretary NewSecretary { get; set; }

        [BindProperty]
        [Required]
        public int SchoolID { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> SchoolSelect { get; set; }
        #endregion
        #region Constructor
        //dependency injection er hvor vi får resourcerne fra, så det ikke behøves at skrive over alle bagvedliggende sider 
        public CreateSecretaryModel(IRepositoryAsync<Secretary> secretaryRepo, IRepositoryAsync<School> schoolRepo)
        {
            _secretaryRepo = secretaryRepo;
            _schoolRepo = schoolRepo;
        }
        #endregion
        #region Methods
        //Metoden OnGet() indsætter man nye data til en ny sekretær objekt og kan drop-down en liste af skoler
        public async Task OnGet()
        {
           NewSecretary = new Secretary();
            List<School> schools = await _schoolRepo.GetAllAsync();
            SchoolSelect = schools.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Name} - {p.PostalCode}" });
        }

        //OnPost metoden er den nye sekretær skabt, hvis ikke ugyldige infomationer bliver lavet
        //Hvis der dukker fejl op kommer der en exception besked og så må man prøve igen
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await OnGet();
                return Page();
            }
            try
            {
                //finder en skole objekt reference ud fra SchoolID og assigner til NewSecretary property
                NewSecretary.School = await _schoolRepo.GetAsync(SchoolID);
                //Skaber en ny sekretær ud fra de nye dataer
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
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Secretaries
{
    public class EditSecretaryModel : PageModel
    {
        #region Instance fields
        private readonly IRepositoryAsync<Secretary> _secretaryRepo;
        private readonly IRepositoryAsync<School> _schoolRepo;
        #endregion
        #region Properties
        [BindProperty]
        public Secretary? SecretaryToUpdate { get; set; }

        [BindProperty]
        public int SchoolID { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> SchoolSelect { get; set; }
        #endregion
        #region Constructor
        public EditSecretaryModel(IRepositoryAsync<Secretary> repo, IRepositoryAsync<School>schoolRepo)
        {
            _secretaryRepo = repo;
            _schoolRepo = schoolRepo;
        }
        #endregion
        #region Methods
        //Metoden henter den specifikke sekretær ud fra den id fra parameter
        //Henter alle skoler til at kunne finde dem på drop-down ved foranliggende
        //sekretærens skole id bliver sat inde i SchoolID
        public async Task OnGet(int id)
        {
            List<School> schools = await _schoolRepo.GetAllAsync();
            SchoolSelect = schools.Select(p => new SelectListItem { Value = Convert.ToString(p.Id), Text = $"{p.Name} - {p.PostalCode}" });
            SecretaryToUpdate = await _secretaryRepo.GetAsync(id);
            SchoolID = SecretaryToUpdate.School.Id;
        }

        //Metoden har en IActionResult som udføre opdatering når brugeren klikker på en knap
        //Metoden henter den nye skole ....
        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(SecretaryToUpdate.Id); 
                return Page();
            }
            try
            {
                SecretaryToUpdate.School = await _schoolRepo.GetAsync(Convert.ToInt32(SchoolID));
                SecretaryToUpdate.School = await _schoolRepo.GetAsync(SchoolID);
                await _secretaryRepo.UpdateAsync(SecretaryToUpdate);
                return RedirectToPage("Index");
            }
            catch (SqlException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers.Sorting;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPageApplication.Pages.Secretaries
{
    public class IndexModel : PageModel
    {
        #region Instance field
       
        IRepositoryAsync<Secretary> _repo;
        #endregion
        #region Properties
        //Properties til at gemme og sæt værdier ind 
        public List<Secretary> Secretaries { get; set; }

        
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }
        #endregion
        #region Constructor
        
        public IndexModel(IRepositoryAsync<Secretary> secretaryRepo)
        {
            _repo = secretaryRepo;
        }
        #endregion
        #region Metoder

        //Metoden henter dataerne fra GetAllAsync()
        //Den kan hente enten data for alle sekretær eller sekretær som accepter en krav
        public async Task OnGet()
        {
            var allSecretary = await _repo.GetAllAsync();

            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                switch (FilterBy)
                {
                   
                    case "Mail": //hvis man har valgt Mail til filtrer, så henter den alle secretaries, som accepter kravende for Mail
                        
                        Secretaries = allSecretary
                            .Where(sc => sc.Mail.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase)).ToList();
                        //en loop med if statement og returner en liste (ToList)
                        break;

                    case "Name":
                        Secretaries = allSecretary
                            .Where(sc => sc.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                    case "PhoneNumber":
                        Secretaries = allSecretary
                            .Where(sc => sc.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                    case "School":
                            Secretaries = allSecretary
                            .Where(sc => sc.School.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                    case "All": //hvordan kender den til All?
                        Secretaries = allSecretary
                            .Where(sc =>
                                sc.Mail.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.Name.Contains(FilterCriteria) ||
                                sc.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.School.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    break;
                }
            }
            else
            {
                Secretaries = allSecretary;
            }
            if (!string.IsNullOrEmpty(SortBy))
            {
                SortSecretaries();
            }

        }
        //Metodens formål er at sortere dataene der bliver vist i en bestemt rækkefølge 
        //Hver Property som har data der bliver vist på siden, kan sorteres ved, at sammenligne hinandens værdier
        private void SortSecretaries()
        {
            switch (SortBy)
            {
                case "Id":
                    Secretaries.Sort(new GenericComparer<Secretary, int>(p => p.Id, IsDescending));
                    break;
                case "Mail":
                    Secretaries.Sort(new GenericComparer<Secretary, string>(p => p.Mail, IsDescending));
                    break;
                case "Name":
                    Secretaries.Sort(new GenericComparer<Secretary, string>(p => p.Name, IsDescending));
                    break;
                case "PhoneNumber":
                    Secretaries.Sort(new GenericComparer<Secretary, string>(p => p.PhoneNumber, IsDescending));
                    break;
                case "School":
                    Secretaries.Sort(new GenericComparer<Secretary, string>(p => p.School.Name, IsDescending));
                    break;
            }
        }
        #endregion
    }
}

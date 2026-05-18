using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.Photos
{
    public class IndexModel : PageModel
    {
        private IRepositoryAsync<Photo> _repo;

        public List<Photo> Photos { get; set; }


        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }
        public IndexModel(IRepositoryAsync<Photo> photoRepository)
        {
            _repo = photoRepository;
        }
        public async Task OnGet()
        {
            var allPhotos = await _repo.GetAllAsync();

            if (!string.IsNullOrEmpty(FilterCriteria))
            {
                switch (FilterBy)
                {

                    case "FilePath":
                        Photos = allPhotos
                            .Where(sc => sc.FilePath.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "Mail":
                        Photos = allPhotos
                            .Where(sc => sc.Date.ToString().Contains(FilterCriteria))
                            .ToList();
                        break;

                    case "PhotographerID":
                        Photos = allPhotos
                            .Where(sc => sc != null && sc.Photographer.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "StudentID":
                        Photos = allPhotos
                            .Where(sc => sc != null && sc.Student.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "All":
                        Photos = allPhotos
                            .Where(sc =>
                                sc.FilePath.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.Date.ToString().Contains(FilterCriteria) ||
                                sc.Photographer.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                                sc.Student.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                }

            }
            else
            {
                Photos = allPhotos;
            }
        }
    }
}

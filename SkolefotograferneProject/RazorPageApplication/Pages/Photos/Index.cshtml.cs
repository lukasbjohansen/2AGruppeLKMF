using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Helpers.Sorting;
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

        [BindProperty(SupportsGet = true)]
        public bool IsDescending { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }
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
                            .Where(sc => sc.Photographer.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;

                    case "StudentID":
                        Photos = allPhotos
                            .Where(sc => sc.Student.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
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
            if (!string.IsNullOrEmpty(SortBy))
            {
                SortPhotos();
            }
        }

        private void SortPhotos()
        {
            switch (SortBy)
            {
                case "Id":
                    Photos.Sort(new GenericComparer<Photo, int>(p => p.Id, IsDescending));
                    break;
                case "FilePath":
                    Photos.Sort(new GenericComparer<Photo, string>(p => p.FilePath, IsDescending));
                    break;
                case "PhotoDate":
                    Photos.Sort(new GenericComparer<Photo, DateTime>(p => p.Date, IsDescending));
                    break;
                case "PhotographerName":
                    Photos.Sort(new GenericComparer<Photo, string>(p => p.Photographer.Name, IsDescending));
                    break;
                case "StudentName":
                    Photos.Sort(new GenericComparer<Photo, string>(p => p.Student.Name, IsDescending));
                    break;
            }
        }
    }
}

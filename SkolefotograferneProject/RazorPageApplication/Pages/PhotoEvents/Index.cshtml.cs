using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Pages.PhotoEvents
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryAsync<PhotoEvent> _repo;

        public List<PhotoEvent> PhotoEvents { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        public IndexModel(IRepositoryAsync<PhotoEvent> repo)
        {
            _repo = repo;
        }

        public async Task OnGet()
        {
            List<PhotoEvent> allPhotoEvents = await _repo.GetAllAsync();
            if (string.IsNullOrEmpty(FilterCriteria))
            {
                PhotoEvents = allPhotoEvents;
                return;
            }
            switch (FilterBy)
            {
                case "PhotoEventID":
                    PhotoEvents = allPhotoEvents
                        .Where(photoEvent => photoEvent.Id.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case "StartDate":
                    PhotoEvents = allPhotoEvents
                        .Where(photoEvent => photoEvent.StartTime.ToString().Contains(FilterCriteria))
                        .ToList();
                    break;
                case "EndDate":
                    PhotoEvents = allPhotoEvents
                        .Where(photoEvent => photoEvent.EndTime.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case "PhotographerID":
                    PhotoEvents = allPhotoEvents
                        .Where(photoEvent => photoEvent.Photographer.Id.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case "SchoolClassID":
                    PhotoEvents = allPhotoEvents
                        .Where(photoEvent => photoEvent.SchoolClass.Id.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case "All":
                    PhotoEvents = allPhotoEvents
                        .Where(photoEvent =>
                            photoEvent.Id.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                            photoEvent.StartTime.ToString().ToString().Contains(FilterCriteria) ||
                            photoEvent.EndTime.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                            photoEvent.Photographer.Id.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                            photoEvent.SchoolClass.Id.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
            }
        }
    }
}

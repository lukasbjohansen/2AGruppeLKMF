using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Pages.Users
{
    public class WelcomeModel : PageModel
    {
        private readonly IUserService _userService;

        public string Username { get; set; }
        public IUser CurrentUser { get; set; }

        public WelcomeModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            if (Username == null)
            {
                return RedirectToPage("Users/Login");
            }
            else
            {
                CurrentUser = await _userService.GetUserByUsername(Username);
                return Page();
            }
        }
    }
}

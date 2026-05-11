using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageApplication.Interfaces;

namespace RazorPageApplication.Pages.Users
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public void OnGetLogout()
        {
            HttpContext.Session.Remove("Username");
        }

        public IActionResult OnPost()
        {
            IUser user = _userService.VerifyUser(Username, Password);
            if(user != null)
            {
                HttpContext.Session.SetString("Username", user.Mail);
                return RedirectToPage("/Welcome");
            } else
            {
                Message = "Invalid username or password";
                Username = "";
                Password = "";
                return Page();
            }
        }
    }
}

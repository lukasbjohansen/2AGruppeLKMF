using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileSystemGlobbing;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;

namespace RazorPageApplication.Pages.Parents
{
    public class CreateParentModel : PageModel
    {
        #region Instance fields
        /// <summary>
        /// The repository field for the Parent entity, which will be used to perform asynchronous operations such as creating a new parent record in the database.
        /// </summary>
        private IRepositoryAsync<Parent> _repo; 
        #endregion

        #region Properties
        [BindProperty]
        public Parent NewParent { get; set; } 
        #endregion

        #region Constructors
        public CreateParentModel(IRepositoryAsync<Parent> parentRepository)
        {
            _repo = parentRepository;
        } 
        #endregion
        #region Methods
        public void OnGet()
        {
            // new Parent sørger for, at objektet findes i hukommelsen, så formularen har et sted at 'skrive til'. Det gør at vi undgår, at parent er null.
            // new Parent binder datab fra vores form, når brugeren trykker 'Gem' eller 'Opret'.
            //new Parent() i OnGet sikrer, at der er en tom "beholder" klar til at tage imod den data, som brugeren indtaster i formularen.
            NewParent = new Parent();
        }

        public async Task<IActionResult> OnPost()
        {
            // Vi tjekker, om de data brugeren har skrevet i formularen er valide (om de er required osv.)
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Hvis valideringen var i orden, ryger vi ned i vores try-del, hvor vores injected repository (_repo) bliver kaldt asynkront for at oprette et nyt objekt (NewParent) i databasen.
            // Hvis det lykkes, sendes brugeren videre til Index-siden
            try
            {
                await _repo.CreateAsync(NewParent);
                return RedirectToPage("Index");
            }
            // Vi ryger ned i catch-delen, hvis noget går galt i vores try.
            // Fejlbeskeden gemmes i ViewData["ErrorMessage"]
            // Fejlen tilføjes til ModelState, hvilket gør, at den kan vises på vores foranliggende side.
            // Til sidst genindlæses siden, så brugeren får besked om, hvad der gik galt.
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

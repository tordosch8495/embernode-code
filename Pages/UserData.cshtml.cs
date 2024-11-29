using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmbernodeApp.Models;
using EmbernodeApp.Services;

namespace EmbernodeApp.Pages
{
    public class UserDataModel : PageModel
    {
        private readonly CosmosDbService _cosmosDbService;

        public UserDataModel(CosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [BindProperty]
        public UserData UserData { get; set; } = new UserData();

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"OnPostAsync: Name={UserData.Name}, Email={UserData.Email}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState ist ungültig.");
                return Page();
            }

            try
            {
                await _cosmosDbService.AddUserDataAsync(UserData);
                Console.WriteLine("Benutzerdaten erfolgreich gespeichert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler in OnPostAsync: {ex.Message}");
            }

            return RedirectToPage("/Index");
        }
    }
}

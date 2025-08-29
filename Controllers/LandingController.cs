using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SaaSDemo.Controllers
{
    public class LandingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LandingController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /landing?token=xxxx
        public async Task<IActionResult> Index(string token)
        {
            if (string.IsNullOrEmpty(token))
                return Content("Aucun token reçu");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:3978/landing.html"); // URL de l’émulateur

            // Appel de /resolve
            var requestBody = new
            {
                token = token
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("subscriptions/resolve?api-version=2018-08-31", content);

            if (!response.IsSuccessStatusCode)
                return Content($"Erreur lors du resolve : {response}");

            var json = await response.Content.ReadAsStringAsync();

            // Passe les données à la vue
            ViewBag.SubscriptionData = json;
            ViewBag.Token = token;

            return View();
        }

        // GET: /landing/activate?subscriptionId=xxxx
        public async Task<IActionResult> Activate(string subscriptionId)
        {
            if (string.IsNullOrEmpty(subscriptionId))
                return Content("Pas d'ID de souscription fourni");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:3978/landing.html");

            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"subscriptions/{subscriptionId}/activate?api-version=2018-08-31", content);

            if (!response.IsSuccessStatusCode)
                return Content($"Erreur lors de l'activation : {response.StatusCode}");

            return Content($"Abonnement {subscriptionId} activé avec succès !");
        }
    }
}

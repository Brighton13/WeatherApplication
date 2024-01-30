using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherApplication.Models;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetWeather(string city)
    {
        await GetWeatherAsync(city);
        return View("Index"); // Redirect back to the Index view
    }

    private async Task GetWeatherAsync(string city)
    {
        string apiKey = "5fd225a6f82cf0ab8f16745d8c8bf33f";

        using var httpClient = new HttpClient();

        try
        {
            var response = await httpClient.GetFromJsonAsync<WeatherResponse>(
                $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}");

            if (response != null)
            {
                ViewData["City"] = response.Name;
                ViewData["Temperature"] = response.Main.Temp;
                ViewData["Weather"] = response.Weather[0].Description;
            }
            else
            {
                ViewData["ErrorMessage"] = "Unable to fetch weather information.";
            }
        }
        catch (HttpRequestException e)
        {
            ViewData["ErrorMessage"] = $"Error: {e.Message}";
        }
    }
}

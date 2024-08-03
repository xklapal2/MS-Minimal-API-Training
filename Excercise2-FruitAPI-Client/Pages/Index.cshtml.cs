using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Excercise2_FruitAPI_Client.Models;
using System.Text.Json;

namespace Excercise2_FruitAPI_Client.Pages;

public class IndexModel : PageModel
{
    // IHttpClientFactory set using dependency injection 
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Adds the data model and binds the form data to the model properties
    // Enumerable since an array is expected as a response
    [BindProperty]
    public IEnumerable<FruitModel> FruitModels { get; set; }

    // OnGet() is async since HTTP operations should be performed async
    public async Task OnGet()
    {
        // Create the HTTP client using the FruitAPI named factory
        var httpClient = _httpClientFactory.CreateClient("FruitAPI");

        // Execute the GET operation and store the response, the empty parameter
        // in GetAsync doesn't modify the base address set in the client factory 
        using HttpResponseMessage response = await httpClient.GetAsync("");

        // If the operation is successful deserialize the results into the data model
        if (response.IsSuccessStatusCode)
        {
            using var contentStream = await response.Content.ReadAsStreamAsync();
            FruitModels = await JsonSerializer.DeserializeAsync<IEnumerable<FruitModel>>(contentStream);
        }
    }
}
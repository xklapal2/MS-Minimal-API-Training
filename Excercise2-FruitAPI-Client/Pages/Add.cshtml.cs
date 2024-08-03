using Excercise2_FruitAPI_Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Diagnostics;


namespace FruitWebApp.Pages;

public class AddModel : PageModel
{
    // IHttpClientFactory set using dependency injection
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AddModel> _logger;

    public AddModel(IHttpClientFactory httpClientFactory, ILogger<AddModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = _logger;
    }

    // Add the data model and bind the form data to the page model properties
    [BindProperty]
    public FruitModel FruitModels { get; set; }

    public async Task<IActionResult> OnPost()
    {
        // Serialize the information to be added to the database
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(FruitModels),
            Encoding.UTF8,
            "application/json"
        );

        // Create the HTTP client using the FruitAPI named factory
        var httpClient = _httpClientFactory.CreateClient("FruitAPI");

        // Execute the POST request and store the response. The parameters in PostAsync 
        // direct the POST to use the base address and passes the serialized data to the API
        using HttpResponseMessage response = await httpClient.PostAsync("/fruitlist", jsonContent);

        // Return to the home (Index) page and add a temporary success/failure 
        // message to the page.
        if (response.IsSuccessStatusCode)
        {
            TempData["success"] = "Data was added successfully.";
            return RedirectToPage("Index");
        }
        else
        {
            TempData["failure"] = "Operation was not successful";
            return RedirectToPage("Index");
        }
    }

}


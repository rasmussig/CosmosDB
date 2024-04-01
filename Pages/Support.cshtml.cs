using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using CosmosDB.Models;
using Microsoft.Azure.Cosmos.Fluent; // CosmosClientBuilder
using Microsoft.Extensions.Logging; // Til fejlsøgning


namespace CosmosDB.Pages
{
    public class SupportModel : PageModel
    {
        [BindProperty]
        public Issue Issue { get; set; }
        [BindProperty]
        public CustomerInformation CustomerInformation { get; set; }
        [BindProperty]
        public InquiryInformation InquiryInformation { get; set; }
        private readonly string _connectionString;
        private CosmosClient _cosmosClient;
        private readonly ILogger<SupportModel> _logger; // Fejlsøgning

        public SupportModel(IConfiguration configuration, ILogger<SupportModel> logger)
        {
            _connectionString = configuration["CosmosDb:ConnectionString"];
            _logger = logger; // Fejlsøgning


            _cosmosClient = new CosmosClientBuilder(_connectionString)
                .WithSerializerOptions(new CosmosSerializationOptions
                {
                    // Konfigurer JSON-serialisering efter behov
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                })
                .Build();
        }

        //Fejl her omkring at ID og Category er required, POST virker dog.
        public async Task<IActionResult> OnPostAsync()
        {

            // Tjek om modellen er i en gyldig tilstand
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Modelstate is not valid");
                return Page(); // Returnerer til samme side med valideringsfejl
            }

            //Opret forbindelse til Databasen
            var container = _cosmosClient.GetContainer("IbasSupportDB", "ibassupport");

            //Indsætter item i Cosmos DB
            await container.UpsertItemAsync(Issue);

            //Redirecter til index 
            return RedirectToPage("/Index");
        }

    }

}


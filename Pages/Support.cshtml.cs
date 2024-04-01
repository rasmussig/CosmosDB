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

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError(error.ErrorMessage);
                    }
                }
            }

            //Opret forbindelse til Databasen
            var container = _cosmosClient.GetContainer("IbasSupportDB", "ibassupport");

            //Sætter et nyt unikt id
            Issue.Id = Guid.NewGuid().ToString(); 

            //Bruger den korrekte property som partition key
            var partitionKey = new PartitionKey(Issue.category); 

            //Indsætter item i Cosmos DB
            await container.UpsertItemAsync(Issue, partitionKey); 

            //Redirecter til index 
            return RedirectToPage("/Index"); 
        }

    }

}


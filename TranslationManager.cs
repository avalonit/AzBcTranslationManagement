using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace com.businesscentral
{
    public static class TranslationManager
    {
        [FunctionName("TranslationManager")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
           HttpRequest req,
           ILogger log,
           ExecutionContext context)
        {
            // Load configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Sentence and destinatio language
            string sentence = req.Query["sentence"];
            string language = req.Query["language"];

            // Business Central is queried
            var bcConfig = new ConnectorConfig(config);
            BusinessCentralConnector centraConnector = new BusinessCentralConnector(bcConfig);
            var translation = await centraConnector.GetTranslation(sentence, language);

            // Reply with the translation
            if (translation == null || translation.Value == null || translation.Value.Count == 0)
                return new BadRequestObjectResult("Translation not found");
            string translatedText = translation.Value[0].TranslatedText;
            return new OkObjectResult(translatedText);
        }

    }
}

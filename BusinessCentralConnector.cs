using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace com.businesscentral
{
    public class BusinessCentralConnector
    {
        ConnectorConfig config;
        public BusinessCentralConnector(ConnectorConfig config)
        {
            this.config = config;
        }
        public async Task<Translations> GetTranslation(string sentence, string language)
        {
            Translations translations = null;

            using (var httpClient = new HttpClient())
            {
                var apiEndPoint = String.Format("https://api.businesscentral.dynamics.com/{0}/{1}/api/ALV/TransMgt/{2}/companies({3})/",
                                    config.apiVersion1, config.tenant, config.apiVersion1, config.companyID);
                var query = String.Format("Translations?$filter=originalText eq '{0}' and language eq '{1}'", sentence, language);
                var baseUrl = apiEndPoint + query;

                String authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(config.authInfo));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);

                var responseMessage = await httpClient.GetAsync(baseUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonContent = await responseMessage.Content.ReadAsStringAsync();
                    translations = JsonConvert.DeserializeObject<Translations>(jsonContent);
                }
            }
            return translations;
        }
    }

}

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using DirectLineClient.Models;
using Newtonsoft.Json;

namespace DirectLineClient
{
    public class DirectLineAPIClient
    {
        private const string host = "https://directline.botframework.com";

        private static string conversationsAPI = $"{host}/api/conversations";

        private readonly string botSecret;
        
        public DirectLineAPIClient(string botSecret)
        {
            this.botSecret = botSecret;
        }

        public async Task<StartConversationResponse> StartConversationAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", botSecret);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(conversationsAPI, null);

                response.EnsureSuccessStatusCode();
                
                return JsonConvert.DeserializeObject<StartConversationResponse>( await response.Content.ReadAsStringAsync());
            }
        }

        public async Task SendMessageAsync(string conversationId, string from, string text)
        {
            string url = $"{host}/api/conversations/{conversationId}/messages";

            var message = new 
            {
                from = from,
                text = text
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", botSecret);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var response = await httpClient.PostAsync(url, content);

                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                   // TODO: response.EnsureSuccessStatusCode();
                }
            }
        }

        public async Task<ConversationMessages> GetMessagesAsync(string conversationId, string watermark)
        {
            string url = $"{host}/api/conversations/{conversationId}/messages?watermark={watermark}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", botSecret);
                var response = await httpClient.GetStringAsync(url);
                
                return JsonConvert.DeserializeObject<ConversationMessages>(response);
            }
        }
    }
}

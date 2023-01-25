using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DaCoblyn.Function
{
    public class LibreConnector
    {
        private string _uri { get; set; }
        private HttpClient _client { get; set; }
        public LibreConnector(HttpClient client, string uri)
        {
            _uri = uri;
            _client = client;
        }

        private async Task<string> GenerateFormContent(string path, Dictionary<string, string> content)
        {
            var data = new FormUrlEncodedContent(content);
            var response = await _client.PostAsync(path, data);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<LibreDetectResponse?> DetectLanguage(string query)
        {
            var path = _uri + "/detect";
            var data = await GenerateFormContent(path, new Dictionary<string, string>
            {
                { "q", query }
            });

            return JsonConvert.DeserializeObject<List<LibreDetectResponse>>(data)![0];
        }

        public async Task<string?> TranslateQuery(string source, string target, string query)
        {
            var path = _uri + "/translate";
            var data = await GenerateFormContent(path, new Dictionary<string, string>
            {
                { "q", query },
                { "source", source },
                { "target", target },
                { "format", "text" }
            });

            return JsonConvert.DeserializeObject<JToken>(data)!["translatedText"]!.ToString();
        }

        public async Task<List<LibreLanguageResponse>?> GetLanguageSupported()
        {
            var path = _uri + "/languages";
            var response = await _client.GetAsync(path);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<LibreLanguageResponse>>(data);
        }
    }

    public class LibreLanguageResponse
    {
        [JsonProperty("code")] public string Code { get; set; } = null!;
        [JsonProperty("name")] public string Name { get; set; } = null!;
    }

    public class LibreDetectResponse
    {
        [JsonProperty("confidence")] public int Confidence { get; set; }

        [JsonProperty("language")] public string Language { get; set; } = null!;
    }
}
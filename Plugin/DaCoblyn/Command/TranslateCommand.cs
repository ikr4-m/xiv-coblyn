using DaCoblyn.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace DaCoblyn.Command
{
    public class TranslateCommand : BaseCommand
    {
        public TranslateCommand(Plugin plugin) : base(plugin)
        {
            Command = "/translate";
            HelpMessage = "Translate some text to some languages.";
        }

        public override async void Execute(string command, string argString)
        {
            //WindowSystem.GetWindow("CoblynWindow")!.IsOpen = true;
            //this.ChatGui.PrintToGame("Hello world!");

            var args = argString.Split(' ').ToList();
            if (args.Count < 2)
            {
                BasePlugin.ChatGui.PrintToGame("Need 2 arguments to do this command! /translate <target> <text>");
                return;
            }

            BasePlugin.ChatGui.PrintToGame("Translating...");
            try
            {
                var targetLang = args[0];
                var sourceLang = "auto";
                var text = string.Join(' ', args.Skip(1).ToArray());
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "target", targetLang },
                    { "source", sourceLang },
                    { "q", text },
                    { "format", "text" }
                });

                var response = await BasePlugin.Configuration.HttpClient.PostAsync(BasePlugin.Configuration.TranslateURI, content);
                var responseString = await response.Content.ReadAsStringAsync();

                var decodeJson = JsonConvert.DeserializeObject<JToken>(responseString);
                var translated = decodeJson!["translatedText"]!.ToString();
                BasePlugin.ChatGui.PrintToGame(translated);
            }
            catch (HttpRequestException e)
            {
                BasePlugin.ChatGui.PrintToGame(e.Message);
            }
        }
    }
}
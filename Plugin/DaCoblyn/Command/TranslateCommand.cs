using DaCoblyn.Extension;
using DaCoblyn.Function;
using System;
using System.Linq;
using System.Net.Http;

namespace DaCoblyn.Command
{
    public class TranslateCommand : BaseCommand
    {
        private HttpClient _httpClient = new HttpClient();

        public TranslateCommand(Plugin plugin) : base(plugin)
        {
            CommandLiterate = new string[] { "/tl", "/translate" };
            HelpMessage = "Translate some text to some languages.";
        }

        public override async void Execute(string command, string argString)
        {
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

                var data = await new LibreConnector(_httpClient, Global.TranslateURI)
                    .TranslateQuery(sourceLang, targetLang, text);
                
                BasePlugin.ChatGui.PrintToGame(data ?? "No response");
            }
            catch (Exception e)
            {
                BasePlugin.ChatGui.PrintToGame(e.Message);
                BasePlugin.ChatGui.PrintToGame(e.StackTrace ?? "");
            }
        }
    }
}
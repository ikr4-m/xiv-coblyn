using DaCoblyn.Extension;
using DaCoblyn.Windows;
using System.Net.Http;

namespace DaCoblyn.Command
{
    public class TranslateCommand : BaseCommand
    {
        private HttpClient _httpClient = new HttpClient();

        public TranslateCommand(Plugin plugin) : base(plugin)
        {
            CommandLiterate = new string[] { "/tl", "/translate" };
            HelpMessage = "Spawn translate window.";
        }

        public override void Execute(string command, string argString)
        {
            BasePlugin.WindowSystem.OpenWindow(typeof(TranslateWindow));
        }
    }
}
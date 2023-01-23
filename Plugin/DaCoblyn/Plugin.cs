using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Game.Gui;
using DaCoblyn.Windows;
using DaCoblyn.Extension;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DaCoblyn
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Coblyn";
        private const string CommandName = "/translate";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        private ChatGui ChatGui { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("DaCoblyn");

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager,
            [RequiredVersion("1.0")] ChatGui chatGui)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;
            this.ChatGui= chatGui;

            // Register configuration
            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            // Register window
            WindowSystem.AddWindow(new ConfigWindow(this));
            WindowSystem.AddWindow(new MainWindow(this, goatImage));

            // Command handler
            // TODO: Will use some DI to inject command so I don't register command one-by-one
            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Translate your text to some language."
            });

            // Draw window
            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        private async void OnCommand(string command, string argString)
        {
            //WindowSystem.GetWindow("CoblynWindow")!.IsOpen = true;
            //this.ChatGui.PrintToGame("Hello world!");

            var args = argString.Split(' ').ToList();
            if (args.Count < 2)
            {
                this.ChatGui.PrintToGame("Need 2 arguments to do this command");
                return;
            }

            this.ChatGui.PrintToGame("Translating...");
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

                var response = await this.Configuration.HttpClient.PostAsync(this.Configuration.TranslateURI, content);
                var responseString = await response.Content.ReadAsStringAsync();

                var decodeJson = JsonConvert.DeserializeObject<JToken>(responseString);
                var translated = decodeJson!["translatedText"]!.ToString();
                this.ChatGui.PrintToGame(translated);
            }
            catch (HttpRequestException e)
            {
                this.ChatGui.PrintToGame(e.Message);
            }
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            WindowSystem.GetWindow("Coblyn Configuration")!.IsOpen = true;
        }
    }
}

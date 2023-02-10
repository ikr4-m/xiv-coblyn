using Dalamud.Game.Command;
using Dalamud.Game.ClientState;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Game.Gui;
using DaCoblyn.Extension;
using DaCoblyn.Function;
using DaCoblyn.Command;
using DaCoblyn.Events;
using DaCoblyn.Windows;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DaCoblyn
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Coblyn";

        public DalamudPluginInterface PluginInterface { get; init; }
        public CommandManager CommandManager { get; init; }
        public ChatGui ChatGui { get; init; }
        public Configuration Configuration { get; init; }
        public ClientState Client { get; init; }
        public WindowSystem WindowSystem = new("DaCoblyn");
        public RegisterCommand CommandList { get; set; }
        public RegisterEvents EventsList { get; set; }
        public RegisterWindow WindowManager { get; set; }
        public List<LibreLanguageResponse> LanguageSupported { get; set; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager,
            [RequiredVersion("1.0")] ChatGui chatGui,
            [RequiredVersion("1.0")] ClientState client)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;
            this.ChatGui = chatGui;
            this.Client = client;
            
            // Register configuration
            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // Register window
            this.WindowManager = new RegisterWindow(this);
            this.WindowManager.Initialize();

            // Command handler
            this.CommandList = new RegisterCommand(this);
            this.CommandList.Initialize();

            // Register event
            this.EventsList = new RegisterEvents(this);
            this.EventsList.Initialize();

            // Draw window
            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            // Register language supported
            this.LanguageSupported = new List<LibreLanguageResponse>();
            try
            {
                var task = new Task(async () =>
                {
                    var httpClient = new HttpClient();
                    var connector = new LibreConnector(httpClient, Global.TranslateURI);
                    this.LanguageSupported = await connector.GetLanguageSupported() ?? new List<LibreLanguageResponse>()
                    {
                        new LibreLanguageResponse() { Code = "en", Name = "Engilsh" },
                        new LibreLanguageResponse() { Code = "ja", Name = "Japanese" },
                    };
                });
                task.RunSynchronously();
            }
            catch (Exception e)
            {
                this.ChatGui.PrintToGame(e.Message);
            }
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandList.Dispose();
            this.EventsList.Dispose();
        }

        private void DrawUI() => this.WindowSystem.Draw();

        public void DrawConfigUI() => this.WindowSystem.OpenWindow(typeof(ConfigWindow));
    }
}

using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Game.Gui;
using DaCoblyn.Command;
using DaCoblyn.Windows;
using System.IO;

namespace DaCoblyn
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Coblyn";

        public DalamudPluginInterface PluginInterface { get; init; }
        public CommandManager CommandManager { get; init; }
        public ChatGui ChatGui { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("DaCoblyn");
        public RegisterCommand CommandList { get; set; }

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
            this.CommandList = new RegisterCommand(this);
            this.CommandList.Initialize();

            // Draw window
            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            // this.CommandManager.RemoveHandler(CommandName);
            this.CommandList.Dispose();
        }

        private void DrawUI() => this.WindowSystem.Draw();

        public void DrawConfigUI() => WindowSystem.GetWindow("Coblyn Configuration")!.IsOpen = true;
    }
}

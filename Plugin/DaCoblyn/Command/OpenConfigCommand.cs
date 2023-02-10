using DaCoblyn.Extension;
using DaCoblyn.Windows;

namespace DaCoblyn.Command
{
    public class OpenConfigCommand : BaseCommand
    {
        public OpenConfigCommand(Plugin plugin) : base(plugin)
        {
            Command = "/coblynconfig";
            HelpMessage = "Open coblyn config.";
        }

        public override void Execute(string command, string argString)
        {
            BasePlugin.WindowSystem.OpenWindow(typeof(ConfigWindow));
        }
    }
}
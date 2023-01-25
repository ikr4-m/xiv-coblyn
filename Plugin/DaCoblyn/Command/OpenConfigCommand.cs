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
            BasePlugin.WindowSystem.GetWindow("Coblyn Configuration")!.IsOpen = true;
        }
    }
}
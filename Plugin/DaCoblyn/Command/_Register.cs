using System;
using Dalamud.Game.Command;

namespace DaCoblyn.Command
{
    public class RegisterCommand : IDisposable
    {
        private Plugin BasePlugin;

        public RegisterCommand(Plugin plugin) => this.BasePlugin = plugin;

        public void Initialize()
        {
            AddCommand(new TranslateCommand(this.BasePlugin));
            AddCommand(new OpenConfigCommand(this.BasePlugin));
        }

        public void Dispose()
        {
            RemoveCommand(new TranslateCommand(this.BasePlugin));
            RemoveCommand(new OpenConfigCommand(this.BasePlugin));
        }

        private void AddCommand(BaseCommand cmd)
        {
            if (cmd.Command != null)
            {
                this.BasePlugin.CommandManager.AddHandler(cmd.Command, new CommandInfo(cmd.Execute)
                {
                    HelpMessage = cmd.HelpMessage
                });
            }
            else
            {
                foreach (var cmds in cmd.CommandLiterate!)
                {
                    this.BasePlugin.CommandManager.AddHandler(cmds, new CommandInfo(cmd.Execute)
                    {
                        HelpMessage = cmd.HelpMessage
                    });
                }
            }
        }

        private void RemoveCommand(BaseCommand cmd)
        {
            if (cmd.Command != null)
            {
                this.BasePlugin.CommandManager.RemoveHandler(cmd.Command);
            }
            else
            {
                foreach (var cmds in cmd.CommandLiterate!)
                {
                    this.BasePlugin.CommandManager.RemoveHandler(cmds);
                }
            }
        }
    }


    public abstract class BaseCommand
    {
        public string? Command { get; set; }
        public string[]? CommandLiterate { get; set; }
        public string HelpMessage { get; set; } = "";
        public Plugin BasePlugin { get; set; }

        public BaseCommand(Plugin plugin)
        {
            this.BasePlugin = plugin;
        }
        
        public abstract void Execute(string command, string args);
    }
}
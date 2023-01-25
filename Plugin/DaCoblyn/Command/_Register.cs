using System;
using System.Collections.Generic;
using Dalamud.Game.Command;

namespace DaCoblyn.Command
{
    public class RegisterCommand : IDisposable
    {
        private Plugin BasePlugin;
        public Dictionary<string, BaseCommand> CommandList = new Dictionary<string, BaseCommand>();
        public RegisterCommand(Plugin plugin) => this.BasePlugin = plugin;

        public void Initialize()
        {
            AddCommand(new TranslateCommand(this.BasePlugin));
            AddCommand(new OpenConfigCommand(this.BasePlugin));
        }

        public void Dispose()
        {
            foreach (var key in CommandList.Keys)
            {
                this.BasePlugin.CommandManager.RemoveHandler(key);
            }
        }

        private void AddCommand(BaseCommand cmd)
        {
            if (cmd.Command != null)
            {
                CommandList.Add(cmd.Command, cmd);
                this.BasePlugin.CommandManager.AddHandler(cmd.Command, new CommandInfo(cmd.Execute)
                {
                    HelpMessage = cmd.HelpMessage
                });
            }

            if (cmd.CommandLiterate != null)
            {
                foreach (var cmds in cmd.CommandLiterate)
                {
                    CommandList.Add(cmds, cmd);
                    this.BasePlugin.CommandManager.AddHandler(cmds, new CommandInfo(cmd.Execute)
                    {
                        HelpMessage = cmd.HelpMessage
                    });
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
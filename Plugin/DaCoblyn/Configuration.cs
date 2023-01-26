using Dalamud.Configuration;
using Dalamud.Game.Text;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace DaCoblyn
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool EnablePlugin { get; set; } = false;
        public string SourceLanguage { get; set; } = "auto";
        public string TargetLanguage { get; set; } = "ja";
        public List<string> IgnoreLanguage { get; set; } = new List<string>();
        public List<XivChatType> ChannelListened { get; set; } = new List<XivChatType>();

        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}

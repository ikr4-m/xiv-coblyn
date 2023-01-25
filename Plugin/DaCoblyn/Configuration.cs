using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace DaCoblyn
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        // Property Things
        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

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

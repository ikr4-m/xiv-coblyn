using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Net.Http;

namespace DaCoblyn
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        // Property Things
        public string TranslateURI = "https://translate.argosopentech.com/translate";

        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        // Initializor Things
        public readonly HttpClient HttpClient = new HttpClient();

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

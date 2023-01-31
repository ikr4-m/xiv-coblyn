using System;
using Dalamud.Interface.Windowing;

namespace DaCoblyn.Windows
{
    public class RegisterWindow : IDisposable
    {
        private Plugin BasePlugin;
        public RegisterWindow(Plugin plugin) => this.BasePlugin = plugin;

        public void Initialize()
        {
            AddWindow(new ConfigWindow(BasePlugin));
            AddWindow(new TranslateWindow(BasePlugin));
        }

        public void Dispose()
        {
            BasePlugin.WindowSystem.RemoveAllWindows();
        }

        private void AddWindow(Window window)
        {
            BasePlugin.WindowSystem.AddWindow(window);
        }
    }
}
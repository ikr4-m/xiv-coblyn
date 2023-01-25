using System;

namespace DaCoblyn.Events
{
    public class RegisterEvents : IDisposable
    {
        private Plugin BasePlugin;

        public RegisterEvents(Plugin plugin) => this.BasePlugin = plugin;

        public void Initialize()
        {
            //
        }

        public void Dispose()
        {
            //
        }
    }
}
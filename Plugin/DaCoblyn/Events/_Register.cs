using System;
using System.Collections.Generic;

namespace DaCoblyn.Events
{
    public class RegisterEvents : IDisposable
    {
        private Plugin BasePlugin;
        private List<BaseEvents> EventList = new List<BaseEvents>();
        public RegisterEvents(Plugin plugin) => this.BasePlugin = plugin;

        public void Initialize()
        {
            AddEvents(new SpoofingChatEvents(this.BasePlugin));
        }

        public void Dispose()
        {
            foreach (var evt in EventList) evt.Dispose();
        }

        private void AddEvents(BaseEvents evt)
        {
            EventList.Add(evt);
        }
    }

    public abstract class BaseEvents : IDisposable
    {
        public Plugin BasePlugin { get; set; }

        public BaseEvents(Plugin plugin)
        {
            this.BasePlugin = plugin;
        }

        public abstract void Dispose();
    }
}
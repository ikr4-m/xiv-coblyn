using System;
using System.Linq;
using Dalamud.Interface.Windowing;

namespace DaCoblyn.Extension
{
    public static class WindowManagerExtension
    {
        public static void OpenWindow(this WindowSystem windowSystem, Type window)
        {
            var w = windowSystem.Windows.Where(x => x.GetType() == window).FirstOrDefault();
            if (w != null) w.IsOpen = true;
        }
    }
}
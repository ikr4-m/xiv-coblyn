using System;
using Dalamud.Game.Gui;

namespace DaCoblyn.Extension
{
    public static class SendingChatExtension
    {
        public static void PrintToGame(this ChatGui chatGui, string message)
        {
            chatGui.Print($"[Coblyn] {message}");
        }

        public static void PrintException(this ChatGui chatGui, Exception e)
        {
            chatGui.PrintError($"[CoblynException] {e.Message}");
        }
    }
}

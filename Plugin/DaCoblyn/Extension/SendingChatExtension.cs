using Dalamud.Game.Gui;

namespace DaCoblyn.Extension
{
    public static class SendingChatExtension
    {
        public static void PrintToGame(this ChatGui chatGui, string message)
        {
            chatGui.Print($"[Coblyn] {message}");
        }
    }
}

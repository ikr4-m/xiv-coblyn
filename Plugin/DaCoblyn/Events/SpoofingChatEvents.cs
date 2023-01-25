using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using DaCoblyn.Extension;

namespace DaCoblyn.Events
{
    public class SpoofingChatEvents : BaseEvents
    {
        public SpoofingChatEvents(Plugin plugin) : base(plugin)
        {
            BasePlugin.ChatGui.ChatMessage += Execute;
        }

        private void Execute(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (type != XivChatType.Say) return;
            BasePlugin.ChatGui.PrintToGame(message.ToJson());
        }

        public override void Dispose()
        {
            BasePlugin.ChatGui.ChatMessage -= Execute;
        }
    }
}

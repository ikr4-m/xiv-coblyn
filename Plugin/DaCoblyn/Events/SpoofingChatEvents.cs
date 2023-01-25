using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
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

            var senderStr = sender.TextValue;
            var messageStr = "";
            foreach (var msgPayload in message.Payloads)
            {
                if (msgPayload.GetType() == typeof(AutoTranslatePayload))
                    messageStr += (msgPayload as AutoTranslatePayload)!.Text[1..^1];
                else
                    messageStr += (msgPayload as TextPayload)!.Text + " ";
            }

            BasePlugin.ChatGui.PrintToGame($"[{senderStr} {messageStr.Trim()}");
        }

        public override void Dispose()
        {
            BasePlugin.ChatGui.ChatMessage -= Execute;
        }
    }
}

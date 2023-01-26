using System;
using System.Linq;
using System.Net.Http;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using DaCoblyn.Extension;
using DaCoblyn.Function;

namespace DaCoblyn.Events
{
    public class SpoofingChatEvents : BaseEvents
    {
        private LibreConnector Connector = new LibreConnector(new HttpClient(), Global.TranslateURI);

        public SpoofingChatEvents(Plugin plugin) : base(plugin)
        {
            BasePlugin.ChatGui.ChatMessage += Execute;
        }

        private void Execute(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)=> ExecuteAsync(type, senderId, sender, message, isHandled);

        private async void ExecuteAsync(XivChatType type, uint senderId, SeString sender, SeString message, bool isHandled)
        {
            if (!BasePlugin.Configuration.EnablePlugin) return;
            if (BasePlugin.Configuration.ChannelListened.Where(x => x == type).Count() == 0) return;

            try
            {
                var senderStr = sender.TextValue;
                var messageStr = "";
                foreach (var msgPayload in message.Payloads)
                {
                    // Keep auto translate not translate
                    if (msgPayload.GetType() == typeof(AutoTranslatePayload))
                        messageStr += (msgPayload as AutoTranslatePayload)!.Text[1..^1];
                    // Translate each payload
                    else if (msgPayload.GetType() == typeof(TextPayload))
                    {
                        var text = (msgPayload as TextPayload)!.Text;
                        var sourceLang = BasePlugin.Configuration.SourceLanguage;
                        var targetLang = BasePlugin.Configuration.TargetLanguage;

                        // Since we provide support to auto-detect translate and "focus" sourceLang,
                        // if player didn't choose Automatic as source language, the plugin will not
                        // try to communicate to server for detecting language.
                        if (sourceLang == "auto")
                        {
                            var detected = await Connector.DetectLanguage(text ?? "");
                            if (detected == null)
                            {
                                BasePlugin.ChatGui.PrintToGame("The language is not supported from Argos Translate.");
                                return;
                            }
                            if (detected.Confidence < 60f)
                            {
                                BasePlugin.ChatGui.PrintToGame("The confident level too low. Rejected to translate it.");
                                return;
                            }
                            if (targetLang == detected.Language) return;

                            sourceLang = detected.Language;
                        }

                        // Ignore blacklisted language
                        if (BasePlugin.Configuration.IgnoreLanguage.Where(x => x == sourceLang).Count() > 0) return;

                        var translated = await Connector.TranslateQuery(sourceLang, targetLang, text ?? "");
                        if (translated == null) return;
                        messageStr += translated + " ";
                    }
                    else
                        messageStr += msgPayload.ToString() + " ";
                }

                BasePlugin.ChatGui.PrintToGame($"[{type.ToString()}][{senderStr}] {messageStr.Trim()}");
            }
            catch (Exception e)
            {
                BasePlugin.ChatGui.PrintException(e);
            }
        }

        public override void Dispose()
        {
            BasePlugin.ChatGui.ChatMessage -= Execute;
        }
    }
}

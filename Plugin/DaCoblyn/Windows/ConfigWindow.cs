using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics;
using Dalamud.Game.Text;
using Dalamud.Interface.Windowing;
using DaCoblyn.Function;
using ImGuiNET;

namespace DaCoblyn.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;
    private Plugin BasePlugin;

    public ConfigWindow(Plugin plugin) : base(
        "Coblyn Configuration",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.HorizontalScrollbar)
    {
        this.Size = new Vector2(500, 500);
        this.SizeCondition = ImGuiCond.Always;

        this.BasePlugin = plugin;
        this.Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text("Before enable this plugin, make sure all the configuration is perfect for you.");
        if (ImGui.BeginTable("", 2, ImGuiTableFlags.None))
        {
            ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None, 370f);
            ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None, 130f);
            ImGui.TableNextColumn();

            var enablePlugin = Configuration.EnablePlugin;
            if (ImGui.Checkbox("Enable Plugin", ref enablePlugin))
            {
                Configuration.EnablePlugin = enablePlugin;
                Configuration.Save();
            }
            ImGui.TableNextColumn();

            ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0x005E5BFF);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, 0xDD000000 | 0x005E5BFF);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0x005E5BFF);
            if (ImGui.Button("Support me on Ko-fi!"))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://ko-fi.com/thatismunn",
                    UseShellExecute = true
                });
            }
            ImGui.EndTable();
        }

        ImGui.PopStyleColor(3);
        ImGui.Separator();

        if (ImGui.CollapsingHeader("Source & Target Language"))
        {
            var sourceLang = Configuration.SourceLanguage;
            var modSourceLang = new List<LibreLanguageResponse>() { new LibreLanguageResponse() { Code = "auto", Name = "Automatic" } };
            modSourceLang.AddRange(BasePlugin.LanguageSupported);
            var _sourceDisplayLang = modSourceLang.Where(x => x.Code == sourceLang).FirstOrDefault();
            var sourceDisplayLang = _sourceDisplayLang == null ? "Null" : _sourceDisplayLang.Name;
            ImGui.Columns(2, "SourceTargetLang", false);
            ImGui.Text("Source Language");
            ImGui.NextColumn();
            if (ImGui.BeginCombo("##sourceLang", sourceDisplayLang))
            {
                foreach (var lang in modSourceLang)
                {
                    if (ImGui.Selectable(lang.Name, lang.Code == sourceLang) && lang.Code != sourceLang)
                    {
                        Configuration.SourceLanguage = lang.Code;
                        Configuration.Save();
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.NextColumn();

            var targetLang = Configuration.TargetLanguage;
            var _targetDisplayLang = BasePlugin.LanguageSupported.Where(x => x.Code == targetLang).FirstOrDefault();
            var targetDisplayLang = _targetDisplayLang == null ? "Null" : _targetDisplayLang.Name;
            ImGui.Text("Target Language");
            ImGui.NextColumn();
            if (ImGui.BeginCombo("##targetLang", targetDisplayLang))
            {
                foreach (var lang in BasePlugin.LanguageSupported)
                {
                    if (ImGui.Selectable(lang.Name, lang.Code == targetLang) && lang.Code != targetLang)
                    {
                        Configuration.TargetLanguage = lang.Code;
                        Configuration.Save();
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.NextColumn();
            ImGui.Columns(1, "EndSourceTargetLang", false);
        }

        if (ImGui.CollapsingHeader("Ignore Translate"))
        {
            foreach (var lang in BasePlugin.LanguageSupported)
            {
                var ignored = Configuration.IgnoreLanguage;
                var isIgnored = ignored.Where(x => x == lang.Code).Count() > 0;
                if (ImGui.Checkbox(lang.Name, ref isIgnored))
                {
                    if (isIgnored) ignored.Add(lang.Code);
                    else ignored.Remove(lang.Code);
                    Configuration.IgnoreLanguage = ignored;
                    Configuration.Save();
                }
            }
        }

        if (ImGui.CollapsingHeader("Translate the Channel"))
        {
            var channelList = Enum.GetNames(typeof(XivChatType)).ToList();
            var channelListened = Configuration.ChannelListened;
            foreach (var channel in channelList)
            {
                var numType = (XivChatType)Enum.Parse(typeof(XivChatType), channel);
                var isChecked = channelListened.Where(x => x == numType).Count() > 0;
                if (ImGui.Checkbox(channel, ref isChecked))
                {
                    if (isChecked) channelListened.Add(numType);
                    else channelListened.Remove(numType);
                    Configuration.ChannelListened = channelListened;
                    Configuration.Save();
                }
            }
        }

    }
}

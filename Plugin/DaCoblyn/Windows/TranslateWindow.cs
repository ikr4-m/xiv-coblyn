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
public class TranslateWindow : Window, IDisposable
{
    private Configuration Configuration;
    private Plugin BasePlugin;
    public int Count { get; set; } = 0;

    private string _targetLang = "";
    private string _targetInput = "";
    private string _sourceLang = "";
    private string _sourceInput = "";

    public TranslateWindow(Plugin plugin) : base(
        "Translator",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.HorizontalScrollbar)
    {
        Size = new Vector2(700, 300);
        SizeCondition = ImGuiCond.Always;

        BasePlugin = plugin;
        Configuration = plugin.Configuration;
        _targetLang = BasePlugin.Configuration.TargetLanguage;
        _sourceLang = BasePlugin.Configuration.SourceLanguage;
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (ImGui.BeginTable("##windowTranslate", 3, ImGuiTableFlags.None))
        {
            ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None, 340f);
            ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None, 20f);
            ImGui.TableSetupColumn("", ImGuiTableColumnFlags.None, 340f);
            ImGui.TableNextColumn();

            // Header
            var modSourceLang = new List<LibreLanguageResponse>() { new LibreLanguageResponse() { Code = "auto", Name = "Automatic" } };
            modSourceLang.AddRange(BasePlugin.LanguageSupported);
            var sourceDisplay = modSourceLang.Where(x => x.Code == _sourceLang).FirstOrDefault();
            var sourceDisplayLang = sourceDisplay == null ? "Null" : sourceDisplay.Name;
            if (ImGui.BeginCombo("##sourceLangTranslate", sourceDisplayLang))
            {
                foreach (var lang in modSourceLang)
                {
                    if (ImGui.Selectable(lang.Name, lang.Code == _sourceLang) && lang.Code != _sourceLang)
                    {
                        _sourceLang = lang.Code;
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.TableNextColumn();
            ImGui.Button("\u21D4");
            ImGui.TableNextColumn();
            var targetLang = BasePlugin.LanguageSupported.Where(x => x.Code == _targetLang).FirstOrDefault();
            var targetLangDisplay = targetLang == null ? "Null" : targetLang.Name;
            if (ImGui.BeginCombo("##targetLangTranslate", targetLangDisplay))
            {
                foreach (var lang in BasePlugin.LanguageSupported)
                {
                    if (ImGui.Selectable(lang.Name, lang.Code == _targetLang) && lang.Code != _targetLang)
                    {
                        _targetLang = lang.Code;
                    }
                }
                ImGui.EndCombo();
            }
            ImGui.TableNextColumn();

            // Body
            ImGui.InputTextMultiline("##sourceTranslatorInput", ref _sourceInput, 2000, new Vector2(340, 195));
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            ImGui.InputTextMultiline("##targetTranslatorInput", ref _targetInput, 2000, new Vector2(340, 195), ImGuiInputTextFlags.ReadOnly);
            ImGui.TableNextColumn();

            ImGui.EndTable();
        }

        ImGui.Button("Translate");
        ImGui.SameLine();
        if (ImGui.Button("Reset"))
        {
            _sourceInput = "";
            _targetInput = "";
        }
        ImGui.SameLine();
        ImGui.Button("Copy left column to clipboard");
        ImGui.SameLine();
        ImGui.Button("Copy right button to clipboard");
    }
}

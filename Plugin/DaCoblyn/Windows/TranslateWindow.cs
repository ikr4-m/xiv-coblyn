using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dalamud.Interface.Windowing;
using DaCoblyn.Function;
using DaCoblyn.Extension;
using ImGuiNET;
using TextCopy;

namespace DaCoblyn.Windows;
public class TranslateWindow : Window, IDisposable
{
    private Configuration Configuration;
    private Plugin BasePlugin;
    private LibreConnector Connector = new LibreConnector(new HttpClient(), Global.TranslateURI);

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

    public async void ExecuteTranslate()
    {
        try
        {
            _targetInput = "Loading...";
            var translated = await Connector.TranslateQuery(_sourceLang, _targetLang, _sourceInput);
            _targetInput = translated == null ? "Please try again" : translated;
        }
        catch (Exception e)
        {
            _targetInput = e.Message;
            BasePlugin.ChatGui.PrintException(e);
        }
    }

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
            if (_sourceLang != "auto")
            {
                if (ImGui.Button("\u21D4"))
                    (_sourceLang, _targetLang) = (_targetLang, _sourceLang);
            }
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

        if (ImGui.Button("Translate"))
        {
            _ = Task.Run(() => ExecuteTranslate());
        }
        ImGui.SameLine();
        if (ImGui.Button("Reset"))
        {
            _sourceInput = "";
            _targetInput = "";
        }
        ImGui.SameLine();
        if (ImGui.Button("Copy left column to clipboard")) ClipboardService.SetText(_sourceInput);
        ImGui.SameLine();
        if (ImGui.Button("Copy right button to clipboard")) ClipboardService.SetText(_targetInput);
    }
}

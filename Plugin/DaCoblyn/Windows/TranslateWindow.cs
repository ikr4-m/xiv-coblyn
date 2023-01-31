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

    public TranslateWindow(Plugin plugin) : base(
        "Translator",
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
        ImGui.Text($"Count: {Count}");
        if (ImGui.Button("+")) Count++;
        if (ImGui.Button("-")) Count--;
    }
}

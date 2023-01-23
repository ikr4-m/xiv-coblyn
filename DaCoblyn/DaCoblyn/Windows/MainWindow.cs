using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;

namespace DaCoblyn.Windows;

public class MainWindow : Window, IDisposable
{
    private TextureWrap IconImage;
    private Plugin Plugin;

    public MainWindow(Plugin plugin, TextureWrap iconApp) : base(
        "CoblynWindow", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.IconImage = iconApp;
        this.Plugin = plugin;
    }

    public void Dispose()
    {
        this.IconImage.Dispose();
    }

    public override void Draw()
    {
        ImGui.Text($"The random config bool is {this.Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

        if (ImGui.Button("Show Settings"))
        {
            this.Plugin.DrawConfigUI();
        }

        ImGui.Spacing();

        ImGui.Text("Have a goat:");
        ImGui.Indent(55);
        ImGui.Image(this.IconImage.ImGuiHandle, new Vector2(this.IconImage.Width, this.IconImage.Height));
        ImGui.Unindent(55);
    }
}

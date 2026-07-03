using Dalamud.Interface.Windowing;
using MirageUI.Layout;

namespace MirageUiSample.Windows;

internal sealed class LauncherWindow : Window
{
    private readonly LayoutPreviewSettings _settings;

    public LauncherWindow(LayoutPreviewSettings settings)
        : base("MirageUI Sample Settings###MirageUiSampleLauncher", ImGuiWindowFlags.NoResize)
    {
        _settings = settings;

        Size = new Vector2(320, 210);
        SizeCondition = ImGuiCond.Always;
    }

    public override void Draw()
    {
        ImGui.TextUnformatted("左カラム");

        var showSidebar = _settings.ShowSidebar;
        if (ImGui.Checkbox("左カラムを表示", ref showSidebar))
            _settings.ShowSidebar = showSidebar;

        ImGui.BeginDisabled(!_settings.ShowSidebar);

        var showHeader = _settings.ShowSidebarHeader;
        if (ImGui.Checkbox("プラグイン情報", ref showHeader))
            _settings.ShowSidebarHeader = showHeader;

        var showSearch = _settings.ShowSearch;
        if (ImGui.Checkbox("検索バー", ref showSearch))
            _settings.ShowSearch = showSearch;

        ImGui.BeginDisabled(!_settings.ShowSearch);
        var searchTop = _settings.SearchPosition == MirageTwoColumnSearchPosition.Top;
        if (ImGui.RadioButton("検索: 上部", searchTop) && !searchTop)
            _settings.SearchPosition = MirageTwoColumnSearchPosition.Top;

        ImGui.SameLine();
        var searchBottom = _settings.SearchPosition == MirageTwoColumnSearchPosition.Bottom;
        if (ImGui.RadioButton("\u4e0b\u90e8", searchBottom) && !searchBottom)
            _settings.SearchPosition = MirageTwoColumnSearchPosition.Bottom;
        ImGui.EndDisabled();

        var showToggle = _settings.ShowEntryToggle;
        if (ImGui.Checkbox("項目トグル", ref showToggle))
            _settings.ShowEntryToggle = showToggle;

        var showFooter = _settings.ShowSidebarFooter;
        if (ImGui.Checkbox("\u5de6\u30ab\u30e9\u30e0\u30d5\u30c3\u30bf\u30fc", ref showFooter))
            _settings.ShowSidebarFooter = showFooter;

        ImGui.EndDisabled();
    }
}

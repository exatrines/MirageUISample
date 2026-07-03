using MirageUI.Layout;

namespace MirageUiSample.Windows;

internal sealed class LayoutPreviewSettings
{
    public bool ShowSidebar { get; set; } = true;

    public bool ShowSidebarHeader { get; set; } = true;

    public bool ShowSearch { get; set; }

    public MirageTwoColumnSearchPosition SearchPosition { get; set; } = MirageTwoColumnSearchPosition.Top;

    public bool ShowEntryToggle { get; set; }

    public bool ShowSidebarFooter { get; set; }

    public void ApplyTo(MirageTwoColumnState state)
    {
        state.ShowSidebar = ShowSidebar;
        state.ShowSidebarHeader = ShowSidebarHeader;
        state.ShowSearch = ShowSearch;
        state.ShowEntryToggle = ShowEntryToggle;
        state.SearchPosition = SearchPosition;
        state.ShowSidebarFooter = ShowSidebarFooter;
    }
}

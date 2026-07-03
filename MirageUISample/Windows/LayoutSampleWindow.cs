using Dalamud.Interface.Windowing;
using MirageUiSample.UI;
using MirageUI.Layout;

namespace MirageUiSample.Windows;

internal sealed class LayoutSampleWindow : Window
{
    private readonly LayoutPreviewSettings _settings;
    private readonly ImRaii.StyleDisposable _windowStyle = new();
    private ImRaii.ColorDisposable? _colorScope;
    private readonly MirageTwoColumnState _sidebar;

    private static readonly MirageTwoColumnEntry[] Pages = CreatePages();

    private static MirageTwoColumnEntry[] CreatePages()
    {
        var pages = new List<MirageTwoColumnEntry>
        {
            new() { Id = "ui-sample", Label = "UI Sample", Enabled = true },
            new() { Id = "theme-editor", Label = "Theme Editor", Enabled = true },
        };

        for (var i = 1; i <= 40; i++)
        {
            pages.Add(new()
            {
                Id = $"scroll-{i:D2}",
                Label = $"Scroll Item {i:D2}",
                Enabled = true,
            });
        }

        return pages.ToArray();
    }

    private string _selectedPageId = "ui-sample";

    public LayoutSampleWindow(LayoutPreviewSettings settings)
        : base("Layout Preview###MirageUiSample_Preview", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        _settings = settings;
        var version = typeof(LayoutSampleWindow).Assembly.GetName().Version;
        var versionText = version != null ? $"v{version.ToString(3)}" : "v1.0.0";
        _sidebar = new MirageTwoColumnState
        {
            SearchHint = "ページを検索...",
            SidebarHeader = new MirageTwoColumnSidebarHeader
            {
                ImagePath = System.IO.Path.Combine(PluginInterface.AssemblyLocation.DirectoryName!, "sample.jpg"),
                ImageWidth = 48f,
                ImageHeight = 48f,
                ImageIsCircle = true,
                Title = "MirageUI Sample",
                Subtitle = $"{versionText} \u00b7 mirage",
            },
            SidebarFooterLinks =
            [
                new MirageTwoColumnSidebarFooterLink { Label = "GitHub", Url = "https://github.com/" },
                new MirageTwoColumnSidebarFooterLink { Label = "Contact", Url = "mailto:example@example.com" },
                new MirageTwoColumnSidebarFooterLink { Label = "Ko-fi", Url = "https://ko-fi.com/" },
            ],
        };

        Size = new Vector2(900, 630);
        SizeCondition = ImGuiCond.FirstUseEver;
        SizeConstraints = new()
        {
            MinimumSize = Size.Value,
            MaximumSize = new Vector2(4096, 2160),
        };
        Flags |= ImGuiWindowFlags.NoResize;

        _sidebar.OnSelectionChanged = id =>
        {
            _selectedPageId = string.IsNullOrEmpty(id) ? "ui-sample" : id;
        };
        _sidebar.OnEnabledChanged = (id, enabled) =>
        {
            var entry = _sidebar.Entries.FirstOrDefault(page => page.Id == id);
            if (entry != null)
                entry.Enabled = enabled;
        };

        _sidebar.Entries = Pages.Select(page => new MirageTwoColumnEntry
        {
            Id = page.Id,
            Label = page.Label,
            Enabled = page.Enabled,
        }).ToList();
    }

    public override void OnOpen()
    {
        _sidebar.SelectedId = _selectedPageId;
        _sidebar.ScrollSelectedIntoView = true;
    }

    public override void OnClose()
    {
        _sidebar.SelectedId = null;
        _windowStyle.Dispose();
    }

    public override void PreDraw()
    {
        MirageTheme.EnsureDefaultsCaptured();
        _colorScope = MirageTheme.PushCustom(MirageTheme.ResolveAppliedColors());
        _windowStyle.Push(ImGuiStyleVar.WindowPadding, Vector2.Zero);
    }

    public override void PostDraw()
    {
        _windowStyle.Dispose();
        MirageTheme.Pop(_colorScope);
        _colorScope = null;
    }

    public override void Draw()
    {
        using (MirageUi.PushFont(MirageUi.FontSize.Default))
        {
            _windowStyle.Dispose();
            _settings.ApplyTo(_sidebar);

            MirageUi.TwoColumn.Draw(_sidebar, DrawContent);

            _selectedPageId = string.IsNullOrEmpty(_sidebar.SelectedId) ? "ui-sample" : _sidebar.SelectedId;
        }
    }

    private void DrawContent()
    {
        if (_selectedPageId.StartsWith("scroll-", StringComparison.Ordinal))
        {
            var label = Pages.FirstOrDefault(page => page.Id == _selectedPageId)?.Label ?? _selectedPageId;
            MirageUi.Header(label);
            MirageUi.Text(
                "\u5de6\u30ab\u30e9\u30e0\u306e\u30b9\u30af\u30ed\u30fc\u30eb\u78ba\u8a8d\u7528\u306e\u30d7\u30ec\u30fc\u30b9\u30db\u30eb\u30c0\u30fc\u9805\u76ee\u3067\u3059\u3002",
                MirageUi.Color.Secondary);
            return;
        }

        switch (_selectedPageId)
        {
            case "theme-editor":
                ThemeEditorPanel.Draw();
                break;
            case "ui-sample":
            default:
                UiSamplePanel.Draw();
                break;
        }
    }
}

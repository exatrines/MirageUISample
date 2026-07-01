namespace MirageUiSample.UI;

internal static class UiSamplePanel
{
    private static string _searchFilter = string.Empty;
    private static readonly Vector4 CustomOrange = new(1f, 0.65f, 0.2f, 1f);

    private static readonly (string Key, string Label)[] SearchItems =
    [
        ("alpha", "Alpha Item"),
        ("beta", "Beta Item"),
        ("gamma", "Gamma Item"),
    ];

    public static void Draw()
    {
        MirageUi.SubHeader("Header");
        MirageUi.Text("ツール詳細上部と同じ、大きい見出し + 下線。", MirageUi.Color.Secondary);
        MirageUi.HeaderWithBool("Header タイトル", true);
        MirageUi.HeaderWithBool("Header タイトル", false);
        MirageUi.Header("Header タイトルのみ");
        MirageUi.Header("Header — 下線なし", underline: false);
        MirageUi.Header("Header — Accent", color: MirageUi.Color.Accent);
        MirageUi.Header("Header — Default size", fontSize: MirageUi.FontSize.Default);

        MirageUi.SubHeader("SubHeader");
        MirageUi.Text("設定画面内の通常見出し + 下線。", MirageUi.Color.Secondary);
        MirageUi.SubHeader("SubHeader 見出し", pushDown: false);
        MirageUi.SubHeader("SubHeader — 下線なし", underline: false);
        MirageUi.SubHeader("SubHeader — Accent", color: MirageUi.Color.Accent);
        MirageUi.SubHeader("SubHeader — Large", fontSize: MirageUi.FontSize.Large);

        MirageUi.SubHeader("Text — underline");
        MirageUi.Text("文字幅の下線", color: MirageUi.Color.Title, wrap: false, underline: true);
        MirageUi.Text("Large + underline", color: MirageUi.Color.Accent, fontSize: MirageUi.FontSize.Large, wrap: false, underline: true);

        MirageUi.SubHeader("Text — FontSize.Large");
        MirageUi.Text("FontSize.Large — 通常より少し大きい文字。下線なし。", color: MirageUi.Color.Secondary);
        MirageUi.Text("Large — デフォルト", color: MirageUi.Color.Title, fontSize: MirageUi.FontSize.Large);
        MirageUi.Text("Large — Secondary", color: MirageUi.Color.Secondary, fontSize: MirageUi.FontSize.Large);
        MirageUi.Text("Large — Accent", color: MirageUi.Color.Accent, fontSize: MirageUi.FontSize.Large);

        MirageUi.SubHeader("Text — FontSize");
        MirageUi.Text("FontSize.Default", fontSize: MirageUi.FontSize.Default);
        MirageUi.Text("FontSize.Large", fontSize: MirageUi.FontSize.Large);

        MirageUi.SubHeader("Text — MirageUi.Color");
        MirageUi.Text("Default — color: Default", spaced: true);
        MirageUi.Text("Secondary", MirageUi.Color.Secondary);
        MirageUi.Text("Accent", MirageUi.Color.Accent);
        MirageUi.Text("Title", MirageUi.Color.Title);
        MirageUi.Text("Warning", MirageUi.Color.Warning);
        MirageUi.Text("Default", MirageUi.Color.Default);

        MirageUi.SubHeader("Text — Vector4 直指定");
        MirageUi.Text("Vector4 直指定 — オレンジ", CustomOrange);

        MirageUi.SubHeader("Text (centered)");
        using (var child = ImRaii.Child("##CenteredContentDemo", new Vector2(-1, 72 * ImGuiHelpers.GlobalScale)))
        {
            if (child)
                MirageUi.Text("Centered + Title", MirageUi.Color.Title, wrap: false, centered: true);
        }

        MirageUi.SubHeader("PaddedSeparator");
        MirageUi.Text("通常区切り", MirageUi.Color.Secondary);
        ImGui.Separator();
        MirageUi.Text("PaddedSeparator 前", MirageUi.Color.Secondary);
        MirageUi.PaddedSeparator();
        MirageUi.Text("PaddedSeparator 後", MirageUi.Color.Secondary);

        MirageUi.SubHeader("SearchFilter / MatchesFilter");
        MirageUi.Text("検索入力と MatchesFilter による一覧フィルタ。", MirageUi.Color.Secondary);
        _ = MirageUi.SearchFilter("##UiSampleSearch"u8, ref _searchFilter, "検索...", 64);

        foreach (var (key, label) in SearchItems)
        {
            if (!MirageUi.MatchesFilter(key, label, _searchFilter))
                continue;

            var emphasized = !string.IsNullOrWhiteSpace(_searchFilter)
                && key.Contains(_searchFilter, StringComparison.OrdinalIgnoreCase);
            MirageUi.Text($"{key}: {label}", emphasized ? MirageUi.Color.Accent : MirageUi.Color.Default, wrap: false);
        }

        MirageUi.SubHeader("Image");
        MirageUi.Text("MirageUi.Image(path, width, height, isCircle, isCentering)", MirageUi.Color.Secondary);
        var samplePath = System.IO.Path.Combine(PluginInterface.AssemblyLocation.DirectoryName!, "sample.jpg");
        if (!MirageUi.Image(samplePath, 64f * ImGuiHelpers.GlobalScale, 64f * ImGuiHelpers.GlobalScale))
            MirageUi.Text("sample.jpg \u672a\u914d\u7f6e\u307e\u305f\u306f\u8aad\u307f\u8fbc\u307f\u4e2d", MirageUi.Color.Warning);
        MirageUi.Image(samplePath, 96f * ImGuiHelpers.GlobalScale, 96f * ImGuiHelpers.GlobalScale, isCircle: true, isCentering: true);

        MirageUi.SubHeader("OverlayFill");
        MirageUi.Text("MirageUi.PanelOverlay で矩形を塗りつぶす。", MirageUi.Color.Secondary);
        using (var child = ImRaii.Child("##OverlayFillDemo", new Vector2(-1, 56 * ImGuiHelpers.GlobalScale)))
        {
            if (!child)
                return;

            var pos = ImGui.GetCursorScreenPos();
            var size = ImGui.GetContentRegionAvail();
            MirageUi.OverlayFill(pos, size, ImGui.GetStyle().FrameRounding);
            ImGui.SetCursorScreenPos(pos + new Vector2(12, size.Y / 2 - ImGui.GetTextLineHeight() / 2));
            MirageUi.Text("オーバーレイ上のテキスト", MirageUi.Color.Secondary, wrap: false);
        }
    }
}

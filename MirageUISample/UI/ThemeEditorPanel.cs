namespace MirageUiSample.UI;

internal static class ThemeEditorPanel
{
    public static void Draw()
    {
        C.ThemeColors ??= new MirageColorSettings();
        MirageTheme.EnsureDefaultsCaptured();

        MirageUi.Text("MirageUI Sample ウィンドウのテーマ（配色）を変更します。", MirageUi.Color.Secondary, spaced: true);

        if (ImGui.Button("MirageDefault に戻す"))
        {
            MirageTheme.ResetToDefault(C.ThemeColors);
            C.Save();
        }

        ImGui.SameLine();

        if (ImGui.Button("Dalamud デフォルトに戻す"))
        {
            MirageTheme.ResetToDalamudDefaults(C.ThemeColors);
            C.Save();
        }

        DrawImportExport();

        ImGui.Spacing();

        MirageUi.SubHeader("フォント");

        var uiSizePt = C.ThemeColors.GetDefaultFontSizePt();
        ImGui.SetNextItemWidth(120f * ImGuiHelpers.GlobalScale);
        if (ImGui.InputFloat("Default フォントサイズ (pt)", ref uiSizePt, 0.5f, 1f, "%.1f"))
        {
            uiSizePt = Math.Clamp(uiSizePt, 1f, 127f);
            EnsureCustomSnapshot();
            C.ThemeColors.UiFontSizePt = uiSizePt;
            C.ThemeColors.UseCustomColors = true;
            C.Save();
        }

        if (ImGui.IsItemDeactivatedAfterEdit())
            C.Save();

        var sizePt = C.ThemeColors.GetLargeFontSizePt();
        ImGui.SetNextItemWidth(120f * ImGuiHelpers.GlobalScale);
        if (ImGui.InputFloat("Large フォントサイズ (pt)", ref sizePt, 0.5f, 1f, "%.1f"))
        {
            sizePt = Math.Clamp(sizePt, 1f, 127f);
            EnsureCustomSnapshot();
            C.ThemeColors.TitleFontSizePt = sizePt;
            C.ThemeColors.UseCustomColors = true;
            C.Save();
        }

        if (ImGui.IsItemDeactivatedAfterEdit())
            C.Save();

        ImGui.Spacing();

        MirageUi.SubHeader("ウィンドウ");
        DrawColorSetting(s => s.WindowBg, (s, v) => s.WindowBg = v, "WindowBg", "ウィンドウ背景", "メインウィンドウの背景色");
        DrawColorSetting(s => s.TitleBg, (s, v) => s.TitleBg = v, "TitleBg", "タイトルバー", "ウィンドウ上部のタイトル領域");

        MirageUi.SubHeader("見出し");
        DrawColorSetting(s => s.Header, (s, v) => s.Header = v, "Header", "見出し背景", "ヘッダー領域");
        DrawColorSetting(s => s.HeaderHovered, (s, v) => s.HeaderHovered = v, "HeaderHovered", "見出し（ホバー）", "マウスオーバー時の見出し");
        DrawColorSetting(s => s.HeaderActive, (s, v) => s.HeaderActive = v, "HeaderActive", "見出し（選択）", "選択中の見出し背景");

        MirageUi.SubHeader("入力");
        DrawColorSetting(s => s.FrameBg, (s, v) => s.FrameBg = v, "FrameBg", "入力背景", "入力フィールドの背景色");
        DrawColorSetting(s => s.FrameBgHovered, (s, v) => s.FrameBgHovered = v, "FrameBgHovered", "入力背景（ホバー）", "入力フィールドのホバー時背景");

        MirageUi.SubHeader("テキスト");
        DrawSemanticColor(MirageUi.Color.Default, "Default", "標準テキスト色");
        DrawSemanticColor(MirageUi.Color.Secondary, "Secondary", "補助テキスト（説明文など）");
        DrawSemanticColor(MirageUi.Color.Title, "Title", "タイトル・見出しテキスト");
        DrawSemanticColor(MirageUi.Color.Accent, "Accent", "強調表示テキスト");
        DrawSemanticColor(MirageUi.Color.Warning, "Warning", "警告テキスト色");

        MirageUi.SubHeader("その他");
        DrawSemanticColor(MirageUi.Color.PanelOverlay, "PanelOverlay", "パネルオーバーレイ", alpha: true);
        DrawColorSetting(s => s.Separator, (s, v) => s.Separator = v, "Separator", "区切り線", "セパレータの色");
        DrawColorSetting(s => s.CheckMark, (s, v) => s.CheckMark = v, "CheckMark", "チェックマーク", "チェックボックスのマーク色");
        DrawColorSetting(s => s.ChildBg, (s, v) => s.ChildBg = v, "ChildBg", "子ウィンドウ", "子領域の背景色");
    }

    private static void DrawSemanticColor(
        MirageUi.Color color,
        string label,
        string description,
        bool alpha = false)
    {
        using var id = ImRaii.PushId(label);

        var value = MirageTheme.GetEffective(C.ThemeColors!).GetColor(color);
        var changed = false;

        if (alpha)
            changed = ImGui.ColorEdit4("##Input", ref value, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.AlphaBar);
        else
        {
            var rgb = new Vector3(value.X, value.Y, value.Z);
            if (ImGui.ColorEdit3("##Input", ref rgb, ImGuiColorEditFlags.NoInputs))
            {
                value = new Vector4(rgb, value.W);
                changed = true;
            }
        }

        ImGui.SameLine();
        ImGui.BeginGroup();
        ImGui.TextWrapped(label);
        if (!string.IsNullOrEmpty(description))
            MirageUi.Text(description, MirageUi.Color.Secondary);
        ImGui.EndGroup();

        if (!changed)
            return;

        EnsureCustomSnapshot();
        C.ThemeColors!.SetColor(color, value);
        C.ThemeColors.UseCustomColors = true;
        C.Save();
    }

    private static void DrawColorSetting(
        Func<MirageColorSettings, Vector4> get,
        Action<MirageColorSettings, Vector4> set,
        string fieldName,
        string label,
        string description,
        bool alpha = false)
    {
        using var id = ImRaii.PushId(fieldName);

        var color = get(MirageTheme.GetEffective(C.ThemeColors!));
        var changed = false;

        if (alpha)
            changed = ImGui.ColorEdit4("##Input", ref color, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.AlphaBar);
        else
        {
            var rgb = new Vector3(color.X, color.Y, color.Z);
            if (ImGui.ColorEdit3("##Input", ref rgb, ImGuiColorEditFlags.NoInputs))
            {
                color = new Vector4(rgb, color.W);
                changed = true;
            }
        }

        ImGui.SameLine();
        ImGui.BeginGroup();
        ImGui.TextWrapped(label);
        if (!string.IsNullOrEmpty(description))
            MirageUi.Text(description, MirageUi.Color.Secondary);
        ImGui.EndGroup();

        if (!changed)
            return;

        EnsureCustomSnapshot();
        set(C.ThemeColors!, color);
        C.ThemeColors!.UseCustomColors = true;
        C.Save();
    }

    private static void EnsureCustomSnapshot()
    {
        if (C.ThemeColors!.UseCustomColors)
            return;

        var snapshot = MirageTheme.GetEffective(C.ThemeColors).Clone();
        snapshot.UseCustomColors = true;
        C.ThemeColors = snapshot;
    }

    private static void DrawImportExport()
    {
        MirageUi.SubHeader("エクスポート / インポート");

        if (ImGui.Button("テーマ JSON をクリップボードにコピー"))
        {
            var json = MirageColorSettingsJson.Export(MirageTheme.GetEffective(C.ThemeColors!));
            ImGui.SetClipboardText(json);
            Log.Information("テーマ JSON をクリップボードにコピーしました。");
        }

        ImGui.SameLine();

        if (ImGui.Button("クリップボードからインポート"))
        {
            var json = ImGui.GetClipboardText();
            if (MirageColorSettingsJson.TryImport(json, out var imported))
            {
                C.ThemeColors = imported;
                C.Save();
                Log.Information("テーマ JSON をインポートしました。");
            }
        }
    }
}

using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using MirageUiSample.Windows;
using MirageUI.Theme;

namespace MirageUiSample;

public sealed class Plugin : IDalamudPlugin
{
    public const string Name = "MirageUI Sample";
    private const string CommandName = "/mirageuisample";
    private const string CommandAlias = "/muisample";

    internal static Configuration C = null!;
    internal static IDalamudPluginInterface PluginInterface = null!;
    internal static IPluginLog Log = null!;

    private readonly ICommandManager _commandManager;
    private readonly WindowSystem _windowSystem;
    private readonly LayoutSampleWindow _previewWindow;
    private readonly LauncherWindow _launcherWindow;

    public Plugin(IDalamudPluginInterface pluginInterface, ICommandManager commandManager, IPluginLog log, ITextureProvider textureProvider)
    {
        PluginInterface = pluginInterface;
        Log = log;
        _commandManager = commandManager;

        C = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        C.Initialize(pluginInterface);
        C.ThemeColors ??= MirageColorSettings.CreateDefault();

        MirageUi.ConfigureTheme(() => C.ThemeColors ?? MirageColorSettings.CreateDefault());
        MirageUi.Init(pluginInterface, textureProvider, log);

        var settings = new LayoutPreviewSettings();
        _previewWindow = new LayoutSampleWindow(settings);
        _launcherWindow = new LauncherWindow(settings);

        _windowSystem = new WindowSystem("MirageUiSample");
        _windowSystem.AddWindow(_previewWindow);
        _windowSystem.AddWindow(_launcherWindow);

        pluginInterface.UiBuilder.Draw += _windowSystem.Draw;
        pluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;
        pluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUi;

        _commandManager.AddHandler(CommandName, new CommandInfo(ToggleMainUiCommand)
        {
            HelpMessage = "Open MirageUI Sample layout preview.",
        });
        _commandManager.AddHandler(CommandAlias, new CommandInfo(ToggleMainUiCommand)
        {
            HelpMessage = "Open MirageUI Sample layout preview.",
        });
    }

    private void ToggleMainUiCommand(string command, string args) => ToggleMainUi();

    private void ToggleMainUi() => _previewWindow.IsOpen = !_previewWindow.IsOpen;

    private void ToggleConfigUi() => _launcherWindow.IsOpen = !_launcherWindow.IsOpen;

    public void Dispose()
    {
        _commandManager.RemoveHandler(CommandName);
        _commandManager.RemoveHandler(CommandAlias);

        MirageUi.Dispose();

        PluginInterface.UiBuilder.Draw -= _windowSystem.Draw;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUi;
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfigUi;

        _windowSystem.RemoveAllWindows();
        C = null!;
    }
}

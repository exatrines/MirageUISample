using Dalamud.Configuration;
using Dalamud.Plugin;
using MirageUI.Theme;

namespace MirageUiSample;

[Serializable]
public sealed class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;

    public MirageColorSettings? ThemeColors;

    [NonSerialized]
    private IDalamudPluginInterface? _pluginInterface;

    public void Initialize(IDalamudPluginInterface pluginInterface) =>
        _pluginInterface = pluginInterface;

    public void Save() => _pluginInterface?.SavePluginConfig(this);
}

using KobberLan.Models;

namespace KobberLan.Services;

public interface IGameConfigService
{
    public KobberLanConfig? TryLoad(string gameFolder);
    public string GetGameFullPath();
}
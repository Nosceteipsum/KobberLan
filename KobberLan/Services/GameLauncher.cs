using System;
using System.Diagnostics;
using KobberLan.Utilities;

namespace KobberLan.Services;

public class GameLauncher : IGameLauncher
{
    public void Play(string gameFolder, string startGameFullPath)
    {
        AppLog.Info("Start game process, filename: " + startGameFullPath);
        
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = startGameFullPath,
                WorkingDirectory = gameFolder,
                UseShellExecute = true,
            };

            Process.Start(psi);
        }
        catch (Exception ex)
        {
            AppLog.Error("Error start game process.", ex);
        }        
    }
}
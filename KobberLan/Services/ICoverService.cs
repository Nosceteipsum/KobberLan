using Avalonia.Media.Imaging;
using KobberLan.Models;

namespace KobberLan.Services;

public interface ICoverService
{
    Bitmap LoadCover(LocalGame game);
}
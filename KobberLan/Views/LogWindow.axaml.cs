using Avalonia.Controls;
using KobberLan.Utilities;

namespace KobberLan.Views;

public partial class LogWindow : Window
{
    public LogWindow()
    {
        InitializeComponent();
        DataContext = AppLog.Store;
    }
}
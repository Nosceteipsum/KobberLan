using Avalonia.Controls;
using Avalonia.Interactivity;
using KobberLan.Models;

namespace KobberLan.Views;

public partial class SuggestGameWindow : Window
{
    public LocalGame? Result { get; private set; }

    public SuggestGameWindow()
    {
        InitializeComponent();
    }

    private void Game_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is LocalGame game)
        {
            Result = game;
            Close(true);
        }
    }    

}
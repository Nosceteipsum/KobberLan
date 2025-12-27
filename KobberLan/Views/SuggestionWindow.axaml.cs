using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using KobberLan.Models;
using KobberLan.ViewModels;

namespace KobberLan.Views;

public partial class SuggestGameWindow : Window
{
    public LocalGame? Result { get; private set; }

    public SuggestGameWindow()
    {
        InitializeComponent();
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SuggestGameWindowViewModel vm && vm.SelectedGame is not null)
        {
            Result = vm.SelectedGame;
            Close(true);
        }
        else
        {
            Close(false);
        }
    }

    private void Game_DoubleTapped(object? sender, TappedEventArgs e)
    {
        // Hvis du dobbeltklikker på et cover, vælg og luk som OK
        if (DataContext is SuggestGameWindowViewModel vm && vm.SelectedGame is not null)
        {
            Result = vm.SelectedGame;
            Close(true);
        }
    }
}
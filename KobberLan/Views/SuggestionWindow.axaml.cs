using Avalonia.Controls;
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

    private void Game_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SuggestGameWindowViewModel vm && vm.SelectedGame is not null)
        {
            Result = vm.SelectedGame;
            Close(true);
        }
    }    

}
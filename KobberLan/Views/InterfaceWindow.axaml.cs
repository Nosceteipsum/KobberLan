using Avalonia.Controls;
using Avalonia.Interactivity;

namespace KobberLan.Views;

public partial class InterfaceWindow : Window
{
    public InterfaceWindow()
    {
        InitializeComponent();
    }

    private void Ok_Click(object? sender, RoutedEventArgs e) => Close();
}
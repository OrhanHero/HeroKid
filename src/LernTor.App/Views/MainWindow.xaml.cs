using System.Windows;
using LernTor.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LernTor.App.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void ParentAccessButton_Click(object sender, RoutedEventArgs e)
    {
        var window = ((App)Application.Current).Services.GetRequiredService<ParentSettingsWindow>();
        window.Owner = this;
        window.ShowDialog();
    }
}

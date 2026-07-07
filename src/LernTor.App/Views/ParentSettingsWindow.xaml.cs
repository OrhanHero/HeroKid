using System.Windows;
using LernTor.App.ViewModels;

namespace LernTor.App.Views;

public partial class ParentSettingsWindow : Window
{
    private readonly ParentSettingsViewModel _viewModel;

    public ParentSettingsWindow(ParentSettingsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
        _viewModel.RequestClose += () => Dispatcher.Invoke(Close);

        Loaded += async (_, _) => await _viewModel.InitializeAsync();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoginCommand.ExecuteAsync(PasswordInput.Password);
        PasswordInput.Clear();
    }

    private void UnlockAndExitButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.UnlockAndExitCommand.Execute(PasswordInput.Password);
        PasswordInput.Clear();
    }
}

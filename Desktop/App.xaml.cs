using Desktop.Model;
using Desktop.View;
using Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ApiServiceClient _service;
        private MainViewModel _mainViewModel;
        private LoginViewModel _loginViewModel;
        private LoginWindow _loginView;
        private MainWindow _mainView;
        private PollCreatorView _editorView;
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new ApiServiceClient(ConfigurationManager.AppSettings["baseAddress"]);

            _loginViewModel = new LoginViewModel(_service);
            _loginViewModel.LoginSucceeded += _loginViewModel_LoginSucceeded;
            _loginViewModel.LoginFailed += _loginViewModel_LoginFailed;
            _loginViewModel.MessageApplication += onMessageApplication;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };

            _mainViewModel = new MainViewModel(_service);
            _mainViewModel.MessageApplication += onMessageApplication;
            _mainViewModel.LogoutSucceeded += _mainViewModel_LogoutSucceeded;
            _mainViewModel.StartingCreatePoll += _mainViewModel_StartingPoll;
            _mainViewModel.FinishingCreatePoll += _mainViewModel_FinishingPoll;

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _loginView.Show();
        }

        private void _mainViewModel_FinishingPoll(object sender, EventArgs e)
        {
                _editorView.Close();
        }

        private void _mainViewModel_StartingPoll(object sender, EventArgs e)
        {
            _editorView = new PollCreatorView
            {
                DataContext = _mainViewModel
            };
            _editorView.ShowDialog();
        }

        private void _mainViewModel_LogoutSucceeded(object sender, EventArgs e)
        {
            _mainView.Hide();
            _loginView.Show();
        }

        private void onMessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Szavazo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void _loginViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Login Failed!", "Szavazo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void _loginViewModel_LoginSucceeded(object sender, EventArgs e)
        {
            _loginView.Hide();
            _mainView.Show();
        }
    }
}

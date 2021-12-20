using Desktop.Model;
using Persistence;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace Desktop.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public Boolean IsLoading { get; set; }

        private readonly ApiServiceClient _service;

        
        public String Email { get; set; }

        public event EventHandler LoginSucceeded;

        public event EventHandler LoginFailed;

        public DelegateCommand LoginCommand { get; set; }

        public LoginViewModel(ApiServiceClient service)
        {
            _service = service;

            IsLoading = false;

            LoginCommand = new DelegateCommand(_ => !IsLoading, param => LoginAsync(param as PasswordBox));
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            try
            {
                var foo = new EmailAddressAttribute();
                IsLoading = true;
                if (!foo.IsValid(Email))
                {
                    OnMessageApplication("Email formátum nem megfelelő!");
                }
                else
                {
                    
                    Boolean result = await _service.LoginAsync(Email, passwordBox.Password);

                    if (result)
                        LoginSucceeded?.Invoke(this, EventArgs.Empty);
                    else
                        LoginFailed?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }

            finally
            {
                IsLoading = false;
            }
        }
    }
}
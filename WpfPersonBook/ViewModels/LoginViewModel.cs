using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPersonBook.Services;
using PropertyChanged;
using Splat;
using WpfPersonBook.Models;
using WpfPersonBook.Services.Data;
using WpfPersonBook.Views.UserControls;
using System.Windows;

namespace WpfPersonBook.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            IsLoginViewVisible = "Hidden";
            IsProgressBarVisible = "Hidden";
            IsMessageInfoVisible = "Visible";
            IsMessageErrorVisible = "Hidden";
            MainWindowVM = Locator.Current.GetService<MainWindowViewModel>();

            _userRepository = Locator.Current.GetService<IUserRepository>();
        }

        // Команда открывает окно авторизации
        private RelayCommand loginViewVisibleCommand;
        public RelayCommand LoginViewVisibleCommand
        {
            get
            {
                return loginViewVisibleCommand ??
                   (loginViewVisibleCommand = new RelayCommand(obj =>
                   {
                       if (IsLoginViewVisible == "Hidden")
                       {
                           ElementsVisible.IsHidden();
                           MainWindowVM.IsMainPictureVisible = "Visible";
                           IsLoginViewVisible = "Visible";
                       }
                   }));
            }
        }

        // Команда закрывает окно авторизации
        private RelayCommand loginViewCancelCommand;
        public RelayCommand LoginViewCancelCommand
        {
            get
            {
                return loginViewCancelCommand ??
                   (loginViewCancelCommand = new RelayCommand(obj =>
                   {
                       ElementsVisible.IsHidden();
                       ElementsVisible.IsMainScreenVisible();
                   }));
            }
        }

        // Команда авторизует юзера в Web API
        private RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                   (loginCommand = new RelayCommand(async obj =>
                   {
                       loginUC = Locator.Current.GetService<LoginUserControl>();

                       EmailField = loginUC.EmailTextBox.Text;

                       var user = new ApplicationUser()
                       {
                           Name = EmailField,
                           Password = loginUC.PasswordBox.Password
                       };

                       IsProgressBarVisible = "Visible";

                       var result = await _userRepository.LoginUser(user);

                       if(result.StatusCode == System.Net.HttpStatusCode.OK)
                       {
                           Token = result.Content.ReadAsStringAsync().Result;

                           MainWindowVM.AuthorizeUserName = $"Hello, {EmailField}!";
                           MainWindowVM.IsAuthorizeUserNameVisible = "Visible";

                           IsMessageInfoVisible = "Visible";
                           IsMessageErrorVisible = "Hidden";

                           ElementsVisible.IsHidden();
                           ElementsVisible.IsMainScreenVisible();

                           loginUC.PasswordBox.Password = null;
                           IsEnableLoginMenuBtn = false;
                           IsEnableLogoutMenuBtn = true;
                       }
                       else
                       {
                           MessageBox.Show($"Operation failed\nStatus Code - {result.StatusCode}", "About failed",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                           EmailField = null;
                           loginUC.PasswordBox.Password = null;
                           loginUC.EmailTextBox.Text = null;
                           IsProgressBarVisible = "Hidden";
                           IsMessageInfoVisible = "Hidden";
                           IsMessageErrorVisible = "Visible";
                       }
                   }));
            }
        }

        // Команда сбрасывает авторизацию и разлогинивает юзера
        private RelayCommand logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                return logoutCommand ?? (logoutCommand = new RelayCommand(obj =>
                {
                    Token = null;
                    EmailField = null;
                    loginUC.PasswordBox.Password = null;
                    MainWindowVM.AuthorizeUserName = null;
                    MainWindowVM.IsAuthorizeUserNameVisible = "Hidden";
                    MainWindowVM.NewContactVM.CancelFields();

                    ElementsVisible.IsHidden();
                    ElementsVisible.IsMainScreenVisible();

                    IsEnableLoginMenuBtn = true;
                    IsEnableLogoutMenuBtn = false;

                }));
            }
        }


        public string IsLoginViewVisible { get; set; }
        public string IsProgressBarVisible { get; set; }
        public string IsMessageInfoVisible { get; set; }
        public string IsMessageErrorVisible { get; set; }
        public bool IsEnableLoginMenuBtn { get; set; } = true;
        public bool IsEnableLogoutMenuBtn { get; set; }

        public MainWindowViewModel MainWindowVM { get; set; }
        public LoginUserControl loginUC { get; set; }

        public IUserRepository _userRepository { get; set; }

        public string EmailField { get; set; }
        public string Token { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PropertyChanged;
using Splat;
using WpfPersonBook.Models;
using WpfPersonBook.Services;
using WpfPersonBook.Services.Data;
using WpfPersonBook.Views.UserControls;

namespace WpfPersonBook.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            IsRegisterVisible = "Hidden";
            IsMessageInfoVisible = "Visible";
            IsMessageErrorVisible = "Hidden";

            MainWindowVM = Locator.Current.GetService<MainWindowViewModel>();

            _userRepository = Locator.Current.GetService<IUserRepository>();
        }

        // Команда открывает окно регистрации
        private RelayCommand registerViewVisibleCommand;
        public RelayCommand RegisterViewVisibleCommand
        {
            get
            {
                return registerViewVisibleCommand ??
                   (registerViewVisibleCommand = new RelayCommand(obj =>
                   {
                           ElementsVisible.IsHidden();
                           MainWindowVM.IsMainPictureVisible = "Visible";
                           IsRegisterVisible = "Visible";
                   }));
            }
        }

        // Команда закрывает окно регистрации
        private RelayCommand registerCancelCommand;
        public RelayCommand RegisterCancelCommand
        {
            get
            {
                return registerCancelCommand ??
                   (registerCancelCommand = new RelayCommand(obj =>
                   {
                       ElementsVisible.IsHidden();
                       ElementsVisible.IsMainScreenVisible();
                   }));
            }
        }

        // Команда регистрирует юзера в Web API и авторизует в Wpf клиенте
        private RelayCommand registerCommand;
        public RelayCommand RegisterCommand
        {
            get
            {
                return registerCommand ??
                   (registerCommand = new RelayCommand(async obj =>
                   {
                       registerUC = Locator.Current.GetService<RegisterUserControl>();

                       #region // Validation
                       if (registerUC.EmailTextBox.Text == null)
                       {
                           MessageBox.Show("\"Email\" field must not be empty",
                            "About \"Email\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                           return;
                       }

                       if (!ValidData.IsEmailValid(registerUC.EmailTextBox.Text))
                       {
                           MessageBox.Show("\"Email\" field is not in the correct format",
                            "About \"Email\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                           return;
                       }

                       if(registerUC.PasswordBox.Password.Length < 5)
                       {
                           MessageBox.Show("The Password length must be 5 chars and more", "About Enter Password",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                           return;
                       }

                       if(registerUC.PasswordBox.Password != registerUC.ConfirmPasswordBox.Password)
                       {
                           MessageBox.Show("The Password and Confirm Password fields must match", "About Enter Password",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                           return;
                       }
                       #endregion

                       MainWindowVM.LoginVM.EmailField = registerUC.EmailTextBox.Text;

                       var user = new ApplicationUser()
                       {
                           Name = MainWindowVM.LoginVM.EmailField,
                           Password = registerUC.PasswordBox.Password
                       };

                       MainWindowVM.LoginVM.IsProgressBarVisible = "Visible";

                       var result = await _userRepository.RegisterUser(user);

                       if (result.StatusCode == System.Net.HttpStatusCode.OK)
                       {
                           MainWindowVM.LoginVM.Token = result.Content.ReadAsStringAsync().Result;

                           MainWindowVM.AuthorizeUserName = $"Hello, {MainWindowVM.LoginVM.EmailField}!";
                           MainWindowVM.IsAuthorizeUserNameVisible = "Visible";

                           ElementsVisible.IsHidden();
                           ElementsVisible.IsMainScreenVisible();

                           registerUC.PasswordBox.Password = null;
                           registerUC.ConfirmPasswordBox.Password = null;
                           MainWindowVM.LoginVM.IsEnableLoginMenuBtn = false;
                           MainWindowVM.LoginVM.IsEnableLogoutMenuBtn = true;
                       }
                       else
                       {
                           MessageBox.Show($"Operation failed\nStatus Code - {result.StatusCode}", "About failed",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                           MainWindowVM.LoginVM.EmailField = null;
                           registerUC.PasswordBox.Password = null;
                           registerUC.ConfirmPasswordBox.Password = null;
                           MainWindowVM.LoginVM.IsProgressBarVisible = "Hidden";
                           IsMessageInfoVisible = "Hidden";
                           IsMessageErrorVisible = "Visible";
                       }
                   }));
            }
        }


        public string IsRegisterVisible { get; set; }
        public string IsMessageInfoVisible { get; set; }
        public string IsMessageErrorVisible { get; set; }

        public MainWindowViewModel MainWindowVM { get; set; }
        public IUserRepository _userRepository { get; set; }
        public RegisterUserControl registerUC { get; set; }
        public string ConfirmEmailField { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using PropertyChanged;
using Splat;
using WpfPersonBook.Models;
using WpfPersonBook.Services;
using WpfPersonBook.Services.Data;
using WpfPersonBook.Services.Mapping;
using WpfPersonBook.Views.UserControls;
using WpfPersonBook.Views.Windows;

namespace WpfPersonBook.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Locator.CurrentMutable.RegisterConstant(this);

            var mapper = AppMappingProfile.ConfigureMapping();
            Locator.CurrentMutable.Register<IMapper>(() => mapper);

            MainWin = Locator.Current.GetService<MainWindow>();

            ContactsList = new List<WpfContact>();
            CurrentUserContacts = new List<WpfContact>();

            _dataRepository = new DataRepository();
            Locator.CurrentMutable.RegisterConstant<IDataRepository>(_dataRepository);

            _userRepository = new UserRepository();
            Locator.CurrentMutable.RegisterConstant<IUserRepository>(_userRepository);

            LoginVM = new LoginViewModel();
            RegisterVM = new RegisterViewModel();
            ButtonBlockVM = new ButtonBlockViewModel();
            ShowDetailsVM = new ShowDetailsViewModel();
            NewContactVM = new NewContactViewModel();
            EditorVM = new EditorViewModel();
            DeleteVM = new DeleteViewModel();
            AboutVM = new AboutViewModel();

            IsMainPictureVisible = "Visible";
            IsContactsListVisible = "Hidden";
            IsMainTitleAppNameVisible = "Visible";
            IsAuthorizeUserNameVisible = "Hidden";
        }

        // Команда открывает окно показа всех деталей выбранного контакта
        private RelayCommand showAllContactsCommand;
        public RelayCommand ShowAllContactsCommand
        {
            get
            {
                return showAllContactsCommand ??
                   (showAllContactsCommand = new RelayCommand(async obj =>
                   {
                       if(ContactsList.Count == 0)
                       {
                           LoginVM.IsProgressBarVisible = "Visible";
                           ContactsList = await _dataRepository.GetListData();
                       }

                       MainWin.ListContactsDataGrid.ItemsSource = ContactsList;
                       SelectedItem = null;

                       ElementsVisible.IsHidden();
                       IsContactsListVisible = "Visible";
                       IsMainTitleAppNameVisible = "Visible";
                   }));
            }
        }

        // Команда фильтрует список контактов
        // и возвращает только контакты авторизованного юзера 
        private RelayCommand onlyUserContactsShowCommand;
        public RelayCommand OnlyUserContactsShowCommand
        {
            get
            {
                return onlyUserContactsShowCommand ??
                   (onlyUserContactsShowCommand = new RelayCommand(async obj =>
                   {
                       if (ContactsList.Count == 0)
                       {
                           LoginVM.IsProgressBarVisible = "Visible";
                           ContactsList = await _dataRepository.GetListData();
                       }

                       CurrentUserContacts = ContactsList.Where(u => u.UserId == LoginVM.EmailField).ToList();
                       MainWin.ListContactsDataGrid.ItemsSource = CurrentUserContacts;
                       SelectedItem = null;

                       ElementsVisible.IsHidden();
                       IsMainTitleAppNameVisible = "Visible";
                       IsContactsListVisible = "Visible";
                   }));
            }
        }

        // Команда закрывает приложение
        private RelayCommand closeAppCommand;
        public RelayCommand CloseAppCommand
        {
            get
            {
                return closeAppCommand ??
                   (closeAppCommand = new RelayCommand(obj =>
                   {
                       Application.Current.Shutdown();
                   }));
            }
        }

        // Свойство хранит выбранный юзером контакт.
        // Если юзер авторизован, то при смене свойства с null,
        // открывается блок кнопок с возможностью манипулировать выбранным контактом,
        // если контакт создан этим юзером.
        private WpfContact selectedItem;
        public WpfContact SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                if(selectedItem != null && AuthorizeUserName != null)
                {
                    ButtonBlockVM.IsButtonBlockVisible = "Visible";
                    if(selectedItem.UserId == LoginVM.EmailField)
                    {
                        ButtonBlockVM.IsButtonVisible = "Visible";
                    }
                    else
                    {
                        ButtonBlockVM.IsButtonVisible = "Hidden";
                    }
                }
                else
                {
                    ButtonBlockVM.IsButtonBlockVisible = "Hidden";
                }
            }
        }

        public MainWindow MainWin { get; set; }
        public IDataRepository _dataRepository { get; set; }
        public IUserRepository _userRepository { get; set; }
        public List<Contact> ContactsListFromApi { get; set; }
        public List<WpfContact> ContactsList { get; set; }
        public List<WpfContact> CurrentUserContacts { get; set; }

        public LoginViewModel LoginVM { get; set; }
        public RegisterViewModel RegisterVM { get; set; }
        public ButtonBlockViewModel ButtonBlockVM { get; set; }
        public ShowDetailsViewModel ShowDetailsVM { get; set; }
        public NewContactViewModel NewContactVM { get; set; }
        public EditorViewModel EditorVM { get; set; }
        public DeleteViewModel DeleteVM { get; set; }
        public AboutViewModel AboutVM { get; set; }
        public string IsContactsListVisible { get; set; }
        public string IsMainPictureVisible { get; set; }
        public string IsMainTitleAppNameVisible { get; set; }
        public string IsAuthorizeUserNameVisible { get; set; }
        public string AuthorizeUserName { get; set; }



    }
}

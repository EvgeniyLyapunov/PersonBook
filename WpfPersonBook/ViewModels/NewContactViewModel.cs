using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPersonBook.Services;
using PropertyChanged;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using WpfPersonBook.Models;
using System.Net.Http;

namespace WpfPersonBook.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class NewContactViewModel
    {
        public NewContactViewModel()
        {
            IsNewContactVisible = "Hidden";
            MainWindowVM = Locator.Current.GetService<MainWindowViewModel>();
        }

        #region // Commands
        // Каманда открывающая окно создания нового контакта
        private RelayCommand newContactUCVisibleCommand;
        public RelayCommand NewContactUCVisibleCommand
        {
            get
            {
                return newContactUCVisibleCommand ??
                   (newContactUCVisibleCommand = new RelayCommand(obj =>
                   {
                       ElementsVisible.IsHidden();
                       IsNewContactVisible = "Visible";
                       MainWindowVM.IsMainTitleAppNameVisible = "Visible";
                   }));
            }
        }

        // Каманда отмены и выхода из окна нового контакта
        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                   (cancelCommand = new RelayCommand(obj =>
                   {
                       MainWindowVM.SelectedItem = null;
                       CancelFields();
                       ElementsVisible.IsHidden();
                       ElementsVisible.IsMainScreenVisible();
                   }));
            }
        }

        // Каманда для создания нового контакта
        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
            return createCommand ??
                (createCommand = new RelayCommand(async obj =>
                {
                    #region // Validation
                    if (FirstName == null)
                    {
                        MessageBox.Show("\"First Name\" field must not be empty",
                            "About \"First Name\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (LastName == null)
                    {
                        MessageBox.Show("\"Last Name\" field must not be empty",
                            "About \"Last Name\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (PhoneNumber == null)
                    {
                        MessageBox.Show("\"Phone Number\" field must not be empty",
                            "About \"Phone Number\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if(PhoneNumber != null && !ValidData.IsPhoneValid(PhoneNumber))
                    {
                        MessageBox.Show("\"Phone Number\" field can only contain numbers",
                            "About \"Phone Number\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if(Email != null && !ValidData.IsEmailValid(Email))
                    {
                        MessageBox.Show("\"Email\" field is not in the correct format",
                            "About \"Email\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    #endregion

                    Contact contact = new Contact()
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        PhoneNumber = PhoneNumber,
                        EMail = Email,
                        AdditionalInfo = AdditionalInfo,
                        PictureName = Guid.NewGuid().ToString(),
                        UserId = MainWindowVM.LoginVM.EmailField,
                        Avatar = SelectedImageFileName != null ? Data : AppConst.GetDefaultAvatar()
                    };

                    MainWindowVM.LoginVM.IsProgressBarVisible = "Visible";
                    var result = await MainWindowVM._dataRepository.CreateData(contact, MainWindowVM.LoginVM.Token);

                    if(result.IsSuccessStatusCode)
                    {
                        MainWindowVM.ContactsList = await MainWindowVM._dataRepository.GetListData();
                        MainWindowVM.MainWin.ListContactsDataGrid.ItemsSource = MainWindowVM.ContactsList;
                        MainWindowVM.LoginVM.IsProgressBarVisible = "Hidden";

                        MessageBox.Show("New contact successfully created", "About success", MessageBoxButton.OK, MessageBoxImage.Information);

                        CancelFields();
                        ElementsVisible.IsHidden();
                        ElementsVisible.IsMainScreenVisible();
                    }
                    else
                    {
                        MainWindowVM.LoginVM.IsProgressBarVisible = "Hidden";
                        MessageBox.Show($"Something went wrong and new contact wasn't created\nStatus Cod - {result.StatusCode.GetTypeCode()}", "About success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }));
            }
        }

        // Команда открывающая окно выбора файла
        private RelayCommand openFileDialogWindowCommand;
        public RelayCommand OpenFileDialogWindowCommand
        {
            get
            {
                return openFileDialogWindowCommand ??
                   (openFileDialogWindowCommand = new RelayCommand(obj =>
                   {
                       OpenFileDialog openFileDialog = new OpenFileDialog();
                       if (openFileDialog.ShowDialog() == true)
                       {
                           Uri fileUri = new Uri(openFileDialog.FileName);
                           var SelectedImage = new BitmapImage(fileUri);
                           Data = SelectedImage.ConvertToByteArray(); 

                           var length = Data.Length;
                           if(length > AppConst.MaxSizeOfPicture)
                           {
                               MessageBox.Show("Image size must not be larger than 100Kb", "About Image size", MessageBoxButton.OK, MessageBoxImage.Information);
                               Data = null;
                               SelectedImageFileName = null;
                           }
                           else
                           {
                               var index = openFileDialog.FileName.LastIndexOf('\\');
                               var fileName = openFileDialog.FileName.Substring(index+1);
                               SelectedImageFileName = fileName;
                           }
                       }
                   }));
            }
        }
        #endregion

        #region // Fields and Properties
        public string IsNewContactVisible { get; set; }
        public MainWindowViewModel MainWindowVM { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AdditionalInfo { get; set; }
        public string SelectedImageFileName { get; set; }
        public byte[] Data { get; set; }
        #endregion

        /// <summary>
        /// Метод обнуляет введённые данные в поля View Новый контакт
        /// </summary>
        public void CancelFields()
        {
            FirstName = null;
            LastName = null;
            PhoneNumber = null;
            Email = null;
            AdditionalInfo = null;
            SelectedImageFileName = null;
            Data = null;
        }
    }
}

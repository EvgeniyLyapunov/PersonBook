using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Splat;
using PropertyChanged;
using WpfPersonBook.Models;
using WpfPersonBook.Services;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;

namespace WpfPersonBook.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class EditorViewModel
    {
        private readonly IMapper _mapper;
        public EditorViewModel()
        {
            MainWindowVM = Locator.Current.GetService<MainWindowViewModel>();
            _mapper = Locator.Current.GetService<IMapper>();
            SelectedForEditContact = new WpfContact();
            IsEditorVisible = "Hidden";
        }

        // Каманда отмены и выхода из окна редактора контакта
        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                   (cancelCommand = new RelayCommand(obj =>
                   {
                       MainWindowVM.SelectedItem = null;
                       ElementsVisible.IsHidden();
                       ElementsVisible.IsMainScreenVisible();
                   }));
            }
        }

        // Каманда для создания нового контакта
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                (updateCommand = new RelayCommand(async obj =>
                {
                    #region // Validation
                    if (SelectedForEditContact.FirstName == null)
                    {
                        MessageBox.Show("\"First Name\" field must not be empty",
                            "About \"First Name\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (SelectedForEditContact.LastName == null)
                    {
                        MessageBox.Show("\"Last Name\" field must not be empty",
                            "About \"Last Name\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (SelectedForEditContact.PhoneNumber == null)
                    {
                        MessageBox.Show("\"Phone Number\" field must not be empty",
                            "About \"Phone Number\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (SelectedForEditContact.PhoneNumber != null && !ValidData.IsPhoneValid(SelectedForEditContact.PhoneNumber))
                    {
                        MessageBox.Show("\"Phone Number\" field can only contain numbers",
                            "About \"Phone Number\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (SelectedForEditContact.EMail != null && !ValidData.IsEmailValid(SelectedForEditContact.EMail))
                    {
                        MessageBox.Show("\"Email\" field is not in the correct format",
                            "About \"Email\" field",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    #endregion

                    Contact contact = new Contact()
                    {
                        Id = SelectedForEditContact.Id,
                        FirstName = SelectedForEditContact.FirstName,
                        LastName = SelectedForEditContact.LastName,
                        PhoneNumber = SelectedForEditContact.PhoneNumber,
                        EMail = SelectedForEditContact.EMail,
                        AdditionalInfo = SelectedForEditContact.AdditionalInfo,
                        PictureName = SelectedImageFileName == null ? SelectedForEditContact.PictureName 
                                : Guid.NewGuid().ToString(),
                        UserId = SelectedForEditContact.UserId,
                        Avatar = SelectedImageFileName != null ? Data 
                                : SelectedForEditContact.Avatar.ConvertToByteArray()
                    };

                    MainWindowVM.LoginVM.IsProgressBarVisible = "Visible";
                    var result = await MainWindowVM._dataRepository.UpdateData(contact, MainWindowVM.LoginVM.Token);

                    if (result.IsSuccessStatusCode)
                    {
                        MainWindowVM.ContactsList = await MainWindowVM._dataRepository.GetListData();
                        MainWindowVM.MainWin.ListContactsDataGrid.ItemsSource = MainWindowVM.ContactsList;
                        MainWindowVM.LoginVM.IsProgressBarVisible = "Hidden";
                        MessageBox.Show("Contact successfully updated", "About success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ElementsVisible.IsHidden();
                        MainWindowVM.IsContactsListVisible = "Visible";
                        MainWindowVM.IsMainTitleAppNameVisible = "Visible";
                    }
                    else
                    {
                        MainWindowVM.LoginVM.IsProgressBarVisible = "Hidden";
                        MessageBox.Show($"Something went wrong and contact wasn't updated\nStatus Cod - {result.StatusCode}", "About success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                           SelectedImage = new BitmapImage(fileUri);

                           Data = SelectedImage.ConvertToByteArray();

                           var length = Data.Length;
                           if (length > AppConst.MaxSizeOfPicture)
                           {
                               MessageBox.Show("Image size must not be larger than 100Kb", "About Image size", MessageBoxButton.OK, MessageBoxImage.Information);
                               Data = null;
                               SelectedImageFileName = null;
                           }
                           else
                           {
                               var index = openFileDialog.FileName.LastIndexOf('\\');
                               var fileName = openFileDialog.FileName.Substring(index + 1);
                               SelectedImageFileName = fileName;
                           }
                       }
                   }));
            }
        }

        public MainWindowViewModel MainWindowVM { get; set; }

        // Свойство открытия окна редактирования.
        // Если значение "Visible" - в свойство SelectedForEditContact
        // мапится выбранный контакт с целью не изменять контакт при отмене
        // в процессе редактирования
        private string isEditorVisible;
        public string IsEditorVisible
        {
            get => isEditorVisible;
            set
            {
                isEditorVisible = value;
                if(isEditorVisible == "Visible")
                {
                    SelectedForEditContact = _mapper.Map<WpfContact>(MainWindowVM.SelectedItem);
                    SelectedImage = SelectedForEditContact.Avatar;
                }
            }
        }

        public WpfContact SelectedForEditContact { get; set; }
        public string SelectedImageFileName { get; set; }
        public byte[] Data { get; set; }
        public BitmapImage SelectedImage { get; set; }


    }
}

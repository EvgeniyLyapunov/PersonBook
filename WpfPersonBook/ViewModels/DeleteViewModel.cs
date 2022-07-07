using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPersonBook.Services;
using PropertyChanged;
using System.Windows;

namespace WpfPersonBook.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class DeleteViewModel
    {
        public DeleteViewModel()
        {
            MainWindowVM = Locator.Current.GetService<MainWindowViewModel>();
            IsDeleteVisible = "Hidden";
        }

        // Каманда отмены и выхода из окна удаления контакта
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
                       MainWindowVM.IsContactsListVisible = "Visible";
                       MainWindowVM.IsMainTitleAppNameVisible = "Visible";
                   }));
            }
        }

        // Каманда удаления контакта
        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                (deleteCommand = new RelayCommand(async obj =>
                {
                    MainWindowVM.LoginVM.IsProgressBarVisible = "Visible";

                    var result = await MainWindowVM._dataRepository.DeleteData(MainWindowVM.SelectedItem.Id, MainWindowVM.LoginVM.Token);

                    if (result.IsSuccessStatusCode)
                    {
                        MainWindowVM.ContactsList = await MainWindowVM._dataRepository.GetListData();
                        MainWindowVM.MainWin.ListContactsDataGrid.ItemsSource = MainWindowVM.ContactsList;
                        MainWindowVM.LoginVM.IsProgressBarVisible = "Hidden";
                        MainWindowVM.SelectedItem = null;
                        MessageBox.Show("Contact successfully deleted", "About success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ElementsVisible.IsHidden();
                        MainWindowVM.IsContactsListVisible = "Visible";
                        MainWindowVM.IsMainTitleAppNameVisible = "Visible";
                    }
                    else
                    {
                        MainWindowVM.LoginVM.IsProgressBarVisible = "Hidden";
                        MessageBox.Show($"Something went wrong and contact wasn't deleted\nStatus Cod - {result.StatusCode}", "About success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }));
            }
        }

        public MainWindowViewModel MainWindowVM { get; set; }
        public string IsDeleteVisible { get; set; }
    }
}

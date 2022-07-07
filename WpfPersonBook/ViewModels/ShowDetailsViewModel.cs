using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using WpfPersonBook.Services;

namespace WpfPersonBook.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ShowDetailsViewModel
    {
        public ShowDetailsViewModel()
        {
            IsShowDitailsVisible = "Hidden";
            MainWindowVM = Locator.Current.GetService<MainWindowViewModel>();
        }

        public string IsShowDitailsVisible { get; set; }
        public MainWindowViewModel MainWindowVM { get; set; }

        // Команда закрывает окно детального показа выбранного контакта
        private RelayCommand closeShowDetailsCommand;
        public RelayCommand CloseShowDetailsCommand
        {
            get
            {
                return closeShowDetailsCommand ??
                   (closeShowDetailsCommand = new RelayCommand(obj =>
                   {
                       MainWindowVM.SelectedItem = null;
                       ElementsVisible.IsHidden();
                       MainWindowVM.IsContactsListVisible = "Visible";
                       MainWindowVM.IsMainTitleAppNameVisible = "Visible";
                   }));
            }
        }

    }
}

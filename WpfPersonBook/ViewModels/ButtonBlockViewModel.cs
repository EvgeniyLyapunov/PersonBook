using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Splat;
using WpfPersonBook.Services;

namespace WpfPersonBook.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ButtonBlockViewModel
    {
        public ButtonBlockViewModel()
        {
            IsButtonBlockVisible = "Hidden";
            IsButtonVisible = "Hidden";
            MainWindowVM = Locator.Current.GetService<MainWindowViewModel>();
        }

        // Команда открывает окно детального отображения выбранного контакта
        private RelayCommand showDetailsCommand;
        public RelayCommand ShowDetailsCommand
        {
            get
            {
                return showDetailsCommand ??
                   (showDetailsCommand = new RelayCommand(obj =>
                   {
                       ElementsVisible.IsHidden();
                       MainWindowVM.ShowDetailsVM.IsShowDitailsVisible = "Visible";
                       MainWindowVM.IsMainTitleAppNameVisible = "Visidle";
                   }));
            }
        }

        // Команда открывает окно редактирования выбранного контакта
        private RelayCommand showEditorCommand;
        public RelayCommand ShowEditorCommand
        {
            get
            {
                return showEditorCommand ??
                   (showEditorCommand = new RelayCommand(obj =>
                   {
                       ElementsVisible.IsHidden();
                       MainWindowVM.EditorVM.IsEditorVisible = "Visible";
                       MainWindowVM.IsMainTitleAppNameVisible = "Visible";
                   }));
            }
        }

        // Команда открывает окно удаления выбранного контакта
        private RelayCommand showDeleteCommand;
        public RelayCommand ShowDeleteCommand
        {
            get
            {
                return showDeleteCommand ??
                   (showDeleteCommand = new RelayCommand(obj =>
                   {
                       ElementsVisible.IsHidden();
                       MainWindowVM.DeleteVM.IsDeleteVisible = "Visible";
                       MainWindowVM.IsMainTitleAppNameVisible = "Visible";
                   }));
            }
        }



        public string IsButtonBlockVisible { get; set; }
        public MainWindowViewModel MainWindowVM { get; set; }
        public string IsButtonVisible { get; set; }
    }
}

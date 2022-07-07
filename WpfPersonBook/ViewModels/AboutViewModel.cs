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
    public class AboutViewModel
    {
        public AboutViewModel()
        {
            IsAboutVisible = "Hidden";
        }

        // Команда открывает окно About this project
        private RelayCommand showAboutCommand;
        public RelayCommand ShowAboutCommand
        {
            get
            {
                return showAboutCommand ??
                   (showAboutCommand = new RelayCommand( obj =>
                   {
                       IsAboutVisible = "Visible";
                   }));
            }
        }

        // Команда открывает окно About this project
        private RelayCommand closeAboutCommand;
        public RelayCommand CloseAboutCommand
        {
            get
            {
                return closeAboutCommand ??
                   (closeAboutCommand = new RelayCommand(obj =>
                   {
                       IsAboutVisible = "Hidden";
                   }));
            }
        }


        public string IsAboutVisible { get; set; }
    }
}

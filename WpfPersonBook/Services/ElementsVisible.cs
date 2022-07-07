using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPersonBook.ViewModels;
using Splat;

namespace WpfPersonBook.Services
{
    public class ElementsVisible
    {
        /// <summary>
        /// Метод закрывает все Юзер Контролы
        /// </summary>
        public static void IsHidden()
        {
            var MainWindowVM = Locator.Current.GetService<MainWindowViewModel>();

            MainWindowVM.IsMainPictureVisible = "Hidden";
            MainWindowVM.IsMainTitleAppNameVisible = "Hidden";

            MainWindowVM.IsContactsListVisible = "Hidden";
            MainWindowVM.ShowDetailsVM.IsShowDitailsVisible = "Hidden";
            MainWindowVM.NewContactVM.IsNewContactVisible = "Hidden";
            MainWindowVM.EditorVM.IsEditorVisible = "Hidden";
            MainWindowVM.DeleteVM.IsDeleteVisible = "Hidden";
            MainWindowVM.AboutVM.IsAboutVisible = "Hidden";

            MainWindowVM.LoginVM.IsLoginViewVisible = "Hidden";
            MainWindowVM.RegisterVM.IsRegisterVisible = "Hidden";
            MainWindowVM.ButtonBlockVM.IsButtonBlockVisible = "Hidden";

            MainWindowVM.LoginVM.IsProgressBarVisible = "Hidden";

            MainWindowVM.RegisterVM.IsMessageInfoVisible = "Visible";
            MainWindowVM.RegisterVM.IsMessageErrorVisible = "Hidden";
        }

        /// <summary>
        /// Метод выводит заставку и название приложения
        /// </summary>
        public static void IsMainScreenVisible()
        {
            var MainWindowVM = Locator.Current.GetService<MainWindowViewModel>();

            MainWindowVM.IsMainPictureVisible = "Visible";
            MainWindowVM.IsMainTitleAppNameVisible = "Visible";
        }


    }
}

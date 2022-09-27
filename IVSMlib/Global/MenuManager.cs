using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IVSMlib.Utils;
using IVSMlib.ViewModel;
using IVSMlib.Windows;

namespace IVSMlib.Global
{
    public static class MenuManager
    {
        private static WMainMenu MainMenuWindow;

        private static bool IsMenuShow = false;

        public static void OpenMainMenu(ControlBar window)
        {
            if (IsMenuShow == false)
            {
                MainMenuWindow = new WMainMenu();

                MainMenuWindow.Left =WindowExtensions.GetWindowLeft(Window.GetWindow(window));
                MainMenuWindow.Top = WindowExtensions.GetWindowTop(Window.GetWindow(window));
                MainMenuWindow.Owner = Application.Current.MainWindow;
                IsMenuShow = true;
                MainMenuWindow.Show();
            }
        }

        public static void MouseDown()
        {
            if(IsMenuShow == true)
            {
                MainMenuWindow.Close();
                MainMenuWindow = null;
                IsMenuShow = false;
            }
        }
    }
}

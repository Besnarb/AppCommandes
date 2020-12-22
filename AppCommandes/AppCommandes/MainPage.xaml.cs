using AppCommandes.Data;
using AppCommandes.MenuControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppCommandes
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public struct Menu
        {
            public string Name { get; set; }
            //public UserControl UserControl { get; set; }
        }
        public ObservableCollection<Menu> Menus { get; set; }
        public UserControl actualMenu;
        public MainPage()
        {
            Menus = new ObservableCollection<Menu>()
            {
                new Menu()
                {
                    Name = "Retirer",
                },
                new Menu()
                {
                    Name = "Ajouter",
                },
                new Menu()
                {
                    Name = "Totaux",
                }
            };
            DataContext = this;
            this.InitializeComponent();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            switch (((Menu)((sender as Button).DataContext)).Name)
            {
                case "Retirer":
                    var retTmp = new Retirer();
                    retTmp.ModifyRequested += RetTmp_ModifyRequested;
                    actualMenu = retTmp;
                    break;
                case "Ajouter":
                    actualMenu = new Ajouter();
                    break;
                case "Totaux":
                    actualMenu = new Totaux();
                    break;
            }
            MainMenu.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Visible;
            Root.Children.Add(actualMenu);
            actualMenu.Unloaded += ActualMenu_Unloaded;

            //switch (menu.Name)
            //{
            //    case "Retirer":

            //        break;
            //    case "Ajouter":
            //        MainMenu.Visibility = Visibility.Collapsed;
            //        break;
            //    case "Totaux":
            //        MainMenu.Visibility = Visibility.Collapsed;
            //        break;
            //    default:
            //        break;
            //}
        }
        bool CanUse = true;
        private void RetTmp_ModifyRequested(object sender, Client e)
        {
            CanUse = false;
            Root.Children.Remove(actualMenu);
            actualMenu = new Ajouter(e);
            MainMenu.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Visible;
            Root.Children.Add(actualMenu);
            actualMenu.Unloaded += ActualMenu_Unloaded;
        }

        private void ActualMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            if (CanUse)
            {
                if (actualMenu is Retirer)
                {
                    var ret = actualMenu as Retirer;
                    if (ret.IsDetailedView)
                        ret.BackFromDetailedView();
                    else
                    {
                        MainMenu.Visibility = Visibility.Visible;
                        BackButton.Visibility = Visibility.Collapsed;
                        Root.Children.Remove(actualMenu);
                    }
                }
                else
                {
                    MainMenu.Visibility = Visibility.Visible;
                    BackButton.Visibility = Visibility.Collapsed;
                    Root.Children.Remove(actualMenu);
                }
            }
            CanUse = true;
        }
    }
}

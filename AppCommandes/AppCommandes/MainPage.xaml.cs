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
            public UserControl UserControl { get; set; }
        }
        public DataHolder DataHolder { get; set; }
        public ObservableCollection<Menu> Menus { get; set; }
        public UserControl actualMenu;
        public MainPage()
        {
            DataHolder = new DataHolder();
            Menus = new ObservableCollection<Menu>()
            {
                new Menu()
                {
                    Name = "Retirer",
                    UserControl = new Retirer ()
                },
                new Menu()
                {
                    Name = "Ajouter",
                    UserControl = new Ajouter ()
                },
                new Menu()
                {
                    Name = "Totaux",
                    UserControl = new Totaux ()
                }
            };
            //foreach (var product in DataHolder.Products)
            //{
            //    if (Menus.Where(Menu => Menu.Name == product.Category).Count() == 0)
            //    {

            //    }
            //}
            DataContext = this;
            this.InitializeComponent();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            actualMenu = ((Menu)((sender as Button).DataContext)).UserControl;
            MainMenu.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Visible;
            Root.Children.Add(actualMenu);

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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Collapsed;
            Root.Children.Remove(actualMenu);
        }
    }
}

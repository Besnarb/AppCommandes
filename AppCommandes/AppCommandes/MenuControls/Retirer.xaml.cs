using AppCommandes.Data;
using System;
using System.Collections.Generic;
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

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppCommandes.MenuControls
{
    public sealed partial class Retirer : UserControl
    {
        DataHolder dataHolder;
        public Retirer()
        {
            this.InitializeComponent();
            this.Loaded += Retirer_Loaded;
        }

        private void Retirer_Loaded(object sender, RoutedEventArgs e)
        {
            dataHolder = ((MainPage)DataContext).DataHolder;
            ClientsList.ItemsSource = dataHolder.Clients;
        }

        private void Date_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DisplayAll_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

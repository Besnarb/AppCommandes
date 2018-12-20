using AppCommandes.Data;
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

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppCommandes.MenuControls
{
    public sealed partial class Retirer : UserControl
    {
        DataHolder dataHolder;
        //0 partie
        //1 En attente
        //2 en Attente + note
        public Retirer()
        {
            this.InitializeComponent();
            this.Loaded += Retirer_Loaded;
        }
        void UpdateCollection()
        {
            dataHolder = ((MainPage)DataContext).DataHolder;
            if (DisplayAll.IsChecked == false)
                ClientsList.ItemsSource = dataHolder.Clients.Where(cmd => cmd.Name.ToLower() == SearchBar.Text.ToLower()).AsEnumerable();
            else
                ClientsList.ItemsSource = dataHolder.Clients.Where(tmp => tmp.State > 0).Where(tmp => tmp.Name.ToUpper().Contains(SearchBar.Text.ToUpper()));
        }
        private void Retirer_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCollection();
        }
      
        private void DisplayAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateCollection();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCollection();
        }
    }
}

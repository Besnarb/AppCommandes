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
        ObservableCollection<Client> ClientCollection;
        //0 partie
        //1 En attente
        //2 en Attente + note
        public Retirer()
        {
            ClientCollection = new ObservableCollection<Client>();
            this.InitializeComponent();
            this.Loaded += Retirer_Loaded;
        }
        void UpdateCollection()
        {
            dataHolder = ((MainPage)DataContext).DataHolder;
            ClientCollection.Clear();
            if (DisplayAll.IsChecked == true)
                ClientCollection = dataHolder.Clients;
            else
            {
                ClientCollection.ToList().AddRange(dataHolder.Clients.Where(cmd => cmd.State > 0));
                if (Noel.IsChecked == true)
                    ClientCollection.ToList().AddRange(ClientCollection.Where(cmd => cmd.Day == 24));
                if (An.IsChecked == true)
                    ClientCollection.ToList().AddRange(ClientCollection.Where(cmd => cmd.Day == 31));

                if (SearchBar.Text.Count() > 0)
                    ClientCollection = (ObservableCollection<Client>)ClientCollection.Where(cmd => cmd.Name.ToLower() == SearchBar.Text.ToLower()).AsEnumerable();
            }
            ClientsList.ItemsSource = ClientCollection.OrderBy(cmd => cmd.Hour);
        }
        private void Retirer_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCollection();
        }

        private void Date_Click(object sender, RoutedEventArgs e)
        {
            UpdateCollection();
        }

        private void DisplayAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateCollection();
        }
    }
}

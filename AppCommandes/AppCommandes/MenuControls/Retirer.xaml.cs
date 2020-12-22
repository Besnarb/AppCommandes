using AppCommandes.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        DataHolder DataHolder;
        public bool IsDetailedView { get; set; }
        //0 partie
        //1 En attente
        //2 en Attente + note
        public Retirer()
        {
            this.InitializeComponent();
            IsDetailedView = false;
            this.Loaded += Retirer_Loaded;
        }
        private async Task UpdateCollection()
        {
            DataHolder = new DataHolder();
            await DataHolder.Init();
            if (DisplayAll.IsChecked == true)
                ClientsList.ItemsSource = DataHolder.Clients.Where(cmd => cmd.Name.ToUpper().Contains(SearchBar.Text.ToUpper()));
            else
                ClientsList.ItemsSource = DataHolder.Clients.Where(tmp => tmp.State > 0).Where(tmp => tmp.Name.ToUpper().Contains(SearchBar.Text.ToUpper())); // Enlever le supérieur ou égal
            TotalListe.Text = ClientsList.Items.Count.ToString() + "/" + DataHolder.Clients.Count.ToString();
        }
        private async void Retirer_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateCollection();
        }
      
        private async void DisplayAll_Click(object sender, RoutedEventArgs e)
        {
            await UpdateCollection();
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            await UpdateCollection();
        }
        public void BackFromDetailedView()
        {
            IsDetailedView = false;
            ClientsList.Visibility = Visibility.Visible;
            DetailedGrid.Visibility = Visibility.Collapsed;
        }
        private void ClientsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Client clientToDisplay =  e.ClickedItem as Client;
            ClientsList.Visibility = Visibility.Collapsed;
            DetailedGrid.Visibility = Visibility.Visible;
            RetirerToggle.IsOn = clientToDisplay.State == 0 ? true : false;
            DetailedGrid.DataContext = clientToDisplay;
            IsDetailedView = true;

        }
        public delegate void EventHandler(object sender, Client e);
        public event EventHandler ModifyRequested;
        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            ModifyRequested?.Invoke(this, (DetailedGrid.DataContext as Client));
        }

        private async void Retirer_Toggled(object sender, RoutedEventArgs e)
        {
            var client = (DetailedGrid.DataContext as Client);
            if (client != null)
            {
                if (client.State != 0)
                    client.State = 0;
                else
                    client.State = client.Remarks == string.Empty ? 1 : 2;
                await DataHolder.ModifyClient(client);
                await UpdateCollection();
            }
        }

        private async void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            var client = (DetailedGrid.DataContext as Client);
            if (client != null)
            {
                SearchBar.Text = string.Empty;
                BackFromDetailedView();
                await DataHolder.RemoveClient(client);
                await UpdateCollection();
            }
        }
    }
}

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
        void UpdateCollection()
        {
            dataHolder = ((MainPage)DataContext).DataHolder;
            if (DisplayAll.IsChecked == false)
                ClientsList.ItemsSource = dataHolder.Clients.Where(cmd => cmd.Name.ToLower() == SearchBar.Text.ToLower()).AsEnumerable();
            else
                ClientsList.ItemsSource = dataHolder.Clients.Where(tmp => tmp.State >= 0).Where(tmp => tmp.Name.ToUpper().Contains(SearchBar.Text.ToUpper())); // Enlever le supérieur ou égal
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

        private void Retirer_Toggled(object sender, RoutedEventArgs e)
        {
            var client = (DetailedGrid.DataContext as Client);
            if (client != null)
            {
                if (client.State != 0)
                    client.State = 0;
                else
                    client.State = client.Remarks == string.Empty ? 1 : 2;
                dataHolder.Save();
            }
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            var client = (DetailedGrid.DataContext as Client);
            if (client != null)
            {
                dataHolder.Clients.Remove(client);
                SearchBar.Text = string.Empty;
                UpdateCollection();
                BackFromDetailedView();
                dataHolder.Save();
            }
        }
    }
}

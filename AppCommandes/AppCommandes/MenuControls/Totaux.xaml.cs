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
    public sealed partial class Totaux : UserControl
    {
        public class Prod
        {
            public Product Product { get; set; }
            public int Quant { get; set; }
            public string DisplayString
            {
                get
                {
                    return String.Format("{0} : {1}", Product.Name, Quant);
                }
            }
        }

        public ObservableCollection<Prod> observableCollection;
        DataHolder dataHolder;
        public Totaux()
        {
            this.InitializeComponent();
            this.Loaded += Totaux_Loaded;
        }

        private void Totaux_Loaded(object sender, RoutedEventArgs e)
        {
            observableCollection = new ObservableCollection<Prod>();
            dataHolder = ((MainPage)DataContext).DataHolder;
            foreach (var data in dataHolder.Products)
            {
                observableCollection.Add(new Prod() { Product = data, Quant = 0 });
            }
            foreach (var client in dataHolder.Clients)
            {
                foreach (var pr in client.Products)
                {
                    //https://stackoverflow.com/questions/6781192/how-do-i-update-a-single-item-in-an-observablecollection-class
                    var prod = observableCollection.First(tmp => tmp.Product.Name == pr.Product.Name);
                    var idx = observableCollection.IndexOf(prod);
                    prod.Quant += pr.Quantity;
                    observableCollection[idx] = prod;
                }
            }
            Total.ItemsSource = observableCollection;
        }
    }
}

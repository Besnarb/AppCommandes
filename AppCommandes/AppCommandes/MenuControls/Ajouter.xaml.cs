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
    public sealed partial class Ajouter : UserControl
    {
        DataHolder DataHolder { get; set; }
        ObservableCollection<String> Categories { get; set; }
        ObservableCollection<String> SelectedProducts { get; set; }

        public Ajouter()
        {
            Categories = new ObservableCollection<string>();
            this.Loaded += Ajouter_Loaded;
            this.InitializeComponent();
        }

        private void Ajouter_Loaded(object sender, RoutedEventArgs e)
        {
            DataHolder = ((MainPage)DataContext).DataHolder;
            foreach (var product in DataHolder.Products)
            {
                if (!Categories.Contains(product.Category))
                {
                    Categories.Add(product.Category);
                }
            }
            ProductsMenu.ItemsSource = Categories;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedProducts = new ObservableCollection<string>();
            foreach (var products in DataHolder.Products)
            {
                if (products.Category == (String)((Button)sender).DataContext)
                    SelectedProducts.Add(products.Name);
            }
            ProductsMenu.ItemsSource = SelectedProducts;
        }
    }
}

using AppCommandes.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
        ObservableCollection<Product> Categories { get; set; }
        ObservableCollection<Product> SelectedProducts { get; set; }
        ObservableCollection<OrderedProduct> OrderedProducts { get; set; }

        public Ajouter()
        {
            Categories = new ObservableCollection<Product>();
            OrderedProducts = new ObservableCollection<OrderedProduct>();
            this.Loaded += Ajouter_Loaded;
            this.InitializeComponent();
        }

        private void Ajouter_Loaded(object sender, RoutedEventArgs e)
        {
            ProductsList.ItemsSource = OrderedProducts;
            DataHolder = ((MainPage)DataContext).DataHolder;
            foreach (var product in DataHolder.Products)
            {
                if (!Categories.Any( cat => cat.Category == product.Category))
                {
                    Categories.Add(new Product()
                    {
                        Category = product.Category,
                        Name = product.Category,
                        Price = 0,
                        Slicable = null
                    });
                }
            }
            ProductsMenu.ItemsSource = Categories;
        }

        int state = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (state)
            {
                case 0:
                    SelectedProducts = new ObservableCollection<Product>();
                    foreach (var products in DataHolder.Products)
                    {
                        if (products.Category == ((Product)((Button)sender).DataContext).Category)//
                            SelectedProducts.Add(products);
                    }
                    ProductsMenu.ItemsSource = SelectedProducts;
                    state++;
                    break;
                case 1:
                    ProductsMenu.ItemsSource = Categories;
                    OrderedProducts.Add(new OrderedProduct()
                    {
                        Product = (Product)((Button)sender).DataContext,
                        Quantity = 1,
                        Sliced = null
                    });
                    state--;
                    break;
            }
      
        }

        private void NbProd_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            OrderedProduct data = (OrderedProduct) ((Button) sender).DataContext;
            if (btn.Content.ToString() == "+")
                data.Quantity++;
            else if (btn.Content.ToString() == "-" && data.Quantity > 1)
                data.Quantity--;
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            int completed = 0;
            if (ClientName.Text.Count() == 0)
            {
                ClientName.Background = new SolidColorBrush(Colors.Red);
                completed++;
            }
            if (Phone.Text.Count() == 0)
            {
                Phone.Background = new SolidColorBrush(Colors.Red);
                completed++;
            }
            if (ProductsList.Items.Count() == 0)
            {
                ClientName.Background = new SolidColorBrush(Colors.Red);
                completed++;
            }
            if (completed == 0)
                ;
        }
    }
}

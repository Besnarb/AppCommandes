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
        Client LocalClient { get; set; }
        public Ajouter(Client client)
        {
            Categories = new ObservableCollection<Product>();
            LocalClient = client;
            this.Loaded += Ajouter_Loaded1;
            this.InitializeComponent();
        }

        private void Ajouter_Loaded1(object sender, RoutedEventArgs e)
        {
            ClientName.Text = LocalClient.Name;
            Phone.Text = LocalClient.Phone;
            Remarks.Text = LocalClient.Remarks;
            OrderedProducts = LocalClient.Products;
            ProductsList.ItemsSource = OrderedProducts;
            DataHolder = ((MainPage)DataContext).DataHolder;
            foreach (var product in DataHolder.Products)
            {
                if (!Categories.Any(cat => cat.Category == product.Category))
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

        public Ajouter()
        {
            LocalClient = null;
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
                    ProductsMenuBack.IsEnabled = true;
                    ProductsMenu.ItemsSource = SelectedProducts;
                    state++;
                    break;
                case 1:
                    ProductsMenu.ItemsSource = Categories;
                    ProductsMenuBack.IsEnabled = false;
                    var prod = (Product)((Button)sender).DataContext;
                    var query = OrderedProducts.FirstOrDefault(op => op.Product == prod);
                    if (query != null)
                    {
                        query.Quantity++;
                    }
                    else
                    {
                        OrderedProducts.Add(new OrderedProduct()
                        {
                            Product = prod,
                            Quantity = 1,
                            Sliced = false,
                            Slicable = prod.Category == "Pain" ? true : false
                        });
                    }
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
            {
                if (LocalClient != null)
                {
                    var idx = DataHolder.Clients.IndexOf(DataHolder.Clients.First(d => d.Number == LocalClient.Number));
                    DataHolder.Clients[idx] = new Client()
                    {
                        Name = ClientName.Text,
                        Phone = Phone.Text,
                        Remarks = Remarks.Text,
                        Day = Day.Date,
                        Hour = Day.Hour,
                        State = Remarks.Text == string.Empty ? 1 : 2,
                        Products = OrderedProducts,
                        Number = LocalClient.Number
                    };
                }
                else
                {
                    DataHolder.Clients.Add(new Client()
                    {
                        Name = ClientName.Text,
                        Phone = Phone.Text,
                        Remarks = Remarks.Text,
                        Day = Day.Date,
                        Hour = Day.Hour,
                        State = Remarks.Text == string.Empty ? 1 : 2,
                        Products = OrderedProducts,
                        Number = DataHolder.Clients.Last().Number + 1
                    });
                }
                DataHolder.Save();
                ClientName.Text = "";
                Phone.Text = "";
                Remarks.Text = "";
                OrderedProducts.Clear();
                ProductsMenu.ItemsSource = Categories;
                ((Grid)this.Parent).Children.Remove(this);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            OrderedProduct data = (OrderedProduct)((Button)sender).DataContext;
            OrderedProducts.Remove(data);
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            OrderedProduct data = (OrderedProduct)((ToggleButton)sender).DataContext;
            data.Sliced = ((ToggleButton)sender).IsChecked.Value;
        }

        private void ProductsMenuBack_Click(object sender, RoutedEventArgs e)
        {
            ProductsMenu.ItemsSource = Categories;
            state--;
        }
    }
}

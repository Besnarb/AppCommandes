using AppCommandes.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AppCommandes.Data
{
    public class DataHolder
    {
        public ObservableCollection<Product> Products { get; set; }
     
        public DataHolder()
        {
            PopulateProductsAsync();
        }
        async void PopulateProductsAsync()
        {
            StorageFile sampleFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Json/Products.json"));
            Products = new ObservableCollection<Product>();
            string json = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
            System.Diagnostics.Debug.WriteLine("Products loaded");
        }
    }
}

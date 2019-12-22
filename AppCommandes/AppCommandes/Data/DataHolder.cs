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
        public ObservableCollection<Client> Clients { get; set; }
        private StorageFolder StorageFolder { get; set; }
        private StorageFile clientsFile { get; set; }
        public DataHolder()
        {
            StorageFolder = Windows.Storage.ApplicationData.Current.LocalFolder; // C:\Users\Antoine\AppData\Local\Packages\ApplicationCommande2018_zfcpjwz6qmvxy\LocalState
            Refresh();
        }
        public void Refresh()
        {
            PopulateProductsAsync();
            PopulateClientsAsync();
        }
        public async void Save()
        {
            var json = JsonConvert.SerializeObject(Clients);
            await FileIO.WriteTextAsync(clientsFile, json);
        }
        async void PopulateClientsAsync()
        {
            clientsFile = await StorageFolder.CreateFileAsync("Clients.json",CreationCollisionOption.OpenIfExists);
            string json = await FileIO.ReadTextAsync(clientsFile);
            Clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(json);
            if (Clients == null)
                Clients = new ObservableCollection<Client>();
            //int i = 0;
            //foreach(var c in Clients)
            //{
            //    c.Number = i;
            //    i++;
            //}
            //Save();
            System.Diagnostics.Debug.WriteLine("Clients loaded");
        }

        async void PopulateProductsAsync()
        {
            StorageFile sampleFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Json/Products.json"));
            string json = await Windows.Storage.FileIO.ReadTextAsync(sampleFile,Windows.Storage.Streams.UnicodeEncoding.Utf8);
            Products = new ObservableCollection<Product>(JsonConvert.DeserializeObject<ObservableCollection<Product>>(json).OrderBy(e => e.Category).ThenBy(f => f.Name));
            System.Diagnostics.Debug.WriteLine("Products loaded");
        }
    }
}

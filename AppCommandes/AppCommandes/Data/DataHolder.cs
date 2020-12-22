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
    public static class ExtensionMethods
    {
        public static int Remove<T>(
            this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }
    }
    public class DataHolder
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        private StorageFolder StorageFolder { get; set; }
        public DataHolder()
        {
            StorageFolder = Windows.Storage.ApplicationData.Current.LocalFolder; // C:\Users\Antoine\AppData\Local\Packages\ApplicationCommande2018_zfcpjwz6qmvxy\LocalState
            System.Diagnostics.Debug.WriteLine("DataHolder Created");
        }

        public async Task Init()
        {
            await InitProducts();
            await InitClients();
            System.Diagnostics.Debug.WriteLine("DataHolder Initialized");
        }

        public async Task SaveNewClient(Client client)
        {
            Clients.Add(client);
            await SaveToDisk();
            System.Diagnostics.Debug.WriteLine("New Client Saved");
        }

        public async Task ModifyClient(Client client)
        {
            Clients.Remove<Client>(x => x.Number == client.Number);
            await SaveNewClient(client);
            System.Diagnostics.Debug.WriteLine("Client Modified");
        }

        public async Task RemoveClient(Client client)
        {
            Clients.Remove<Client>(x => x.Number == client.Number);
            await SaveToDisk();
            System.Diagnostics.Debug.WriteLine("Client Removed");
        }

        private async Task SaveToDisk()
        {
            //Get the jsonFile and wtite the Client list to the disk
            var clientsFile = await StorageFolder.CreateFileAsync("Clients.json", CreationCollisionOption.OpenIfExists);
            var json = JsonConvert.SerializeObject(Clients);
            await FileIO.WriteTextAsync(clientsFile, json);
            System.Diagnostics.Debug.WriteLine("Clients Collection Saved To Disk");
        }

        private async Task InitProducts()
        {
            StorageFile sampleFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Json/Products.json"));
            string json = await FileIO.ReadTextAsync(sampleFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            Products = new ObservableCollection<Product>(JsonConvert.DeserializeObject<ObservableCollection<Product>>(json).OrderBy(e => e.Category).ThenBy(f => f.Name));
            System.Diagnostics.Debug.WriteLine("Products Initialized");
        }

        private async Task InitClients()
        {
            var clientsFile = await StorageFolder.CreateFileAsync("Clients.json", CreationCollisionOption.OpenIfExists);
            string json = await FileIO.ReadTextAsync(clientsFile);
            Clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(json);
            if (Clients == null)
                Clients = new ObservableCollection<Client>();
            System.Diagnostics.Debug.WriteLine("Clients Initialized");
        }
    }
}

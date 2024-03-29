﻿using AppCommandes.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppCommandes.MenuControls
{
    public sealed partial class Totaux : UserControl
    {
        public class Prod
        {
            public Product Product { get; set; }
            public int SlicedQuant { get; set; }
            public int Quant { get; set; }
            public string DisplayString
            {
                get
                {
                    if (Product.Category == "Pain")
                        return String.Format("{0} : {1} {2} dont {3} tranchés", Product.Category, Product.Name, Quant, SlicedQuant);
                    return String.Format("{0} {1} : {2}", Product.Category,Product.Name, Quant);
                }
            }
        }

        public ObservableCollection<Prod> observableCollection;
        DataHolder DataHolder;
        public Totaux()
        {
            this.InitializeComponent();
            this.Loaded += Totaux_Loaded;
        }

        private async void Totaux_Loaded(object sender, RoutedEventArgs e)
        {
            observableCollection = new ObservableCollection<Prod>();
            DataHolder = new DataHolder();
            await DataHolder.Init();
            foreach (var data in DataHolder.Products)
            {
                observableCollection.Add(new Prod() { Product = data, Quant = 0 , SlicedQuant = 0});
            }
            foreach (var client in DataHolder.Clients)
            {
                foreach (var pr in client.Products)
                {
                    //https://stackoverflow.com/questions/6781192/how-do-i-update-a-single-item-in-an-observablecollection-class
                    var prod = observableCollection.First(tmp => tmp.Product.Name == pr.Product.Name);
                    var idx = observableCollection.IndexOf(prod);
                    prod.Quant += pr.Quantity;
                    if (pr.Sliced)
                        prod.SlicedQuant += pr.Quantity;
                    observableCollection[idx] = prod;
                }
            }
            Total.ItemsSource = observableCollection;
        }

        //private async void PrintButton_Click(object sender, RoutedEventArgs e)
        //{
        //    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
        //    await renderTargetBitmap.RenderAsync(ImageHolder);
        //    var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
        //    var pixels = pixelBuffer.ToArray();
        //    var displayInformation = DisplayInformation.GetForCurrentView();
        //    var picker = new FileSavePicker();
        //    picker.FileTypeChoices.Add("JPEG Image", new string[] { ".jpg" });
        //    StorageFile file = await picker.PickSaveFileAsync();
        //    using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
        //    {
        //        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
        //        encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)renderTargetBitmap.PixelWidth, (uint)renderTargetBitmap.PixelHeight, displayInformation.RawDpiX, displayInformation.RawDpiY, pixels);
        //        await encoder.FlushAsync();
        //    }
        //}
    }
}

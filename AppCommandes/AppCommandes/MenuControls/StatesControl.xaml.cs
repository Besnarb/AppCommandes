using System;
using System.Collections.Generic;
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
    public sealed partial class StatesControl : UserControl
    {
        public StatesControl()
        {
            this.InitializeComponent();
            if (DateTime.Compare( DateTime.Now, new DateTime(2018, 12, 24, 0, 0, 0)) <= 0)
            {
                Noel.IsChecked = true;
                An.IsChecked = false;
            }
            else
            {
                Noel.IsChecked = false;
                An.IsChecked = true;
            }
        }

        public int Date
        {
            get
            {
                if ((bool)Noel.IsChecked)
                    return 24;
                return 31;
            }
        }

        public int Hour
        {
            get
            {
                if (picker.SelectedTime.HasValue)
                    return picker.SelectedTime.Value.Hours;
                return -1;
            }
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            if (((ToggleButton)sender) == An)
                Noel.IsChecked = !Noel.IsChecked;
            else
                An.IsChecked = !An.IsChecked;
        }
    }
}

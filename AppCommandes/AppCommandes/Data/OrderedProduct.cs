using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppCommandes.Data
{
    public class OrderedProduct :INotifyPropertyChanged
    {
        public Product Product { get; set; }
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        public string DisplayString
        {
            get
            {
                if (Slicable)
                    return string.Format("{0} {1} {2}", Product.Name, Quantity, Sliced ? "Tranché" :"Non tranché");
                return string.Format("{0} {1}", Product.Name, Quantity);
            }
        }
        public bool Sliced { get; set; }
        public bool Slicable { get; set; }
    }
}

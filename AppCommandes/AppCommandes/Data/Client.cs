using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommandes.Data
{
    class Client
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Remarks { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public ObservableCollection<OrderedProduct> Products { get; set; }

    }
}

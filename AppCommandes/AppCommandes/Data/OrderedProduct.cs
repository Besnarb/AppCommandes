using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommandes.Data
{
    class OrderedProduct
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public bool? Sliced { get; set; }
    }
}

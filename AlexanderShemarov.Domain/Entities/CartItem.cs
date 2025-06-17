using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexanderShemarov.Domain.Entities
{
    public class CartItem
    {
        public Trains trainItem {  get; set; }
        public int Qty { get; set; }
    }
}

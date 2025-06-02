using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningServiceApp.Models
{
    public class Customer
    {
        public int CustomerID { get; set; } // Egyedi azonosító
        public string Name { get; set; } // Ügyfél neve
        public List<ClothingItem> Orders { get; set; } = new List<ClothingItem>(); // Leadott ruhák listája

        public void AddOrder(ClothingItem item)
        {
            Orders.Add(item);
        }

        public void RemoveOrder(ClothingItem item)
        {
            Orders.Remove(item);
        }

        public override string ToString()
        {
            return $"{Name} - {Orders.Count} leadott ruhadarab";
        }

        internal void DropOff(ClothingItem item)
        {
            throw new NotImplementedException();
        }
    }
}

using CleaningServiceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CleaningServiceApp.Services
{
    public class CleaningService
    {
        public List<ClothingItem> ClothingItems { get; set; } = new List<ClothingItem>();
        public List<Customer> Customers { get; set; } = new List<Customer>();

        public void AddClothingItem(ClothingItem item)
        {
            ClothingItems.Add(item);
        }

        public void RemoveClothingItem(ClothingItem item)
        {   
            
           ClothingItems.Remove(item);
        }
        public ClothingItem FindItem(string itemID)
        {
            return ClothingItems.FirstOrDefault(i => i.ItemID == itemID);
        }

        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }
        public Customer FindCustomer (string name)
        {
            return Customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public bool DropOffItem(string customerName, ClothingItem item)
        {
            var customer = FindCustomer(customerName);
            if (customer != null)
            {
                customer.DropOff(item);
                ClothingItems.Add(item);
                return true;
            }
            return false;
        }

        public bool PickUpItem(string customerName, string itemID)
        {
            var customer = FindCustomer(customerName);
            if (customer != null)
            {
                var item = customer.Orders.FirstOrDefault(i => i.ItemID == itemID);
                if (item != null && item.IsCleaned)
                {
                    customer.RemoveOrder(item);
                    ClothingItems.Remove(item);
                    return true;
                }
            }
            return false;
        }
        public List<ClothingItem> ListUncleanedItems()
        {
            return ClothingItems.Where(x => !x.IsCleaned).ToList();
        }
        public List<Customer> ListCustomers()
        {
            return Customers;
        }

        public void SaveData(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var item in ClothingItems)
                {
                    writer.WriteLine($"{item.ItemID},{item.Type},{item.Material},{item.IsCleaned},{item.Price}");
                }
                foreach (var customer in Customers)
                {
                    writer.WriteLine($"{customer.CustomerID},{customer.Name}");
                    foreach (var order in customer.Orders)
                    {
                        writer.WriteLine($"  {order.ItemID}");
                    }
                }
            }
        }

        public void LoadData(CleaningService cleaningServiceManager, string filePath)
        {
            if (!File.Exists(filePath)) return;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 5) // Ruhadarab
                    {
                        var item = new ClothingItem
                        {
                            ItemID = parts[0],
                            Type = parts[1],
                            Material = parts[2],
                            IsCleaned = bool.Parse(parts[3]),
                            Price = decimal.Parse(parts[4])
                        };
                        ClothingItems.Add(item);
                    }
                    else if (parts.Length == 2) // Ügyfél
                    {
                        var customer = new Customer
                        {
                            CustomerID = int.Parse(parts[0]),
                            Name = parts[1]
                        };
                        Customers.Add(customer);
                    }
                    else if (parts.Length == 1 && parts[0].StartsWith("  ")) // Ruhadarab rendelés
                    {
                        var orderItemID = parts[0].Trim();
                        var orderItem = ClothingItems.FirstOrDefault(i => i.ItemID == orderItemID);
                        if (orderItem != null)
                        {
                            var lastCustomer = Customers.LastOrDefault();
                            lastCustomer?.AddOrder(orderItem);
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using CleaningServiceApp.Models;
using CleaningServiceApp.Services;

namespace CleaningServiceApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var CleaningServiceManager = new CleaningService();
            CleaningServiceManager.LoadData(CleaningServiceManager, "cleaning_service_data.txt");

            string choice = string.Empty;
            while (choice != "0")
            {
                Console.WriteLine("\n🧺 Ruhatisztító Ügyfél- és Megrendeléskezelő Rendszer\n");
                Console.WriteLine("1. Ruhák listázása");
                Console.WriteLine("2. Elérhető (nem tisztított) ruhák listázása");
                Console.WriteLine("3. Új ruhadarab felvétele");
                Console.WriteLine("4. Ruhadarab eltávolítása");
                Console.WriteLine("5. Ügyfelek listázása");
                Console.WriteLine("6. Új ügyfél hozzáadása");
                Console.WriteLine("7. Ruhadarab leadása tisztításra");
                Console.WriteLine("8. Ruhadarab átvétele");
                Console.WriteLine("9. Adatok mentése fájlba");
                Console.WriteLine("0. Kilépés");
                Console.Write("\nVálassz egy opciót: ");
                choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        // Ruhák listázása

                        Console.WriteLine("Ruhák listája:");
                       
                        foreach (var item in CleaningServiceManager.ClothingItems)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    case "2":
                        // Elérhető ruhák listázása
                        Console.WriteLine("Elérhető ruhák listája:");
                        foreach (var item in CleaningServiceManager.ClothingItems)
                        {
                            if (!item.IsCleaned)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        break;
                    case "3":
                        // Új ruhadarab felvétele
                        Console.WriteLine("Új ruhadarab felvétele:");
                        Console.Write("Adja meg a ruhadarab azonosítóját (pl. R001): ");
                        string itemId = Console.ReadLine();
                        Console.Write("Adja meg a ruhadarab típusát (pl. ing, nadrág): ");
                        string type = Console.ReadLine();
                        Console.Write("Adja meg az anyagát (pl. pamut, gyapjú): ");
                        string material = Console.ReadLine();
                        Console.Write("Adja meg az árát (pl. 1500): ");
                        decimal price;
                        while (!decimal.TryParse(Console.ReadLine(), out price) || price < 0)
                        {
                            Console.Write("Érvénytelen ár, próbáld újra: ");
                        }
                        var newItem = new ClothingItem
                        {
                            ItemID = itemId,
                            Type = type,
                            Material = material,
                            IsCleaned = false,
                            Price = price
                        };
                        CleaningServiceManager.AddClothingItem(newItem);
                        Console.WriteLine("Ruhadarab hozzáadva!");
                        break;
                    case "4":
                        // Ruhadarab eltávolítása
                        Console.Write("Adja meg a ruhadarab azonosítóját, amit el szeretne távolítani: ");
                        string removeItemId = Console.ReadLine();
                        var itemToRemove = CleaningServiceManager.FindItem(removeItemId);
                        if (itemToRemove != null)
                        {
                            CleaningServiceManager.RemoveClothingItem(itemToRemove);
                            Console.WriteLine("Ruhadarab eltávolítva!");
                        }
                        else
                        {
                            Console.WriteLine("Ruhadarab nem található!");
                        }
                        break;
                    case "5":
                        // Ügyfelek listázása
                        Console.WriteLine("Ügyfelek listája:");
                        foreach (var customer in CleaningServiceManager.Customers)
                        {
                            Console.WriteLine(customer);
                        }
                        break;
                    case "6":
                        // Új ügyfél hozzáadása
                        Console.Write("Adja meg az ügyfél nevét: ");
                        string customerName = Console.ReadLine();
                        var newCustomer = new Customer
                        {
                            Name = customerName
                        };
                        CleaningServiceManager.AddCustomer(newCustomer);
                        Console.WriteLine("Ügyfél hozzáadva!");

                        break;
                    case "7":
                        // Ruhadarab leadása
                        Console.Write("Adja meg az ügyfél nevét, aki leadja a ruhát: ");
                        string dropOffCustomerName = Console.ReadLine();
                        var dropOffCustomer = CleaningServiceManager.FindCustomer(dropOffCustomerName);
                        if (dropOffCustomer != null)
                        {
                            Console.Write("Adja meg a ruhadarab azonosítóját, amit lead: ");
                            string dropOffItemId = Console.ReadLine();
                            var itemToDropOff = CleaningServiceManager.FindItem(dropOffItemId);
                            if (itemToDropOff != null)
                            {
                                dropOffCustomer.DropOff(itemToDropOff);
                                CleaningServiceManager.AddClothingItem(itemToDropOff);
                                Console.WriteLine("Ruhadarab leadva!");
                            }
                            else
                            {
                                Console.WriteLine("Ruhadarab nem található!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ügyfél nem található!");
                        }

                        break;
                    case "8":
                        // Ruhadarab átvétele
                        Console.Write("Adja meg az ügyfél nevét, aki átveszi a ruhát: ");
                        string pickUpCustomerName = Console.ReadLine();
                        var pickUpCustomer = CleaningServiceManager.FindCustomer(pickUpCustomerName);
                        if (pickUpCustomer != null)
                        {
                            Console.Write("Adja meg a ruhadarab azonosítóját, amit átvesz: ");
                            string pickUpItemId = Console.ReadLine();
                            var itemToPickUp = CleaningServiceManager.FindItem(pickUpItemId);
                            if (itemToPickUp != null && itemToPickUp.IsCleaned)
                            {
                                pickUpCustomer.RemoveOrder(itemToPickUp);
                                CleaningServiceManager.RemoveClothingItem(itemToPickUp);
                                Console.WriteLine("Ruhadarab átadva!");
                            }
                            else
                            {
                                Console.WriteLine("Ruhadarab nem található vagy még nem tisztított!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ügyfél nem található!");
                        }
                        break;
                    case "9":
                        // Mentés fájlba
                        Console.WriteLine("Mentés folyamatban...");
                        CleaningServiceManager.SaveData("cleaning_service_data.txt");
                        Console.WriteLine("Mentés kész!");

                        break;
                    case "0":
                        // Kilépés
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás, próbáld újra!");
                        break;
                }
            }
        }
    }



 

  
}

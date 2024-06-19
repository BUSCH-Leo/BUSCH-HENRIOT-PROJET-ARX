﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ARX.model;

namespace ARX.view
{
    public partial class InventoryWindow : Window
    {
        public ObservableCollection<Item> InventoryItems { get; set; }
        public string Pseudo { get; set; }
        public int Vie { get; set; }
        public int VieMax { get; set; }
        public int Dexterite { get; set; }
        public int Force { get; set; }

        public InventoryWindow(Personnage joueur)
        {
            InitializeComponent();

            InventoryItems = new ObservableCollection<Item>();

            var settings = Settings.Load();
            Pseudo = settings.Pseudo;

            Vie = joueur.Vie;
            VieMax = joueur.VieMax;
            Dexterite = joueur.Dexterite;
            Force = joueur.Force;

            this.DataContext = new
            {
                Settings = settings,
                InventoryItems = InventoryItems,
                Vie = Vie,
                VieMax = VieMax,
                Dexterite = Dexterite,
                Force = Force,
                Personnage = joueur
            };
        }

        public void InitializeInventory()
        {
            InventoryItems.Add(new Item { Name = "Un suce", Price = 69, IsSelected = false });
            InventoryItems.Add(new Item { Name = "Potion de vie (+10)", Type = "Potion", Price = 50, EffectValue = 10, IsSelected = false });
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Hide();
            }
        }

        public void AddItem(Item newItem)
        {
            InventoryItems.Add(newItem);
        }

        public void RemoveItem(Item itemToRemove)
        {
            InventoryItems.Remove(itemToRemove);
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public int EffectValue { get; set; }
        public bool IsSelected { get; set; }
        public int Level { get; set; }
        public int Probabilite { get; set; }
        public int Multicritique { get; set; }
        public string Enchant { get; set; }

        public static void RandomItems(ObservableCollection<Item> inventoryItems)
        {
            Random random = new Random();
            int numItems = random.Next(1, 6);

            for (int i = 0; i < numItems; i++)
            {
                if (random.Next(0, 2) == 0)
                {
                    int potionValue = random.Next(5, 16);
                    Item potion = new Item
                    {
                        Name = $"Potion de vie (+{potionValue})",
                        Type = "Potion",
                        Price = 50,
                        EffectValue = potionValue,
                        IsSelected = false
                    };
                    inventoryItems.Add(potion);
                }
                else
                {
                    Arme arme = Arme.Randarme(100);
                    Item armeItem = new Item
                    {
                        Name = arme.Nom,
                        Type = "Arme",
                        Price = arme.Level * 10,
                        EffectValue = arme.DegatsMin,
                        Level = arme.Level,
                        Probabilite = arme.Probabilite,
                        Multicritique = arme.Multicritique,
                        Enchant = arme.Enchant,
                        IsSelected = false
                    };
                    inventoryItems.Add(armeItem);
                }
            }
        }

        public static ObservableCollection<Item> GenerateRandomItems(int numItems)
        {
            ObservableCollection<Item> items = new ObservableCollection<Item>();
            Random random = new Random();

            for (int i = 0; i < numItems; i++)
            {
                if (random.Next(0, 4) == 0) // 0 pour potion, 1 pour arme (exemple)
                {
                    int potionValue = random.Next(5, 16); // Valeur de soin entre 5 et 15
                    Item potion = new Item
                    {
                        Name = $"Potion de vie (+{potionValue})",
                        Type = "Potion",
                        Price = potionValue*5,
                        EffectValue = potionValue,
                        IsSelected = false
                    };
                    items.Add(potion);
                }
                else
                {
                    // Création d'une arme aléatoire
                    Arme arme = Arme.Randarme(10); // Difficulté par exemple
                    Item armeItem = new Item
                    {
                        Name = arme.Nom, // Nom de l'arme
                        Type = "Arme",
                        Price = arme.Level * 10, // Prix basé sur le niveau de l'arme (exemple)
                        EffectValue = arme.DegatsMin, // Valeur d'effet comme dégâts minimaux
                        Level = arme.Level,
                        Probabilite = arme.Probabilite,
                        Multicritique = arme.Multicritique,
                        Enchant = arme.Enchant,
                        IsSelected = false
                    };
                    items.Add(armeItem);
                }
            }

            return items;
        }
    }

}
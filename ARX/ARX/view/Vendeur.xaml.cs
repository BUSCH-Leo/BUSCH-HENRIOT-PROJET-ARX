using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ARX.model;

namespace ARX.view
{
    public partial class Vendeur : Window
    {
        public ObservableCollection<Item> InventoryItems { get; set; }
        public ObservableCollection<Item> ShopItems { get; set; }

        public Vendeur()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/ARX.ico"));

            InventoryItems = new ObservableCollection<Item>
            {
                new Item { Name = "Objet", Price = 10, IsSelected = false },
                new Item { Name = "Objet_2", Price = 5, IsSelected = false }
            };

            ShopItems = new ObservableCollection<Item>
            {
                new Item { Name = "Objet_3", Price = 15, IsSelected = false },
                new Item { Name = "Objet_4", Price = 20, IsSelected = false },
                new Item { Name = "Cramptés", Price = 50, IsSelected = false }
            };

            InventoryList.ItemsSource = InventoryItems;
            ShopList.ItemsSource = ShopItems;
        }

        private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateTotalPrices();
        }

        private void UpdateTotalPrices()
        {
            double totalAchat = ShopItems.Where(item => item.IsSelected).Sum(item => item.Price);
            double totalVente = InventoryItems.Where(item => item.IsSelected).Sum(item => item.Price);

            TotalBuyPrice.Text = totalAchat.ToString("F2");
            TotalSellPrice.Text = totalVente.ToString("F2");
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour la vente d'objets sélectionnés dans l'inventaire
            var itemsToSell = InventoryItems.Where(item => item.IsSelected).ToList();
            foreach (var item in itemsToSell)
            {
                InventoryItems.Remove(item);
            }
            UpdateTotalPrices();
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour l'achat d'objets sélectionnés dans le magasin
            var itemsToBuy = ShopItems.Where(item => item.IsSelected).ToList();
            foreach (var item in itemsToBuy)
            {
                ShopItems.Remove(item);
            }
            UpdateTotalPrices();
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsSelected { get; set; }
    }
}

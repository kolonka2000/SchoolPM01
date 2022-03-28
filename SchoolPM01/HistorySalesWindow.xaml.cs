using System;
using System.Collections.Generic;
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

namespace SchoolPM01
{
    /// <summary>
    /// Логика взаимодействия для HistorySalesWindow.xaml
    /// </summary>
    public partial class HistorySalesWindow : Window
    {
        public HistorySalesWindow(Product SelectedProduct)
        {
            InitializeComponent();

            var currentSale = SchoolPM01Entities.GetContext().ProductSale.ToList();
            var comboSales = SchoolPM01Entities.GetContext().Product.ToList();

            DGrid_Sales.ItemsSource = currentSale.OrderByDescending(p => p.SaleDate).ToList();

            comboSales.Insert(0, new Product
            {
                Title = "Все товары"
            });
            ComboSale.ItemsSource = comboSales;
            if (SelectedProduct != null)
            {
                currentSale = currentSale.Where(p => p.ProductID == SelectedProduct.ID).ToList();
                ComboSale.SelectedIndex = SelectedProduct.ID;
            }
            else
            {
                ComboSale.SelectedIndex = 0;
            }
            DGrid_Sales.ItemsSource = currentSale.OrderByDescending(p => p.SaleDate).ToList();
        }

        private void ComboSale_Changed(object sender, SelectionChangedEventArgs e)
        {
            var currentSale = SchoolPM01Entities.GetContext().ProductSale.ToList();
            if (ComboSale.SelectedIndex == 0)
            {
                currentSale = currentSale.ToList();
            }
            else
            {
                currentSale = currentSale.Where(p => p.Product.Title == (ComboSale.SelectedItem as Product).Title).ToList();
            }
            DGrid_Sales.ItemsSource = currentSale;
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchoolPM01
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();

            var allManufacturers = SchoolPM01Entities.GetContext().Manufacturer.ToList();
            allManufacturers.Insert(0, new Manufacturer { Name = "Все производители" });
            ComboManufacturer.ItemsSource = allManufacturers;

            CheckActual.IsChecked = true;
            ComboManufacturer.SelectedIndex = 0;
            ComboCost.SelectedIndex = 0;

            UpdateProducts();
        }
        public void UpdateProducts()
        {
            var currentProduct = SchoolPM01Entities.GetContext().Product.ToList();

            if (ComboManufacturer.SelectedIndex > 0)
                currentProduct = currentProduct.Where(p => p.Manufacturer.Name.Contains((ComboManufacturer.SelectedItem as Manufacturer).Name)).ToList();

            currentProduct = currentProduct.Where(p => p.Title.Contains(TBox_Search.Text.ToLower())).ToList();

            if (CheckActual.IsChecked.Value)
                currentProduct = currentProduct.Where(p => p.IsActive).ToList();

            switch (ComboCost.SelectedIndex)
            {
                case 0:
                    LViewProd.ItemsSource = currentProduct.ToList();
                    break;
                case 1:
                    LViewProd.ItemsSource = currentProduct.OrderBy(p => p.Cost).ToList();
                    break;
                case 2:
                    LViewProd.ItemsSource = currentProduct.OrderByDescending(p => p.Cost).ToList();
                    break;
                default:
                    LViewProd.ItemsSource = currentProduct.ToList();
                    break;
            }

            var allCount = SchoolPM01Entities.GetContext().Product.Count();
            LabCount.Content = "Показано " + currentProduct.Count() + " из " + allCount.ToString() + " товаров";

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }


        private void BtnHistorySales_Click(object sender, RoutedEventArgs e)
        {
            HistorySalesWindow HSW = new HistorySalesWindow(LViewProd.SelectedItem as Product);
            HSW.ShowDialog();
        }

        private void LViewProd_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(LViewProd.SelectedItem as Product));
        }


        private void TBox_Search_Changed(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void ComboManufacturer_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void ComboCost_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void Actual_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void LViewProd_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                SchoolPM01Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                LViewProd.ItemsSource = SchoolPM01Entities.GetContext().Product.ToList();
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var proddel = LViewProd.SelectedItems.Cast<Product>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {proddel.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    SchoolPM01Entities.GetContext().Product.RemoveRange(proddel);
                    SchoolPM01Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    LViewProd.ItemsSource = SchoolPM01Entities.GetContext().Product.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}

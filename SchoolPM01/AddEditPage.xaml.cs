using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        public string Fill;
        private Product _currentProduct = new Product();
        public AddEditPage(Product SelectedProduct)
        {
            InitializeComponent();

            if (SelectedProduct != null)
                _currentProduct = SelectedProduct;

            DataContext = _currentProduct;

            ComboManufacturer.ItemsSource = SchoolPM01Entities.GetContext().Manufacturer.ToList();
        }

        private void BtnImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image Files(*.JPG;*.PNG;.*JPEG)|*.JPG;*.PNG;.*JPEG|All files (*.*)|*.*";
            if (openDialog.ShowDialog() == true)
            {
                Img.Source = new BitmapImage(new Uri(openDialog.FileName));
                Fill = openDialog.FileName.ToString();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_currentProduct.ID == 0)
            {
                _currentProduct.MainImagePath = Fill;
                SchoolPM01Entities.GetContext().Product.Add(_currentProduct);
            }

            try
            {
                _currentProduct.MainImagePath = Fill;
                SchoolPM01Entities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}

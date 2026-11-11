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
using System.Data.Entity;//

namespace Sinuyshina41_3
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private User _currentUser;//
        public ProductPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            if(_currentUser!=null)
            {
                FIOTB.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                switch (user.IDRole)
                {
                    case 1:
                        RoleTB.Text = "Клиент"; break;
                    case 2:
                        RoleTB.Text = "Менеджер"; break;
                    case 3:
                        RoleTB.Text = "Администратор"; break;
                }
            }
            else
            {
                X.Visibility = Visibility.Collapsed;
                Y.Visibility = Visibility.Collapsed;
            }




                var currentProduct = Sinuyshina41Entities.GetContext().Product.ToList();//Include(s => s.Manufacturer)

            ProductListView.ItemsSource = currentProduct;

            ComboType.SelectedIndex = 0;
            TBAllRecords.Text = $" из {currentProduct.Count}";

            UpdateProduct();
        }

        private void UpdateProduct()
        {
            var currentProduct = Sinuyshina41Entities.GetContext().Product.ToList();

            if(ComboType.SelectedIndex==0)
            {
                currentProduct = currentProduct.Where(p => (p.ProductDiscountAmount==null||p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 100)).ToList();

            }
            if(ComboType.SelectedIndex==1)
            {
                currentProduct = currentProduct.Where(p => (p.ProductDiscountAmount == null || p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentProduct = currentProduct.Where(p => (p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentProduct = currentProduct.Where(p => (p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount <= 100)).ToList();
            }

            currentProduct = currentProduct.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            
            if(RButtonDown.IsChecked.Value)
            {
                currentProduct = currentProduct.OrderByDescending(p => p.ProductCost).ToList();

            }
            if(RButtonUp.IsChecked.Value)
            {
                currentProduct = currentProduct.OrderBy(p => p.ProductCost).ToList();
            }
            ProductListView.ItemsSource = currentProduct;
            
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }

        private void RbuttonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }

    }

}

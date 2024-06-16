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

namespace OrderMenagmentApp_2
{
    /// <summary>
    /// Логика взаимодействия для ReceptionistWindow.xaml
    /// </summary>
    public partial class ReceptionistWindow : Window
    {
        public ReceptionistWindow()
        {
            InitializeComponent();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.NavigationService.Navigate(new OrdersPage());
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.NavigationService.Navigate(new CustomersPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}

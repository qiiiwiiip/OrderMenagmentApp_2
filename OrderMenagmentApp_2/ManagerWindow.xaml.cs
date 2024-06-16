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

    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.NavigationService.Navigate(new OrdersPage());
        }

        private void ServicesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.NavigationService.Navigate(new ServicesPage());
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.NavigationService.Navigate(new CustomersPage());
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.NavigationService.Navigate(new EmployeesPage());
        }
        private void JobTitlesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.NavigationService.Navigate(new JobTitlesPage());
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.NavigationService.Navigate(new UsersPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
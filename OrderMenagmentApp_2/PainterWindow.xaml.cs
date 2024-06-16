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
    public partial class PainterWindow : Window
    {
        public PainterWindow()
        {
            InitializeComponent();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            // Загрузка страницы "OrdersPage.xaml" во фрейм ContentFrame
            ContentFrame.NavigationService.Navigate(new PaintOrdersPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Выход из приложения или переход на страницу входа (в зависимости от вашей логики)
            // Например:
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}
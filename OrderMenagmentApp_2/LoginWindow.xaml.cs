using OrderMenagmentApp_2;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace OrderMenagmentApp_2
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = PasswordBox.Password;
            PasswordBox.Visibility = Visibility.Hidden;
            PasswordTextBox.Visibility = Visibility.Visible;
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = PasswordTextBox.Text;
            PasswordBox.Visibility = Visibility.Visible;
            PasswordTextBox.Visibility = Visibility.Hidden;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = ShowPasswordCheckBox.IsChecked == true ? PasswordTextBox.Text : PasswordBox.Password;

            // Подключение к базе данных
            string connectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Поиск пользователя по логину и паролю
                string query = "SELECT RoleID FROM Users WHERE Login = @Login AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    int? roleId = (int?)command.ExecuteScalar();

                    if (roleId != null)
                    {
                        // В зависимости от роли пользователя открываем соответствующее окно
                        switch (roleId)
                        {
                            case 1:
                                ManagerWindow managerWindow = new ManagerWindow();
                                managerWindow.Show();
                                break;
                            case 2:
                                PainterWindow painterWindow = new PainterWindow();
                                painterWindow.Show();
                                break;
                            case 3:
                                ReceptionistWindow receptionistWindow = new ReceptionistWindow();
                                receptionistWindow.Show();
                                break;
                        }
                        // Закрываем текущее окно входа
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль");
                    }
                }
            }
        }
    }
}
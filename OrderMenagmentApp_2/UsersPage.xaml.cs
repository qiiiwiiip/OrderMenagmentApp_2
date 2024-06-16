using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        private const string ConnectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";

        public UsersPage()
        {
            InitializeComponent();
            DisplayDataInGrid();
            LoadJobTitles(); // Загрузка списка 
        }

        private DataTable GetDataFromDatabase(string query)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                return dataTable;
            }
        }

        private void DisplayDataInGrid()
        {
            string query = "SELECT Users.*, Roles.Role FROM Users INNER JOIN Roles ON Users.RoleID = Roles.RoleID";

            DataTable dataTable = GetDataFromDatabase(query);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }


        private void LoadJobTitles()
        {
            string query = "SELECT * FROM Roles";
            DataTable dataTable = GetDataFromDatabase(query);

            roleComboBox.ItemsSource = dataTable.DefaultView;
            roleComboBox.DisplayMemberPath = "Role";
            roleComboBox.SelectedValuePath = "RoleID";
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                loginTextBox.Text = selectedRow["Login"].ToString();
                passwordTextBox.Text = selectedRow["Password"].ToString();

                // Установка выбранного значения в ComboBox
                int roleID = Convert.ToInt32(selectedRow["RoleID"]);
                roleComboBox.SelectedValue = roleID;
            }
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (roleComboBox.SelectedValue != null)
            {
                string login = loginTextBox.Text;
                string password = passwordTextBox.Text;

                int roleId = (int)roleComboBox.SelectedValue;

                string query = $"INSERT INTO Users (Login, Password, RoleID) VALUES ('{login}', '{password}','{roleId}')";

                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }

        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null && roleComboBox.SelectedValue != null)
            {
                int userId = (int)selectedRow["UserID"];
                string login = loginTextBox.Text;
                string password = passwordTextBox.Text;
                int roleId = (int)roleComboBox.SelectedValue;

                string query = $"UPDATE Users SET Login='{login}', Password='{password}', RoleID={roleId} WHERE UserID={userId}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }

        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int userId = (int)selectedRow["UserID"];
                string query = $"DELETE FROM Users WHERE UserID={userId}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }

    }
}
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
using System.Windows.Controls.Primitives;



namespace OrderMenagmentApp_2
{
    /// <summary>
    /// Логика взаимодействия для CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        private const string ConnectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";

        public CustomersPage()
        {
            InitializeComponent();
            DisplayDataInGrid();
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
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                firstNameTextBox.Text = selectedRow["FirstName"].ToString();
                lastNameTextBox.Text = selectedRow["LastName"].ToString();
                patronymicTextBox.Text = selectedRow["Patronymic"].ToString();
                phoneNumberTextBox.Text = selectedRow["PhoneNumber"].ToString();
            }
        }

        private void DisplayDataInGrid()
        {
            string query = "SELECT * FROM Customers";
            DataTable dataTable = GetDataFromDatabase(query);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                int customerId = (int)selectedRow["CustomerID"];

                CustomerDetailsWindow detailsWindow = new CustomerDetailsWindow(customerId);
                detailsWindow.Show();
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string patronymic = patronymicTextBox.Text;
            string phoneNumber = phoneNumberTextBox.Text;

            string query = $"INSERT INTO Customers (FirstName, LastName, Patronymic, PhoneNumber) VALUES ('{firstName}', '{lastName}', '{patronymic}', '{phoneNumber}')";
            GetDataFromDatabase(query);
            DisplayDataInGrid();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int сustomerID = (int)selectedRow["CustomerID"];
                string firstName = firstNameTextBox.Text;
                string lastName = lastNameTextBox.Text;
                string patronymic = patronymicTextBox.Text;
                string phoneNumber = phoneNumberTextBox.Text;

                string query = $"UPDATE Customers SET FirstName='{firstName}', LastName='{lastName}', Patronymic='{patronymic}', PhoneNumber='{phoneNumber}' WHERE CustomerID={сustomerID}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int сustomerID = (int)selectedRow["CustomerID"];
                string query = $"DELETE FROM Customers WHERE CustomerID={сustomerID}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }
    }
}
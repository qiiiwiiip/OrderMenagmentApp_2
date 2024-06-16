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
using System.Configuration;

namespace OrderMenagmentApp_2
{

    public partial class OrdersPage : Page
    {
        private const string ConnectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";

        public OrdersPage()
        {
            InitializeComponent();
            LoadComboBoxData();
            DisplayDataInGrid();
            dataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
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

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;

                string description = selectedRow["Description"].ToString();
                string startDate = ((DateTime)selectedRow["StartDate"]).ToString("yyyy-MM-dd");
                string endDate = selectedRow["EndDate"] != DBNull.Value ? ((DateTime)selectedRow["EndDate"]).ToString("yyyy-MM-dd") : string.Empty;
                string customer = selectedRow["CustomerFirstName"].ToString() + " " + selectedRow["CustomerLastName"].ToString();
                string employee = selectedRow["EmployeeFirstName"].ToString() + " " + selectedRow["EmployeeLastName"].ToString();
                string service = selectedRow["ServiceName"].ToString();
                string status = selectedRow["StatusName"].ToString();

                OrderDetailsWindow orderDetailsWindow = new OrderDetailsWindow();
                orderDetailsWindow.SetOrderDetails(description, startDate, endDate, customer, employee, service, status);
                orderDetailsWindow.Show();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                descriptionTextBox.Text = selectedRow["Description"].ToString();
                startDateDatePicker.SelectedDate = (DateTime)selectedRow["StartDate"];
                endDateDatePicker.SelectedDate = selectedRow["EndDate"] as DateTime?;

                // Установка выбранных значений в ComboBox'ы
                int customerID = Convert.ToInt32(selectedRow["CustomerID"]);
                customerComboBox.SelectedValue = customerID;

                int employeeID = Convert.ToInt32(selectedRow["EmployeeID"]);
                employeeComboBox.SelectedValue = employeeID;

                int serviceID = Convert.ToInt32(selectedRow["ServiceID"]);
                serviceComboBox.SelectedValue = serviceID;

                int statusID = Convert.ToInt32(selectedRow["StatusID"]);
                statusComboBox.SelectedValue = statusID;
            }
        }

        private void DisplayDataInGrid(string searchTerm = "")
        {
            string query = "SELECT Orders.*, " +
                           "Customers.FirstName AS CustomerFirstName, " +
                           "Customers.LastName AS CustomerLastName, " +
                           "Employees.FirstName AS EmployeeFirstName, " +
                           "Employees.LastName AS EmployeeLastName, " +
                           "Services.Name AS ServiceName, " +
                           "Statuses.Status AS StatusName " +
                           "FROM Orders " +
                           "INNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID " +
                           "INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID " +
                           "INNER JOIN Services ON Orders.ServiceID = Services.ServiceID " +
                           "INNER JOIN Statuses ON Orders.StatusID = Statuses.StatusID";

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += $" WHERE Orders.Description LIKE '%{searchTerm}%' " +
                         $"OR Customers.FirstName LIKE '%{searchTerm}%' " +
                         $"OR Customers.LastName LIKE '%{searchTerm}%' " +
                         $"OR Employees.FirstName LIKE '%{searchTerm}%' " +
                         $"OR Employees.LastName LIKE '%{searchTerm}%' " +
                         $"OR Services.Name LIKE '%{searchTerm}%' " +
                         $"OR Statuses.Status LIKE '%{searchTerm}%'";
            }

            DataTable dataTable = GetDataFromDatabase(query);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTextBox.Text;
            DisplayDataInGrid(searchTerm);
        }

        private void LoadComboBoxData()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Load Customers
                SqlDataAdapter customerAdapter = new SqlDataAdapter("SELECT CustomerID, FirstName + ' ' + LastName AS Name FROM Customers", connection);
                DataTable customerTable = new DataTable();
                customerAdapter.Fill(customerTable);
                customerComboBox.ItemsSource = customerTable.DefaultView;
                customerComboBox.DisplayMemberPath = "Name";
                customerComboBox.SelectedValuePath = "CustomerID";

                // Load Employees
                SqlDataAdapter employeeAdapter = new SqlDataAdapter("SELECT EmployeeID, FirstName + ' ' + LastName AS Name FROM Employees", connection);
                DataTable employeeTable = new DataTable();
                employeeAdapter.Fill(employeeTable);
                employeeComboBox.ItemsSource = employeeTable.DefaultView;
                employeeComboBox.DisplayMemberPath = "Name";
                employeeComboBox.SelectedValuePath = "EmployeeID";

                // Load Services
                SqlDataAdapter serviceAdapter = new SqlDataAdapter("SELECT ServiceID, Name FROM Services", connection);
                DataTable serviceTable = new DataTable();
                serviceAdapter.Fill(serviceTable);
                serviceComboBox.ItemsSource = serviceTable.DefaultView;
                serviceComboBox.DisplayMemberPath = "Name";
                serviceComboBox.SelectedValuePath = "ServiceID";

                // Load Statuses
                SqlDataAdapter statusAdapter = new SqlDataAdapter("SELECT StatusID, Status FROM Statuses", connection);
                DataTable statusTable = new DataTable();
                statusAdapter.Fill(statusTable);
                statusComboBox.ItemsSource = statusTable.DefaultView;
                statusComboBox.DisplayMemberPath = "Status";
                statusComboBox.SelectedValuePath = "StatusID";

                // Set default value for statusComboBox to "создан" (StatusID = 1)
                statusComboBox.SelectedValue = 1;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (customerComboBox.SelectedValue != null && employeeComboBox.SelectedValue != null && serviceComboBox.SelectedValue != null && statusComboBox.SelectedValue != null)
            {
                string description = descriptionTextBox.Text;
                DateTime startDate = startDateDatePicker.SelectedDate ?? DateTime.Now;
                int customerID = (int)customerComboBox.SelectedValue;
                int employeeID = (int)employeeComboBox.SelectedValue;
                int serviceID = (int)serviceComboBox.SelectedValue;
                int statusID = 1;

                string query = $"INSERT INTO Orders (Description, StartDate,  CustomerID, EmployeeID, ServiceID, StatusID) VALUES ('{description}', '{startDate:yyyy-MM-dd}',  {customerID}, {employeeID}, {serviceID}, '{statusID}')";

                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null && customerComboBox.SelectedValue != null && employeeComboBox.SelectedValue != null && serviceComboBox.SelectedValue != null && statusComboBox.SelectedValue != null)
            {
                int orderID = (int)selectedRow["OrderID"];
                string description = descriptionTextBox.Text;
                DateTime startDate = startDateDatePicker.SelectedDate ?? DateTime.Now;
                int customerID = (int)customerComboBox.SelectedValue;
                int employeeID = (int)employeeComboBox.SelectedValue;
                int serviceID = (int)serviceComboBox.SelectedValue;
                int statusID = (int)statusComboBox.SelectedValue;

                string query;
                if (statusID == 4) 
                {
                    query = $"UPDATE Orders SET Description='{description}', StartDate='{startDate:yyyy-MM-dd}', EndDate='{DateTime.Now:yyyy-MM-dd}', customerID={customerID}, EmployeeID={employeeID}, ServiceID={serviceID}, StatusID='{statusID}' WHERE OrderID={orderID}";
                }
                else
                {
                    query = $"UPDATE Orders SET Description='{description}', StartDate='{startDate:yyyy-MM-dd}',  customerID={customerID}, EmployeeID={employeeID}, ServiceID={serviceID}, StatusID='{statusID}' WHERE OrderID={orderID}";
                }

                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int orderID = (int)selectedRow["OrderID"];
                string query = $"DELETE FROM Orders WHERE OrderID={orderID}";

                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Очистка полей ввода
            descriptionTextBox.Clear();
            startDateDatePicker.SelectedDate = null;
            endDateDatePicker.SelectedDate = null;
            customerComboBox.SelectedIndex = -1;
            employeeComboBox.SelectedIndex = -1;
            serviceComboBox.SelectedIndex = -1;
            statusComboBox.SelectedValue = 1; // Устанавливаем статус "создан" по умолчанию
        }
    }
}
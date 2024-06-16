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
    /// Логика взаимодействия для PaintOrdersPage.xaml
    /// </summary>
    public partial class PaintOrdersPage : Page
    {
        private const string ConnectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";

        public PaintOrdersPage()
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
                startDateDatePicker.SelectedDate = (DateTime)selectedRow["StartDate"];
                endDateDatePicker.SelectedDate = selectedRow["EndDate"] as DateTime?;

                // Установка выбранных значений в ComboBox'ы
                int statusID = Convert.ToInt32(selectedRow["StatusID"]);
                statusComboBox.SelectedValue = statusID;
            }
        }

        private void DisplayDataInGrid()
        {
            string query = "SELECT Orders.*, \r\n\r\nCustomers.FirstName AS CustomerFirstName, \r\n\r\nCustomers.LastName AS CustomerLastName, \r\n\r\nEmployees.FirstName AS EmployeeFirstName, \r\n\r\nEmployees.LastName AS EmployeeLastName, \r\n\r\nServices.Name AS ServiceName,\r\n\r\nStatuses.Status AS StatusName FROM Orders\r\n\r\n\r\n\r\nINNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID \r\n\r\nINNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID \r\n\r\nINNER JOIN Services ON Orders.ServiceID = Services.ServiceID\r\n\r\nINNER JOIN Statuses ON Orders.StatusID = Statuses.StatusID";
            DataTable dataTable = GetDataFromDatabase(query);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void LoadComboBoxData()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                // Load Statuses
                SqlDataAdapter statusAdapter = new SqlDataAdapter("SELECT StatusID, Status FROM Statuses", conn);
                DataTable statusTable = new DataTable();
                statusAdapter.Fill(statusTable);
                statusComboBox.ItemsSource = statusTable.DefaultView;
                statusComboBox.DisplayMemberPath = "Status";
                statusComboBox.SelectedValuePath = "StatusID";

                // Set default value for statusComboBox to "создан" (StatusID = 1)
                statusComboBox.SelectedValue = 2;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null && statusComboBox.SelectedValue != null)
            {
                int orderID = (int)selectedRow["OrderID"];
                //string description = descriptionTextBox.Text;
                DateTime startDate = startDateDatePicker.SelectedDate ?? DateTime.Now;
                DateTime? endDate = endDateDatePicker.SelectedDate;
                //int customerID = (int)customerComboBox.SelectedValue;
                //int employeeID = (int)employeeComboBox.SelectedValue;
                //int serviceID = (int)serviceComboBox.SelectedValue;
                int statusID = (int)statusComboBox.SelectedValue;

                // Проверка статуса "выдан"
                string query;
                if (statusID == 5) // Предположим, что статус "выдан" имеет StatusID равный 2
                {
                    query = $"UPDATE Orders SET  StartDate='{startDate:yyyy-MM-dd}', EndDate='{DateTime.Now:yyyy-MM-dd}', StatusID='{statusID}' WHERE OrderID={orderID}";
                }
                else
                {
                    query = $"UPDATE Orders SET  StartDate='{startDate:yyyy-MM-dd}', EndDate='{endDate}', StatusID='{statusID}' WHERE OrderID={orderID}";
                }

                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }
    }
}
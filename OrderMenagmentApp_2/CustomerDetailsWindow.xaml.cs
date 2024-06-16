using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace OrderMenagmentApp_2
{
    public partial class CustomerDetailsWindow : Window
    {
        private const string ConnectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";
        private int customerId;

        public CustomerDetailsWindow(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            LoadCustomerDetails();
            LoadOrderHistory();
        }

        private void LoadCustomerDetails()
        {
            string query = $"SELECT * FROM Customers WHERE CustomerID = {customerId}";

            DataTable customerTable = GetDataFromDatabase(query);
            if (customerTable.Rows.Count > 0)
            {
                DataRow row = customerTable.Rows[0];
                customerInfoTextBlock.Text = $"ФИО клиента: {row["FirstName"]} {row["LastName"]} {row["Patronymic"]}\n" +
                                             $"Номер телефона: {row["PhoneNumber"]}";
            }
        }

        private void LoadOrderHistory()
        {
            string query = $"SELECT o.OrderID AS [Код], s.Name AS [Услуга], o.Description AS [Описание], " +
                           $"o.StartDate AS [Дата регистрации], o.EndDate AS [Дата выдачи], st.Status AS [Статус] " +
                           $"FROM Orders o " +
                           $"JOIN Services s ON o.ServiceID = s.ServiceID " +
                           $"JOIN Statuses st ON o.StatusID = st.StatusID " +
                           $"WHERE o.CustomerID = {customerId}";

            DataTable ordersTable = GetDataFromDatabase(query);
            ordersDataGrid.ItemsSource = ordersTable.DefaultView;
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
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace OrderMenagmentApp_2
{
    public partial class EmployeeDetailsWindow : Window
    {
        private const string ConnectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";
        private int employeeId;

        public EmployeeDetailsWindow(int employeeId)
        {
            InitializeComponent();
            this.employeeId = employeeId;
            LoadEmployeeDetails();
            LoadOrderHistory();
        }

        private void LoadEmployeeDetails()
        {
            string query = $"SELECT e.FirstName, e.LastName, e.Patronymic, e.PhoneNumber, j.JobTitles " +
                           $"FROM Employees e " +
                           $"JOIN JobTitles j ON e.JobTitleID = j.JobTitleID " +
                           $"WHERE e.EmployeeID = {employeeId}";

            DataTable employeeTable = GetDataFromDatabase(query);
            if (employeeTable.Rows.Count > 0)
            {
                DataRow row = employeeTable.Rows[0];
                employeeInfoTextBlock.Text = $"ФИО сотрудника: {row["FirstName"]} {row["LastName"]} {row["Patronymic"]}\n" +
                                             $"Номер телефона: {row["PhoneNumber"]}\n" +
                                             $"Должность: {row["JobTitles"]}";
            }
        }

        private void LoadOrderHistory()
        {
            string query = $"SELECT o.OrderID AS [Код], s.Name AS [Услуга], o.Description AS [Описание], " +
                           $"o.StartDate AS [Дата регистрации], o.EndDate AS [Дата выдачи], st.Status AS [Статус] " +
                           $"FROM Orders o " +
                           $"JOIN Services s ON o.ServiceID = s.ServiceID " +
                           $"JOIN Statuses st ON o.StatusID = st.StatusID " +
                           $"WHERE o.EmployeeID = {employeeId}";

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

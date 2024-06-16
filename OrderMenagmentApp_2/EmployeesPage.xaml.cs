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

    public partial class EmployeesPage : Page
    {
        private const string ConnectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";

        public EmployeesPage()
        {
            InitializeComponent();
            DisplayDataInGrid();
            LoadJobTitles(); // Загрузка списка должностей
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
            string query = "SELECT Employees.*, JobTitles.JobTitles FROM Employees INNER JOIN JobTitles ON Employees.JobTitleID = JobTitles.JobTitleID";
            DataTable dataTable = GetDataFromDatabase(query);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void LoadJobTitles()
        {
            string query = "SELECT * FROM JobTitles";
            DataTable dataTable = GetDataFromDatabase(query);

            jobTitleComboBox.ItemsSource = dataTable.DefaultView;
            jobTitleComboBox.DisplayMemberPath = "JobTitles";
            jobTitleComboBox.SelectedValuePath = "JobTitleID";
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

                // Установка выбранного значения в ComboBox
                int jobTitleID = Convert.ToInt32(selectedRow["JobTitleID"]);
                jobTitleComboBox.SelectedValue = jobTitleID;
            }
        }

        private void EmployeeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                int employeeId = (int)selectedRow["EmployeeID"];

                EmployeeDetailsWindow detailsWindow = new EmployeeDetailsWindow(employeeId);
                detailsWindow.Show();
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (jobTitleComboBox.SelectedValue != null)
            {
                string firstName = firstNameTextBox.Text;
                string lastName = lastNameTextBox.Text;
                string patronymic = patronymicTextBox.Text;
                string phoneNumber = phoneNumberTextBox.Text;
                int jobTitleId = (int)jobTitleComboBox.SelectedValue;

                string query = $"INSERT INTO Employees (FirstName, LastName, Patronymic, PhoneNumber, JobTitleID) VALUES ('{firstName}', '{lastName}', '{patronymic}', '{phoneNumber}', '{jobTitleId}')";

                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }

        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null && jobTitleComboBox.SelectedValue != null)
            {
                int employeeId = (int)selectedRow["EmployeeID"];
                string firstName = firstNameTextBox.Text;
                string lastName = lastNameTextBox.Text;
                string patronymic = patronymicTextBox.Text;
                string phoneNumber = phoneNumberTextBox.Text;
                int jobTitleId = (int)jobTitleComboBox.SelectedValue;

                string query = $"UPDATE Employees SET FirstName='{firstName}', LastName='{lastName}', Patronymic='{patronymic}', PhoneNumber='{phoneNumber}', JobTitleID={jobTitleId} WHERE EmployeeID={employeeId}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }

        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int employeeId = (int)selectedRow["EmployeeID"];
                string query = $"DELETE FROM Employees WHERE EmployeeID={employeeId}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }
    }
}
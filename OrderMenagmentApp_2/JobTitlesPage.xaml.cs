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

    public partial class JobTitlesPage : Page
    {
        private const string ConnectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";

        public JobTitlesPage()
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

        private void DisplayDataInGrid()
        {
            string query = "SELECT * FROM JobTitles";
            DataTable dataTable = GetDataFromDatabase(query);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                jobTitleTextBox.Text = selectedRow["JobTitles"].ToString();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string jobTitle = jobTitleTextBox.Text;

            string query = $"INSERT INTO JobTitles (JobTitles) VALUES ('{jobTitle}')";
            GetDataFromDatabase(query);
            DisplayDataInGrid();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int jobTitleID = (int)selectedRow["JobTitleID"];
                string jobTitle = jobTitleTextBox.Text;

                string query = $"UPDATE JobTitles SET JobTitles='{jobTitle}' WHERE JobTitleID={jobTitleID}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int jobTitleID = (int)selectedRow["JobTitleID"];
                string query = $"DELETE FROM JobTitles WHERE JobTitleID={jobTitleID}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }
    }
}
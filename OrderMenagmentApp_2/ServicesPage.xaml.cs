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
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private const string ConnectionString = @"Data Source=MYPC;Initial Catalog=OrderManagmentDB_3;Integrated Security=True";


        public ServicesPage()
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
            string query = "SELECT * FROM Services";
            DataTable dataTable = GetDataFromDatabase(query);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                serviceNameTextBox.Text = selectedRow["Name"].ToString();
                servicePriceTextBox.Text = selectedRow["Price"].ToString();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = serviceNameTextBox.Text;
            string price = servicePriceTextBox.Text;

            string query = $"INSERT INTO Services (Name, Price) VALUES ('{name}', '{price}')";
            GetDataFromDatabase(query);
            DisplayDataInGrid();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int servicesID = (int)selectedRow["ServiceID"];
                string name = serviceNameTextBox.Text;
                string price = servicePriceTextBox.Text;

                string query = $"UPDATE Services SET name='{name}', price='{price}' WHERE ServiceID={servicesID}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                int servicesID = (int)selectedRow["ServiceID"];
                string query = $"DELETE FROM Services WHERE ServiceID={servicesID}";
                GetDataFromDatabase(query);
                DisplayDataInGrid();
            }
        }
    }
}
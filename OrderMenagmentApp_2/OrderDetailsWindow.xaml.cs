using System.Windows;

namespace OrderMenagmentApp_2
{
    public partial class OrderDetailsWindow : Window
    {
        public OrderDetailsWindow()
        {
            InitializeComponent();
        }

        public void SetOrderDetails(string description, string startDate, string endDate, string customer, string employee, string service, string status)
        {
            DescriptionTextBox.Text = description;
            StartDateTextBox.Text = startDate;
            EndDateTextBox.Text = endDate;
            CustomerTextBox.Text = customer;
            EmployeeTextBox.Text = employee;
            ServiceTextBox.Text = service;
            StatusTextBox.Text = status;
        }
    }
}

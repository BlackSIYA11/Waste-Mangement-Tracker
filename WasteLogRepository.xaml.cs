using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Waste_Management_Tracker
{
    public partial class WasteLogWindow : Window
    {
        private string connectionString = "server=localhost;database=Waste;uid=root;pwd=BlackPanther11;";

        public WasteLogWindow()
        {
            InitializeComponent();
        }

        // Event handler for Log Waste button click
        private void LogWasteButton_Click(object sender, RoutedEventArgs e)
        {
            string wasteType = (WasteTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string quantityText = QuantityTextBox.Text;
            DateTime date = DatePicker.SelectedDate ?? DateTime.Now;

            if (string.IsNullOrWhiteSpace(wasteType) || string.IsNullOrWhiteSpace(quantityText) || !decimal.TryParse(quantityText, out decimal quantity))
            {
                StatusTextBlock.Text = "Please fill in all fields correctly.";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            int userId = 1; // Replace with the actual user ID from login session

            WasteLogRepository wasteLogRepo = new WasteLogRepository();
            bool isLogged = wasteLogRepo.LogWaste(userId, wasteType, quantity, date);

            if (isLogged)
            {
                StatusTextBlock.Text = "Waste logged successfully.";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Green;

                // Fetch and display recycling tip from the database
                string tip = wasteLogRepo.GetRecyclingTip(wasteType);
                MessageBox.Show($"Recycling Tip:\n{tip}", "Recycling Advice", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                StatusTextBlock.Text = "Failed to log waste. Please try again.";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        // Event handler for Generate Report button click
        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            int userId = 1; // Replace with actual logged-in user ID
            WasteLogRepository wasteLogRepo = new WasteLogRepository();
            double carbonFootprint = wasteLogRepo.CalculateCarbonFootprint(userId);

            MessageBox.Show($"Your total carbon footprint is {carbonFootprint:F2} kg CO₂e.",
                            "Carbon Footprint Report",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}

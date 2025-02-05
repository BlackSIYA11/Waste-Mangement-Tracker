using MySql.Data.MySqlClient;
using System;

namespace Waste_Management_Tracker
{
    public class WasteLogRepository
    {
        private string connectionString = "server=localhost;database=Waste;uid=root;pwd=BlackPanther11;";

        // Log the waste entry into the database
        public bool LogWaste(int userId, string wasteType, decimal quantity, DateTime date)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO WasteLog (user_id, waste_type, quantity, date) VALUES (@userId, @wasteType, @quantity, @date)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@wasteType", wasteType);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@date", date);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        // Fetch recycling tip from the database
        public string GetRecyclingTip(string wasteType)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT tip FROM RecyclingTips WHERE waste_type = @wasteType";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@wasteType", wasteType);

                    try
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return "No recycling tip available for this waste type.";
                        }
                    }
                    catch (MySqlException ex)
                    {
                        return $"Error fetching tip: {ex.Message}";
                    }
                }
            }
        }

        // Calculate the carbon footprint for the user based on their waste logs
        public double CalculateCarbonFootprint(int userId)
        {
            double totalCarbonFootprint = 0;

            // Query the database for the waste logs of the user
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT quantity, waste_type FROM WasteLog WHERE user_id = @userId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal quantity = reader.GetDecimal("quantity");
                            string wasteType = reader.GetString("waste_type");

                            // Example: Calculate carbon footprint based on waste type (adjust calculation as necessary)
                            decimal carbonEmission = 0; // Default to 0 in case of unrecognized waste type

                            if (wasteType == "Plastic")
                            {
                                carbonEmission = quantity * 0.8m; // Use decimal constant
                            }
                            else if (wasteType == "Paper")
                            {
                                carbonEmission = quantity * 0.5m; // Use decimal constant
                            }

                            // Add the calculated emission to the total carbon footprint
                            totalCarbonFootprint += (double)carbonEmission; // Convert decimal to double for the final result
                        }
                    }
                }
            }

            return totalCarbonFootprint;
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using BCrypt.Net;

namespace Waste_Management_Tracker
{
    public class UserRepository
    {
        private string connectionString = "server=localhost;database=Waste;uid=root;pwd=BlackPanther11;";

        // Method to hash passwords using BCrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Method to check if email already exists
        private bool DoesEmailExist(string email)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE email = @Email";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0; // Return true if email already exists
                }
            }
        }

        // Method to validate password
        private bool IsValidPassword(string password)
        {
            if (password.Length < 8)
            {
                return false; // Password must be at least 8 characters long
            }

            if (!password.Any(char.IsDigit) || !password.Any(char.IsLetter) || !password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return false; // Password must contain letters, numbers, and special characters
            }

            return true;
        }

        // Register a new user
        public bool RegisterUser(string username, string email, string password)
        {
            if (!IsValidEmail(email))
            {
                Console.WriteLine("Invalid email format.");
                return false;
            }

            if (DoesEmailExist(email))
            {
                Console.WriteLine("Email already registered.");
                return false;
            }

            if (!IsValidPassword(password))
            {
                Console.WriteLine("Password is too weak. Ensure it has at least 8 characters, including letters, numbers, and special characters.");
                return false;
            }

            string hashedPassword = HashPassword(password);

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (username, email, password) VALUES (@username, @email, @password)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

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

        // User login
        public bool LoginUser(string email, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT password FROM Users WHERE username = @Username";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", email);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPasswordHash = reader.GetString("password");
                                return BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                return false;
            }

            return false; // Email not found or password mismatch
        }

        // Get User ID after login
        public int GetUserId(string email)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id FROM Users WHERE email = @Email";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}

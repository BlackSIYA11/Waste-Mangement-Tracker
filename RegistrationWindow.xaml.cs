using System.Windows;
using System.Windows.Controls;

namespace Waste_Management_Tracker
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        // Event handler for GotFocus on usernameBox
        private void UsernameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ClearPlaceholderText(usernameBox, "Username");
        }

        // Event handler for LostFocus on usernameBox
        private void UsernameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetPlaceholderText(usernameBox, "Username");
        }

        // Event handler for GotFocus on emailBox
        private void EmailBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ClearPlaceholderText(emailBox, "Email");
        }

        // Event handler for LostFocus on emailBox
        private void EmailBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetPlaceholderText(emailBox, "Email");
        }

        // Event handler for GotFocus on passwordBox
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ClearPlaceholderPassword(passwordBox, "Password");
        }

        // Event handler for LostFocus on passwordBox
        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetPlaceholderPassword(passwordBox, "Password");
        }

        // Helper method to clear placeholder text
        private void ClearPlaceholderText(TextBox textBox, string placeholder)
        {
            if (textBox.Text == placeholder)
            {
                textBox.Text = "";
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            }
        }

        // Helper method to set placeholder text
        private void SetPlaceholderText(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        // Helper method to clear placeholder password
        private void ClearPlaceholderPassword(PasswordBox passwordBox, string placeholder)
        {
            if (passwordBox.Password == placeholder)
            {
                passwordBox.Password = "";
            }
        }

        // Helper method to set placeholder password
        private void SetPlaceholderPassword(PasswordBox passwordBox, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.Password = placeholder;
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text;
            string email = emailBox.Text;
            string password = passwordBox.Password; // Use Password property for PasswordBox

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                statusText.Text = "Please fill in all fields.";
                return;
            }

            UserRepository userRepo = new UserRepository();

            if (userRepo.RegisterUser(username, email, password))
            {
                // Show success message
                MessageBox.Show("Registration successful! Please log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Close the Registration window and show the Login (MainWindow)
                this.Close();

                // Set the MainWindow and show it (Login Page)
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.Show();
            }
            else
            {
                statusText.Text = "Registration failed. Please check your inputs.";
            }
        }

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            // Close the Registration window and show the Login window (MainWindow)
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            this.Close();
        }
    }
}

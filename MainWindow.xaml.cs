using System.Windows;

namespace Waste_Management_Tracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Handle focus on Username TextBox
        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            if (usernameBox.Text == "Username")
            {
                usernameBox.Text = "";
                usernameBox.Foreground = System.Windows.Media.Brushes.Black; // Change text color to black when user starts typing
            }
        }

        // Handle Lost Focus on Username TextBox
        private void Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameBox.Text))
            {
                usernameBox.Text = "Username";
                usernameBox.Foreground = System.Windows.Media.Brushes.Gray; // Placeholder text color
            }
        }

        // Handle focus on Password TextBox
        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password == "Password")
            {
                passwordBox.Password = "";
                passwordBox.Foreground = System.Windows.Media.Brushes.Black; // Change text color to black when user starts typing
            }
        }

        // Handle Lost Focus on Password TextBox
        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.Password = "Password";
                passwordBox.Foreground = System.Windows.Media.Brushes.Gray; // Placeholder text color
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text;
            string password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                statusText.Text = "Please enter both username and password.";
                return;
            }

            UserRepository userRepo = new UserRepository();
            if (userRepo.LoginUser(username, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Open WasteLogWindow after successful login
                WasteLogWindow wasteLogWindow = new WasteLogWindow();
                wasteLogWindow.Show();
                this.Close(); // Close the login window
            }
            else
            {
                statusText.Text = "Invalid credentials. Try again.";
            }
        }

        private void OpenRegisterPage(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWindow = new RegistrationWindow();
            regWindow.Show();
            this.Close();
        }
    }
}

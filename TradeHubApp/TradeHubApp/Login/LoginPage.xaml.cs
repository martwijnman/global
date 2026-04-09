using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TradeHubApp.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TradeHubApp.Login
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Name == Name.Text);
            if (BCrypt.Net.BCrypt.Verify(Password.Password, user.Password))
            {
                Data.User.SetLoggedInUser(user);
                if (user.Role == "User")
                {
                    Frame.Navigate(typeof(Layouts.PublicLayout));
                }
                else
                {
                    Frame.Navigate(typeof(Layouts.ModeratorLayout));
                }
                
            }
            else
            {
                error.Text = "Gebruikersnaam of wachtwoord klopt niet.";
            }
        }
        private void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login.RegisterPage));
        }
    }
}

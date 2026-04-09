using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubApp.Data
{
    internal class User
    {
        public static User LoggedInUser { get; private set; }
        public static event EventHandler OnUserLoggedIn;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Activated { get; set; }
        public string ActivatedText => "Geactiveerd" ?? "Niet geavtiveerd";
        public List<Product> Products { get; set; } = new();

        public List<Order> Orders { get; set; } = new();
        internal static void SetLoggedInUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User is null");
            }

            LoggedInUser = user;
            OnUserLoggedIn?.Invoke(user, EventArgs.Empty);
        }

    }
}

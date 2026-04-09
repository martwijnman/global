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

namespace TradeHubApp.Product
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public int _productId;
        public DetailPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
            {
                // Safety: avoid blank page if something goes wrong
                NameTextBlock.Text = "Geen klant geselecteerd";
                return;
            }

            _productId = (int)e.Parameter;
        }
        private void Button_Click_Buy(object sender, RoutedEventArgs e)
        {
            var db = new AppDbContext();
            db.Add(new Data.Order
            {
                BuyerId = Data.User.LoggedInUser.Id,
                ProductId = _productId,
            });
            db.SaveChanges();
            Frame.Navigate(typeof(Product.OverviewPage));
        }
    }
}

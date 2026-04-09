using Microsoft.EntityFrameworkCore;
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

namespace TradeHubApp.Order
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OverviewPage : Page
    {
        public OverviewPage()
        {
            InitializeComponent();
        }
        public void LoadData()
        {
            var db = new AppDbContext();
            OrderListView.ItemsSource = db.Orders.Include(o => o.Product).Where(o => o.Product.UserId == Data.User.LoggedInUser.Id).ToList();
        }
        private void Button_Click_Detail(object sender, RoutedEventArgs e)
        {
            var db = new AppDbContext();
            var knop = (Button)sender;
            var order = (Data.Order)knop.DataContext;
            var panel = new StackPanel();
            var data = db.Orders.Include(o => o.Product).Include(o => o.Buyer).FirstOrDefault(o => o.Product.UserId == Data.User.LoggedInUser.Id);

            var nameBox = new TextBox { 
                Text = data.Buyer.Name,
            };

            var productBox = new TextBox
            {
                Text = data.Product.Name,
            };

            panel.Children.Add(nameBox);
            panel.Children.Add(productBox);

            var accepButton = new Button
            {
                Content = "Accepteren",
                HorizontalAlignment = HorizontalAlignment.Left
            };

            accepButton.Click += (s, e) =>
            {
                order.IsPaid = true;

            };
            panel.Children.Add(accepButton);

            var rejectButton = new Button
            {
                Content = "Weigeren",
                HorizontalAlignment = HorizontalAlignment.Left
            }
            ;

            rejectButton.Click += (s, e) =>
            {
                order.IsPaid = false;
            };
            panel.Children.Add(rejectButton);

            var dialog = new ContentDialog
            {
                Title = "Order informatie",
                Content = panel,
                CloseButtonText = "Annuleren",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            dialog.ShowAsync();
        }
    }
}

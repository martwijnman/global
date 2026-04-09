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
using Microsoft.EntityFrameworkCore;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TradeHubApp.Product
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OverviewPage : Page
    {
        public OverviewPage()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            var db = new AppDbContext();
            ProductListView.ItemsSource = db.Products.ToList();
        }

        private void Button_Click_Detail(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var product = (Data.Product)button.DataContext;
            Frame.Navigate(typeof(Product.DetailPage), product.Id);
        }
    }
}

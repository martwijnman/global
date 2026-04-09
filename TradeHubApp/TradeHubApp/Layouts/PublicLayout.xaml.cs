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

namespace TradeHubApp.Layouts
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PublicLayout : Page
    {
        public PublicLayout()
        {
            InitializeComponent();
            PublicFrame.Navigate(typeof(Product.OverviewPage));
        }

        private void Button_Click_Product(object sender, RoutedEventArgs e)
        {
            PublicFrame.Navigate(typeof(Product.OverviewPage));
        }
        private void Button_Click_Review(object sender, RoutedEventArgs e)
        {
            PublicFrame.Navigate(typeof(Review.OverviewPage));
        }
        private void Button_Click_My_Products(object sender, RoutedEventArgs e)
        {
            PublicFrame.Navigate(typeof(Product.OverviewPage));
        }
        private void Button_Click_Order(object sender, RoutedEventArgs e)
        {
            PublicFrame.Navigate(typeof(Order.OverviewPage));
        }
    }
}

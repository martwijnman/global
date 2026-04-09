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
    public sealed partial class MyProductPage : Page
    {
        public MyProductPage()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            var db = new Data.AppDbContext();
            ProductListView.ItemsSource = db.Products.Where(p => p.UserId == Data.User.LoggedInUser.Id).ToList();
        }
        private async void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var product = (Data.Product)button.DataContext;

            var nameBox = new TextBox { Text = product.Name };
            var priceBox = new TextBox { Text = product.Price.ToString() };

            var panel = new StackPanel();
            panel.Children.Add(new TextBlock { Text = "Naam" });
            panel.Children.Add(nameBox);
            panel.Children.Add(new TextBlock { Text = "Prijs" });
            panel.Children.Add(priceBox);

            var dialog = new ContentDialog
            {
                Title = "Product bewerken",
                Content = panel,
                PrimaryButtonText = "Opslaan",
                CloseButtonText = "Annuleren",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var db = new AppDbContext();

                var dbProduct = db.Products.FirstOrDefault(p => p.Id == product.Id);

                if (dbProduct != null)
                {
                    dbProduct.Name = nameBox.Text;
                    dbProduct.Price = double.Parse(priceBox.Text);

                    db.SaveChanges();
                }
            }
        }
    }
}

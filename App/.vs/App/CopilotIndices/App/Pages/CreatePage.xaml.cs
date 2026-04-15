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
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatePage : Page
    {
        public CreatePage()
        {
            InitializeComponent();
            LoadData();
            LoadDraft();
        }

        private void LoadData()
        {
            using var db = new AppDbContext();

            ClientComboBox.ItemsSource = db.Clients.ToList();
            ApplicationComboBox.ItemsSource = db.Applications.ToList();
        }

        private void LoadDraft()
        {
            // haal de data uit de database, en voeg het in de combobox
            using var db = new AppDbContext();

            var clients = db.Clients.ToList();
            var applications = db.Applications.ToList();

            ClientComboBox.ItemsSource = clients;
            ApplicationComboBox.ItemsSource = applications;

            // als hij niet leeg is selecteer hij de draft vanuit de Id
            if (Draft.ClientId != 0)
            {
                ClientComboBox.SelectedItem = clients.FirstOrDefault(c => c.Id == Draft.ClientId);
            }

            if (Draft.ApplicationId != 0)
            {
                ApplicationComboBox.SelectedItem = applications.FirstOrDefault(a => a.Id == Draft.ApplicationId);
            }

            // hij pakt de datum en beschrijving
            DateDatePicker.Date = new DateTimeOffset(Draft.DateTime);


            DescriptionTextBox.Text = Draft.Description;
        }

        // als 1 van de velden veranderd slaat hij op in drafts
        private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientComboBox.SelectedItem is Client client)
            {
                Draft.ClientId = client.Id;
            }
        }

        private void ApplicationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ApplicationComboBox.SelectedItem is Application application)
            {
                Draft.ApplicationId = application.Id;
            }
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Draft.Description = DescriptionTextBox.Text;
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {

            if (ClientComboBox.SelectedItem is not Client client)
            {
                return;
            }

            if (ApplicationComboBox.SelectedItem is not Application application)
            {
                return;
            }
            // sla de afspraak op
            using var db = new AppDbContext();

            db.Appointments.Add(new Appointment
            {
                ClientId = client.Id,
                ApplicationId = application.Id,
                ProblemDescription = DescriptionTextBox.Text,
                DateTime = DateDatePicker.Date.DateTime,
            });

            db.SaveChanges();

            // maak alles leeg
            Draft.ClientId = 0;
            Draft.ApplicationId = 0;
            Draft.Description = "";
            Draft.DateTime = DateTime.Now;

            // maak alle velden ook leeg
            ClientComboBox.SelectedItem = null;
            ApplicationComboBox.SelectedItem = null;
            DescriptionTextBox.Text = "";
        }

        private void Button_Click_GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}

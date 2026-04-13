using Microsoft.UI;
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
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Microsoft.EntityFrameworkCore;
using App.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App
{
    public sealed partial class AgendaPage : Page
    {
        // Databasecontext om met de lokale agenda-opslag te werken.
        private AppDbContext _db = new AppDbContext();

        // Houdt de huidige weergegeven maand bij voor het kalenderheader.
        private DateTime _currentDate = DateTime.Today;

        // Alle afspraken die in het geheugen zijn geladen.
        private List<Appointment> _allAppointments;

        public AgendaPage()
        {
            InitializeComponent(); // UI initialiseren.

            LoadAllAppointments(); // Laad afspraken en klantlijst.

            UpdateCalendarHeader(); // Toon de huidige maand en jaar.

            UpdateSelectedDayStatus(null); // Reset de status voor geselecteerde dag.

            CalendarControl.SetDisplayDate(_currentDate); // Laat de huidige maand zien.
        }

        // Laadt alle afspraken uit de database, inclusief gekoppelde klantgegevens.
        private void LoadAllAppointments()
        {
            var db = new AppDbContext();
            _allAppointments = db.Appointments
                .Include(a => a.Client)
                .OrderByDescending(a => a.DateTime)
                .ToList();

            // Vul de combobox met 'alle klanten' en individuele klanten.
            var clients = new List<Client> { new Client { Id = 0, Name = "Show All" } };
            clients.AddRange(_db.Clients.ToList());
            ClientComboBox.ItemsSource = clients;
        }

        // Laadt afspraken voor de gekozen dag en toont ze in de lijst.
        private void LoadAppointmentsForDate(DateTime selectedDate)
        {
            var appointmentsForSelectedDate = _allAppointments
                .Where(a => a.DateTime.Date == selectedDate.Date)
                .ToList();

            DayAppointmentsControl.ItemsSource = appointmentsForSelectedDate;
            UpdateSelectedDayStatus(selectedDate);
        }

        // Stelt de kalenderheader in op de huidige maand en jaar.
        private void UpdateCalendarHeader()
        {
            MonthYearText.Text = _currentDate.ToString("MMMM yyyy");
        }

        // Toont een korte status onder de kalender voor de geselecteerde dag.
        private void UpdateSelectedDayStatus(DateTime? selectedDate)
        {
            if (!selectedDate.HasValue)
            {
                SelectedDayStatusText.Text = "Select a date to view appointments.";
                return;
            }

            int appointmentCount = _allAppointments.Count(a =>
                a.DateTime.Date == selectedDate.Value.Date);

            if (appointmentCount > 0)
            {
                SelectedDayStatusText.Text =
                    $"There are {appointmentCount} appointment(s) on this day.";
            }
            else
            {
                SelectedDayStatusText.Text =
                    "No appointments on this day.";
            }
        }

        // Wordt aangeroepen voor elke dag die in de kalender wordt weergegeven.
        // Hiermee geven we visuele markering wanneer er afspraken op die dag zijn.
        private void CalendarControl_DayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            if (args.Item == null)
                return;

            DateTime day = args.Item.Date.Date;

            bool hasAppointments =
                _allAppointments != null &&
                _allAppointments.Any(a => a.DateTime.Date == day);

            if (hasAppointments)
            {
                // Markeer de dag met een duidelijke rand en achtergrond.
                args.Item.BorderBrush =
                    new SolidColorBrush(Colors.Red);
                args.Item.BorderThickness = new Thickness(2);
                args.Item.Background =
                    new SolidColorBrush(ColorHelper.FromArgb(80, 255, 215, 0));

                try
                {
                    args.Item.CornerRadius = new CornerRadius(20);
                }
                catch { }
            }
            else
            {
                // Reset opmaak voor dagen zonder afspraken.
                args.Item.BorderBrush = null;
                args.Item.BorderThickness = new Thickness(0);
                args.Item.Background = null;

                try
                {
                    args.Item.CornerRadius = new CornerRadius(0);
                }
                catch { }
            }
        }

        // Gebruiker selecteert een datum in de kalender.
        // Laad dan de afspraken voor die gekozen datum.
        private async void CalendarControl_SelectedDatesChanged(
            CalendarView sender,
            CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (sender.SelectedDates.Count > 0)
            {
                DateTime selectedDate = sender.SelectedDates[0].Date;
                LoadAppointmentsForDate(selectedDate);
            }
        }

        // Navigeer naar de vorige maand.
        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddMonths(-1);
            CalendarControl.SetDisplayDate(_currentDate);
            UpdateCalendarHeader();
        }

        // Navigeer naar de volgende maand.
        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddMonths(1);
            CalendarControl.SetDisplayDate(_currentDate);
            UpdateCalendarHeader();
        }

        // Navigeer naar het vorige jaar.
        private void PreviousYear_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddYears(-1);
            CalendarControl.SetDisplayDate(_currentDate);
            UpdateCalendarHeader();
        }

        // Navigeer naar het volgende jaar.
        private void NextYear_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddYears(1);
            CalendarControl.SetDisplayDate(_currentDate);
            UpdateCalendarHeader();
        }

        // Open de pagina om een nieuwe afspraak te maken.
        private void Button_Click_Create(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreatePage));
        }

        // Filter afspraken op basis van de geselecteerde klant.
        private void ClientComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (ClientComboBox.SelectedItem is Client client)
            {
                using var db = new AppDbContext();

                if (client.Id == 0)
                {
                    // Toon alle afspraken wanneer 'Alle klanten' is geselecteerd.
                    _allAppointments = db.Appointments
                        .Include(a => a.Client)
                        .OrderByDescending(a => a.DateTime)
                        .ToList();
                }
                else
                {
                    // Toon alleen afspraken voor de geselecteerde klant.
                    _allAppointments = db.Appointments
                        .Include(a => a.Client)
                        .Where(a => a.ClientId == client.Id)
                        .OrderByDescending(a => a.DateTime)
                        .ToList();
                }

                RefreshCalendarMarkers();

                // Als er een geselecteerde datum is, laad de afspraken opnieuw.
                if (CalendarControl.SelectedDates.Count > 0)
                {
                    DateTime selectedDate = CalendarControl.SelectedDates[0].Date;
                    LoadAppointmentsForDate(selectedDate);
                }
                else
                {
                    DayAppointmentsControl.ItemsSource = null;
                    UpdateSelectedDayStatus(null);
                    CalendarControl.SelectedDates.Clear();
                }
            }
        }

        // Vernieuwt de kalenderweergave nadat de dataset of filters zijn aangepast.
        private void RefreshCalendarMarkers()
        {
            if (CalendarControl == null)
                return;

            CalendarControl.InvalidateMeasure();
            CalendarControl.InvalidateArrange();
            CalendarControl.UpdateLayout();
            CalendarControl.SetDisplayDate(_currentDate);
        }

        // Genereert een ICS-bestand voor de geselecteerde dag en laat de gebruiker het opslaan.
        private async void Button_Click_GenerateIcs(object sender, RoutedEventArgs e)
        {
            if (CalendarControl.SelectedDates.Count == 0)
            {
                var warningDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Geen datum geselecteerd",
                    Content = "Selecteer eerst een dag in de kalender voordat je een ICS genereert.",
                    CloseButtonText = "Sluit"
                };

                await warningDialog.ShowAsync();
                return;
            }

            var selectedDate = CalendarControl.SelectedDates[0].Date;
            var clients = new List<Client> { new Client { Id = 0, Name = "Alle klanten" } };
            clients.AddRange(_db.Clients.ToList());

            var clientComboBox = new ComboBox
            {
                ItemsSource = clients,
                DisplayMemberPath = "Name",
                SelectedIndex = 0,
                Width = 320
            };

            var dialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = $"Kies een klant voor {selectedDate:dd-MM-yyyy}",
                Content = new StackPanel
                {
                    Spacing = 10,
                    Children =
                    {
                        new TextBlock
                        {
                            Text = "Selecteer een klant om de ICS voor die dag te genereren.",
                            TextWrapping = TextWrapping.Wrap
                        },
                        clientComboBox
                    }
                },
                PrimaryButtonText = "Genereer",
                CloseButtonText = "Annuleren"
            };

            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
                return;

            if (clientComboBox.SelectedItem is not Client selectedClient)
            {
                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Geen klant geselecteerd",
                    Content = "Kies eerst een klant in de lijst.",
                    CloseButtonText = "Sluit"
                }.ShowAsync();
                return;
            }

            var appointmentsForClient = _allAppointments
                .Where(a => a.DateTime.Date == selectedDate.Date &&
                            (selectedClient.Id == 0 || a.ClientId == selectedClient.Id))
                .ToList();

            // Maak de ICS-tekst van de geselecteerde afspraken.
            var icsText = IcsHelper.CreateCalendarForDate(selectedDate, appointmentsForClient);

            // Stel de file-save dialog in zodat de gebruiker zelf kan kiezen waar het bestand komt.
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = $"agenda-{selectedDate:yyyy-MM-dd}",
                DefaultFileExtension = ".ics"
            };
            savePicker.FileTypeChoices.Add("ICS-bestand", new List<string> { ".ics" });

            var app = App.Current as App;
            if (app?.MainWindow != null)
            {
                var hwnd = WindowNative.GetWindowHandle(app.MainWindow);
                InitializeWithWindow.Initialize(savePicker, hwnd);
            }

            var storageFile = await savePicker.PickSaveFileAsync();
            if (storageFile == null)
            {
                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Opslaan geannuleerd",
                    Content = "De ICS-export is geannuleerd.",
                    CloseButtonText = "Sluit"
                }.ShowAsync();
                return;
            }

            try
            {
                await FileIO.WriteTextAsync(storageFile, icsText);
                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "ICS opgeslagen",
                    Content = $"Het bestand is opgeslagen als: {storageFile.Name}",
                    CloseButtonText = "Sluit"
                }.ShowAsync();
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Fout bij opslaan",
                    Content = $"Kon het ICS-bestand niet opslaan: {ex.Message}",
                    CloseButtonText = "Sluit"
                }.ShowAsync();
            }
        }

        // Toont een dialog waarmee de gebruiker een ICS-bestand kan plakken
        // en direct kan versturen naar een opgegeven URL.
        private async void Button_Click_ImportIcs(object sender, RoutedEventArgs e)
        {
            var icsTextBox = new TextBox
            {
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Height = 200,
                PlaceholderText = "Plak hier je .ics-tekst..."
            };

            var urlTextBox = new TextBox
            {
                PlaceholderText = "Post URL (bijv. https://example.com/ics)",
                Margin = new Thickness(0, 10, 0, 0)
            };

            var dialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Post ICS-bestand",
                Content = new StackPanel
                {
                    Spacing = 10,
                    Children =
                    {
                        new TextBlock { Text = "Vul de ICS-inhoud in en geef de post-url op.", TextWrapping = TextWrapping.Wrap },
                        urlTextBox,
                        icsTextBox
                    }
                },
                PrimaryButtonText = "Verstuur",
                CloseButtonText = "Annuleren"
            };

            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
                return;

            var requestUrl = string.IsNullOrWhiteSpace(urlTextBox.Text)
                ? "https://example.com/ics"
                : urlTextBox.Text.Trim();

            var icsContent = icsTextBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(icsContent))
            {
                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Fout",
                    Content = "ICS inhoud mag niet leeg zijn.",
                    CloseButtonText = "Sluit"
                }.ShowAsync();
                return;
            }

            try
            {
                // Post de ICS-tekst via de helper en laat de status zien.
                var response = await DataHelper.PostIcsAsync(requestUrl, icsContent);

                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Resultaat",
                    Content = response.IsSuccessStatusCode
                        ? "ICS succesvol verstuurd."
                        : $"Fout bij versturen: {response.StatusCode}",
                    CloseButtonText = "Sluit"
                }.ShowAsync();
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Fout",
                    Content = $"Kon ICS niet versturen: {ex.Message}",
                    CloseButtonText = "Sluit"
                }.ShowAsync();
            }
        }
    }
}



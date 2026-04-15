using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Data
{
    // Zet afspraken om naar een geldig ICS-kalenderbestand voor export per geselecteerde dag.
    internal static class IcsHelper
    {
        // Bouwt een complete kalenderstring op en maakt per afspraak een VEVENT-item aan.
        public static string CreateCalendarForDate(DateTime selectedDate, IEnumerable<Appointment> appointments)
        {
            var builder = new StringBuilder(); // zet een effiencentie manier van een string aanmaken, omdat op deze manier  niet steeds een nieuwe string hoef aangemaakt te worden sneller voor tekst en idealer voor loops 
            builder.AppendLine("BEGIN:VCALENDAR"); // kalender bestand beginnen
            builder.AppendLine("VERSION:2.0"); // meest gebruikte versie
            builder.AppendLine("PRODID:-//App//Agenda ICS//NL");
            builder.AppendLine($"X-WR-CALNAME:Agenda {selectedDate:yyyy-MM-dd}"); // zet datum 
            builder.AppendLine("CALSCALE:GREGORIAN");
            builder.AppendLine("METHOD:PUBLISH"); // maakt de afspraak aan

            var appointmentList = appointments?.ToList() ?? new List<Appointment>();
            if (appointmentList.Count > 0)
            {
                foreach (var appointment in appointmentList)
                {
                    // zet de tijd van de afspraak
                    var start = appointment.DateTime.ToUniversalTime();
                    // zet eindtijd 1 uur later
                    var end = appointment.DateTime.AddHours(1).ToUniversalTime();
                    //start de calender item
                    builder.AppendLine("BEGIN:VEVENT");
                    // geeft unieke id
                    builder.AppendLine($"UID:{Guid.NewGuid()}");
                    // datum zetten
                    builder.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
                    // zet de start time aan
                    builder.AppendLine($"DTSTART:{start:yyyyMMddTHHmmssZ}");
                    // zet wanneer het item is gestopt
                    builder.AppendLine($"DTEND:{end:yyyyMMddTHHmmssZ}");
                    // titel
                    builder.AppendLine($"SUMMARY:{EscapeIcsText(appointment.Client?.Name ?? "Afspraak")}");
                    // beschrijving
                    builder.AppendLine($"DESCRIPTION:{EscapeIcsText(appointment.ProblemDescription ?? string.Empty)}");
                    // plek
                    builder.AppendLine($"LOCATION:{EscapeIcsText(appointment.Client?.Name ?? string.Empty)}");
                    // einde item
                    builder.AppendLine("END:VEVENT");
                }
            }
            else
            {
                var start = selectedDate.Date.ToUniversalTime();
                builder.AppendLine("BEGIN:VEVENT"); // begint afspraak
                builder.AppendLine($"UID:{Guid.NewGuid()}"); // unieke id
                builder.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}"); // zet tijd nu als aangemaakt
                builder.AppendLine($"DTSTART:{start:yyyyMMddTHHmmssZ}"); // atarttijd afspraak
                builder.AppendLine($"DTEND:{start.AddHours(1):yyyyMMddTHHmmssZ}"); // zet het 1 uur later
                builder.AppendLine("SUMMARY:Geen afspraken"); // geen afspraken erbij zetten als er geen afspraak is
                builder.AppendLine("DESCRIPTION:Er zijn geen afspraken voor deze dag.");
                builder.AppendLine("END:VEVENT");
            }

            builder.AppendLine("END:VCALENDAR"); // hier eindigd hij het bestand
            return builder.ToString();
        }

        // Escapet tekens die in een ICS-veld speciale betekenis hebben.
        private static string EscapeIcsText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text
                .Replace("\\", "\\\\")
                .Replace("\n", "\\n")
                .Replace(";", "\\;")
                .Replace(",", "\\,");
        }
    }
}

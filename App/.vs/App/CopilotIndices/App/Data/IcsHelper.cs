using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Data
{
    internal static class IcsHelper
    {
        public static string CreateCalendarForDate(DateTime selectedDate, IEnumerable<Appointment> appointments)
        {
            var builder = new StringBuilder();
            builder.AppendLine("BEGIN:VCALENDAR");
            builder.AppendLine("VERSION:2.0");
            builder.AppendLine("PRODID:-//App//Agenda ICS//NL");
            builder.AppendLine($"X-WR-CALNAME:Agenda {selectedDate:yyyy-MM-dd}");
            builder.AppendLine("CALSCALE:GREGORIAN");
            builder.AppendLine("METHOD:PUBLISH");

            var appointmentList = appointments?.ToList() ?? new List<Appointment>();
            if (appointmentList.Count > 0)
            {
                foreach (var appointment in appointmentList)
                {
                    var start = appointment.DateTime.ToUniversalTime();
                    var end = appointment.DateTime.AddHours(1).ToUniversalTime();

                    builder.AppendLine("BEGIN:VEVENT");
                    builder.AppendLine($"UID:{Guid.NewGuid()}");
                    builder.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
                    builder.AppendLine($"DTSTART:{start:yyyyMMddTHHmmssZ}");
                    builder.AppendLine($"DTEND:{end:yyyyMMddTHHmmssZ}");
                    builder.AppendLine($"SUMMARY:{EscapeIcsText(appointment.Client?.Name ?? "Afspraak")}");
                    builder.AppendLine($"DESCRIPTION:{EscapeIcsText(appointment.ProblemDescription ?? string.Empty)}");
                    builder.AppendLine($"LOCATION:{EscapeIcsText(appointment.Client?.Name ?? string.Empty)}");
                    builder.AppendLine("END:VEVENT");
                }
            }
            else
            {
                var start = selectedDate.Date.ToUniversalTime();
                builder.AppendLine("BEGIN:VEVENT");
                builder.AppendLine($"UID:{Guid.NewGuid()}");
                builder.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
                builder.AppendLine($"DTSTART:{start:yyyyMMddTHHmmssZ}");
                builder.AppendLine($"DTEND:{start.AddHours(1):yyyyMMddTHHmmssZ}");
                builder.AppendLine("SUMMARY:Geen afspraken");
                builder.AppendLine("DESCRIPTION:Er zijn geen afspraken voor deze dag.");
                builder.AppendLine("END:VEVENT");
            }

            builder.AppendLine("END:VCALENDAR");
            return builder.ToString();
        }

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

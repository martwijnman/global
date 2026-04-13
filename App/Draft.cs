using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal static class Draft
    {
        public static DateTime DateTime { get; set; } = DateTime.Now;
        public static int ClientId { get; set; } = 0;
        public static string Description { get; set; } = "";
        public static int ApplicationId { get; set; } = 0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal class Appointment
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
        public string ProblemDescription { get; set; }

        public DateTime DateTime { get; set; }

    }
}

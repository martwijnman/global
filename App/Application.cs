using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public Type Type { get; set; }
    }
    internal class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

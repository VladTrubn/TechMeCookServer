using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechMeCookServer.Models
{
    public class Ingredient
    {
        public int id { get; set; }
        public String? image { get; set; }
        public String name { get; set; }
        public String amount { get; set; }
        public String unit { get; set; }
    }
}

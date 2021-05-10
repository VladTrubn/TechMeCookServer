using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TechMeCookServer.Models
{
    public class Ingredient
    {
        [Key]
        public Int32 Ingrid { get; set; }
        public String? image { get; set; }
        public String name { get; set; }
        public double amount { get; set; }
        public String? unit { get; set; }
    }
}

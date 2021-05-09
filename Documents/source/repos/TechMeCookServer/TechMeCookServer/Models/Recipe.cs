using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechMeCookServer.Models
{
    public class Recipe
    {
        [Key]
        public Guid RId { get; set; } = Guid.NewGuid();
        public int id { get; set; }
        public String? spoonacularSourceUrl { get; set; }
        public String title { get; set; }
        public String summary { get; set; }
        public int readyInMinutes { get; set; }
        public String? image { get; set; }

        public ICollection<Comment> comments { get; set; }


    }
}

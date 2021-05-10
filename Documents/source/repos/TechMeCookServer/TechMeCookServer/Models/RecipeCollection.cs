using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechMeCookServer.Models
{
    public class RecipeCollection
    {
        public List<UndetailedRecipe> results { get; set; }
        public List<UndetailedRecipe> recipes { get; set; }
    }
}

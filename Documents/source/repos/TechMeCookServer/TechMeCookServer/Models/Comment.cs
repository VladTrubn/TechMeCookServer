using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TechMeCookServer.Models
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public String Text { get; set; }
        public String CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }
        public DateTime Created { get; set; }

        public String RecipeDbId { get; set; }

        [JsonIgnore]
        public Recipe Recipe { get; set; }
        
        public Guid RecipeId { get; set; }
    }
}

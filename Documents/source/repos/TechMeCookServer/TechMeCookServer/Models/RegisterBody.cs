using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechMeCookServer.Models
{
    public class RegisterBody
    {
        public String email { get; set; }
        public String password { get; set; }
        public String username { get; set; }
    }
}

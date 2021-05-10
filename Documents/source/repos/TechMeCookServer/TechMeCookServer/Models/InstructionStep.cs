using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TechMeCookServer.Models
{
    public class InstructionStep
    {
        [Key]
        public Int32 id { get; set; }
        public int number { get; set; }
        public String step { get; set; }
    }
}

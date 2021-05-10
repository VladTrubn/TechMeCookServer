using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace TechMeCookServer.Models
{
    public class Instruction
    {
        [Key]
        public Int32 Id { get; set; }
        public String name { get; set; }
        public ICollection<InstructionStep> steps { get; set; }
    }
}

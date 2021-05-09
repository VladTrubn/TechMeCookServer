using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechMeCookServer.Models
{
    public class Instruction
    {
        public String name { get; set; }
        public List<InstructionStep> steps { get; set; }
    }
}

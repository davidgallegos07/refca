using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class TeacherResearch
    {
        public string TeacherId { get; set; }
        public int ResearchId { get; set; }
        public int Order { get; set; }
        public string Role { get; set; }
        public Teacher Teacher { get; set; }
        public Research Research { get; set; }
    }
}
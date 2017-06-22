using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class ConsolidationGrade
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public ICollection<AcademicBody> AcademicBodies { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class ResearchLine
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? AcademicBodyId { get; set; } 
        public AcademicBody AcademicBody { get; set; }
        public ICollection<Thesis> Theses { get; set; }
        public ICollection<Research> Research { get; set; }
        public ICollection<EducationProgramResearchLine> EducationProgramResearchLine { get; set; }
    }
}
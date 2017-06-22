using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class EducationProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProgramCode { get; set; }
        public bool IsCertified { get; set; }
        public string Grade { get; set; }

        public ICollection<Thesis> Theses { get; set; }
        public ICollection<EducationProgramResearchLine> EducationProgramResearchLine { get; set; }
    }
}
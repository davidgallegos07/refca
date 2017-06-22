using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class EducationProgramResearchLine
    {
        public int EducationProgramId { get; set; }
        public EducationProgram EducationProgram { get; set; }
       
        public int ResearchLineId { get; set; }
        public ResearchLine ResearchLine { get; set; }

    }
}
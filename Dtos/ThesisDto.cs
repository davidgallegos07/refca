using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Dtos
{
    public class ThesisDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StudentName { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ThesisPath { get; set; }
        public bool IsApproved { get; set; }
        public EducationProgramDto EducationProgram { get; set; }  
        public ResearchLineDto ResearchLine { get; set; }        

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Resources.TeacherResources
{
    public class TeacherThesisResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StudentName { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ThesisPath { get; set; }
        public bool IsApproved { get; set; }
        public EducationProgramResource EducationProgram { get; set; }  
        public ResearchLineResource ResearchLine { get; set; }        
        public ICollection<TeacherProductivityResource> TeacherTheses { get; set; }
    }
}
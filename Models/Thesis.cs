using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class Thesis
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StudentName { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime AddedDate { get; set; } 
        public DateTime UpdatedDate { get; set; } 
        public string ThesisPath { get; set; }
        public bool IsApproved { get; set; }

        public int EducationProgramId { get; set; }
        public EducationProgram EducationProgram { get; set; }  
        public int ResearchLineId { get; set; }
        public ResearchLine ResearchLine { get; set; }

        public ICollection<TeacherThesis> TeacherTheses { get; set; }
    }
}
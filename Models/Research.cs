using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class Research
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string ResearchType { get; set; }
        public string Sector { get; set; }
        public byte ResearchDuration { get; set; }
        public string InitialPeriod { get; set; }
        public string FinalPeriod { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsApproved { get; set; }
        public string ResearchPath { get; set; }

        public int KnowledgeAreaId { get; set; } 
        public KnowledgeArea KnowledgeArea { get; set; }
        public int AcademicBodyId { get; set; }
        public AcademicBody AcademicBody { get; set; } 
        public int ResearchLineId { get; set; }
        public ResearchLine ResearchLine { get; set; }
        public ICollection<TeacherResearch> TeacherResearch { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Resources
{
    public class ResearchResource
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

        public KnowledgeAreaResource KnowledgeArea { get; set; }
        public AcademicBodyResource AcademicBody { get; set; }
        public ResearchLineResource ResearchLine { get; set; }

        public ICollection<SimpleTeacherResource> TeacherResearch { get; set; }

    }
}
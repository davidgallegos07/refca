using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Dtos
{
    public class ResearchDto
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

        public KnowledgeAreaDto KnowledgeArea { get; set; }
        public AcademicBodyDto AcademicBody { get; set; }
        public ResearchLineDto ResearchLine { get; set; }

    }
}
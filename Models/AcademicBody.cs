using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class AcademicBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PromepCode { get; set; }
        public byte ConsolidationGradeId { get; set; }
        public ConsolidationGrade ConsolidationGrade { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Research> Research { get; set; }
        public ICollection<ResearchLine> ResearchLines { get; set; }
    }
}
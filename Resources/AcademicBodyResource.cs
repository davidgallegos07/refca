using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Resources
{
    public class AcademicBodyResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PromepCode { get; set; }
        public byte ConsolidationGradeId { get; set; }
        public ConsolidationGradeResource ConsolidationGrade { get; set; }

    }
}
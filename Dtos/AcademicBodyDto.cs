using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Dtos
{
    public class AcademicBodyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PromepCode { get; set; }
        public byte ConsolidationGradeId { get; set; }
        public ConsolidationGradeDto ConsolidationGrade { get; set; }

    }
}
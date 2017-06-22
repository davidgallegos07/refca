using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Dtos
{
    public class EducationProgramDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProgramCode { get; set; }
        public bool IsCertified { get; set; }
        public string Grade { get; set; }
    }
}
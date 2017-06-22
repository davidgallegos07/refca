using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Dtos
{
    public class PresentationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CongressName { get; set; }
        public DateTime EditionDate { get; set; }
        public bool IsApproved { get; set; }
        public string PresentationPath { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class Presentation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Congress { get; set; }
        public DateTime EditionDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsApproved { get; set; }
        public string PresentationPath { get; set; }
        public string Owner { get; set; }

        public ICollection<TeacherPresentation> TeacherPresentations { get; set; }
    }
}
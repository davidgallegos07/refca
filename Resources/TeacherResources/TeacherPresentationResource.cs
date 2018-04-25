using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Resources.TeacherResources
{
    public class TeacherPresentationResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Congress { get; set; }
        public DateTime EditionDate { get; set; }
        public bool IsApproved { get; set; }
        public string PresentationPath { get; set; }

        public ICollection<TeacherProductivityResource> TeacherPresentations { get; set; }
    }
}
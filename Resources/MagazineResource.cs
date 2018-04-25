using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Resources
{
    public class MagazineResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Index { get; set; }
        public int ISSN { get; set; }
        public string Editor { get; set; }
        public short Edition { get; set; }
        public DateTime AdddedDate { get; set; }
        public DateTime EditionDate { get; set; }
        public bool IsApproved { get; set; }
        public string MagazinePath { get; set; }

        public ICollection<SimpleTeacherResource> TeacherMagazines { get; set; }
    }
}
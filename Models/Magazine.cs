using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class Magazine
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ISSN { get; set; }
        public string Index { get; set; }
        public string Editor { get; set; }
        public short Edition { get; set; }
        public DateTime EditionDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsApproved { get; set; }
        public string MagazinePath { get; set; }
        public string Owner { get; set; }

        public ICollection<TeacherMagazine> TeacherMagazines { get; set; }
    }
}
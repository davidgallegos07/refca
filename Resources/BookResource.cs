using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Resources
{
    public class BookResource
    {        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public DateTime EditionDate { get; set; } 
        public DateTime AddedDate { get; set; }
        public short Year { get; set; }
        public string ISBN { get; set; }
        public short Edition { get; set; } 
        public string Editorial { get; set; }
        public int PrintLength { get; set; }
        public string Genre { get; set; }
        public bool IsApproved { get; set; }
        public string BookPath { get; set; }

        public ICollection<SimpleTeacherResource> TeacherBooks { get; set; }
    }
}
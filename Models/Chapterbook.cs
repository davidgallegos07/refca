using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class Chapterbook
    {        
        public int Id { get; set; }
        public string Title { get; set; }
        public string BookTitle { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime AddedDate { get; set; } 
        public DateTime UpdatedDate { get; set; }
        public string ISBN { get; set; }
        public string Editorial { get; set; }
        public string ChapterbookPath { get; set; }
        public bool IsApproved { get; set; }
        public string Owner { get; set; }

        public ICollection<TeacherChapterbook> TeacherChapterbooks { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Magazine { get; set; }
        public int ISSN { get; set; }
        public DateTime EditionDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ArticlePath { get; set; }
        public bool IsApproved { get; set; }
        public string Owner { get; set; }

        public ICollection<TeacherArticle> TeacherArticles { get; set; }
    }
}
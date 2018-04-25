using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class TeacherArticle
    {
        public string TeacherId { get; set; }
        public int ArticleId { get; set; }
        public int Order { get; set; }
        public string Role { get; set; }
        public Teacher Teacher { get; set; }
        public Article Article { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Dtos
{
    public class ArticleWithTeachersDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Magazine { get; set; }
        public int ISSN { get; set; }
        public DateTime EditionDate { get; set; }
        public bool IsApproved { get; set; }
        public string ArticlePath { get; set; }

        public ICollection<TeacherDto> TeacherArticles { get; set; }
    }
}
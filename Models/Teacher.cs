using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class Teacher
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int TeacherCode { get; set; }
        public string Email { get; set; }
        public bool SNI { get; set; }
        public bool HasProdep { get; set; }
        public bool IsResearchTeacher { get; set; }
        public string CVPath { get; set; }
        public string Avatar { get; set; }
        public string WebSite { get; set; }
        public string FacebookProfile { get; set; }
        public string TwitterProfile { get; set; }
        public string Biography { get; set; }

        public byte? LevelId { get; set; } 
        public Level Level { get; set; }
        public int KnowledgeAreaId { get; set; }
        public KnowledgeArea KnowledgeArea { get; set; }
        public int AcademicBodyId { get; set; } 
        public AcademicBody AcademicBody { get; set; }

        public ICollection<TeacherThesis> TeacherTheses { get; set; }
        public ICollection<TeacherBook> TeacherBooks { get; set; }
        public ICollection<TeacherChapterbook> TeacherChapterbooks { get; set; }
        public ICollection<TeacherResearch> TeacherResearch { get; set; }
        public ICollection<TeacherArticle> TeacherArticles { get; set; }
        public ICollection<TeacherPresentation> TeacherPresentations { get; set; }
        public ICollection<TeacherMagazine> TeacherMagazines { get; set; }
    }
}
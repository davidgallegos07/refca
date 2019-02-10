using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Resources
{
    public class TeacherResource
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int TeacherCode { get; set; }
        public string Email { get; set; }
        public bool SNI { get; set; }
        public string Level { get; set; } 
        public bool HasProdep { get; set; }
        public bool IsResearchTeacher { get; set; }
        public string Avatar { get; set; }
        public string WebSite { get; set; }
        public string FacebookProfile { get; set; }
        public string TwitterProfile { get; set; }
        public string Biography { get; set; }
        public string CVPath {get;set;}
        public KnowledgeAreaResource KnowledgeArea { get; set; }
        public AcademicBodyResource AcademicBody { get; set; }
    }
}
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.TeacherViewModels
{
    public class TeacherCurriculumViewModel
    {
        [StringLength(450)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El curriculum es requerido")]
        [Display(Name = "Curriculum")]
        public IFormFile CVFile { get; set; }

        public string CVPath { get; set; }
           
    }
}
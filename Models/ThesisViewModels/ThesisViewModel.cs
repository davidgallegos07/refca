using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.ThesisViewModels
{
    public class ThesisViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Nombre del alumno")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha de publicación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishedDate { get; set; }

        [Display(Name = "Archivo de tesis")]
        public IFormFile ThesisFile { get; set; }

        [Required(ErrorMessage = "El programa educativo es requerido")]
        [Display(Name = "Programa educativo")]
        public int EducationProgramId { get; set; }

        [Required(ErrorMessage = "La línea de investigación es requerida")]
        [Display(Name = "Línea de investigación")]
        public int ResearchLineId { get; set; }

        public IEnumerable<EducationProgram> EducationPrograms { get; set; }
        public IEnumerable<ResearchLine> ResearchLines { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
        public List<string> TeacherIds { get; set; }
        public ThesisViewModel()
        {
            TeacherIds = new List<string>();
        }
    }
 
}
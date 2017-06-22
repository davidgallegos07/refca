using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.PresentationViewModels
{
    public class PresentationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "Minimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El congreso es requerido")]
        [StringLength(100, ErrorMessage = "Minimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Congreso")]
        public string Congress { get; set; }

        [Required(ErrorMessage = "La fecha de edición es requerida")]
        [Display(Name = "Fecha de edición")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EditionDate { get; set; } 

        [Display(Name = "Archivo de ponencia")]
        public IFormFile PresentationFile { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
        public List<string> TeacherIds { get; set; }
        public PresentationViewModel()
        {
            TeacherIds = new List<string>();
        }
    }
 
}
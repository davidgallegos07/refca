using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.ArticleViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "Minimo {2} caracteres, Maximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El título de revista es requerido")]
        [StringLength(100, ErrorMessage = "Minimo {2} caracteres, Maximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Título de revista")]
        public string Magazine { get; set; }

        [Required(ErrorMessage = "La fecha de edición es requerida")]
        [Display(Name = "Fecha de edición")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EditionDate { get; set; } 

        [Required(ErrorMessage = "El ISSN es requerido")]
        [Range(1, 99999999, ErrorMessage = "Ingresa un numero entero valido")]
        [Display(Name = "ISSN")]
        public int ISSN { get; set; }

        [Display(Name = "Archivo de capítulo de libro")]
        public IFormFile ArticleFile { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
        public List<string> TeacherIds { get; set; }
        public ArticleViewModel()
        {
            TeacherIds = new List<string>();
        }
    }
 
}
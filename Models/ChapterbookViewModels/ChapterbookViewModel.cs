using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.ChapterbookViewModels
{
    public class ChapterbookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El título de libro es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Título de libro")]
        public string BookTitle { get; set; }

        [Required(ErrorMessage = "La fecha de publicación es requerida")]
        [Display(Name = "Fecha de publicación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishedDate { get; set; } 

        [Required(ErrorMessage = "El ISBN es requerido")]
        [StringLength(13, ErrorMessage = "Inserte ISBN válido", MinimumLength = 10)]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "La editorial es requerida")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Editorial")]
        public string Editorial { get; set; }

        [Display(Name = "Archivo de capítulo de libro")]
        public IFormFile ChapterbookFile { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
        public List<string> TeacherIds { get; set; }
        public ChapterbookViewModel()
        {
            TeacherIds = new List<string>();
        }
    }
 
}
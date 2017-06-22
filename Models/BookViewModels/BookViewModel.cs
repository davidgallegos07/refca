using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.BookViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El abstract es requerido")]
        [StringLength(450, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Abstract")]
        public string Abstract { get; set; }

        [Required(ErrorMessage = "La fecha de edición es requerida")]
        [Display(Name = "Fecha de edición")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EditionDate { get; set; }

        [Required(ErrorMessage = "El año es requerido")]
        [Display(Name = "Año")]
        public short Year { get; set; }

        [Required(ErrorMessage = "El ISBN es requerido")]
        [StringLength(13, ErrorMessage = "Inserte ISBN válido", MinimumLength = 10)]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "El número de edición es requerido")]
        [Range(1, short.MaxValue, ErrorMessage = "Ingresa un número entero válido")]
        [Display(Name = "Número de edición")]
        public short Edition { get; set; } 

        [Required(ErrorMessage = "La editorial es requerida")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Editorial")]
        public string Editorial { get; set; }

        [Required(ErrorMessage = "El número de páginas es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Ingresa un número entero válido")]
        [Display(Name = "Número de páginas")]
        public int PrintLength { get; set; }

        [Required(ErrorMessage = "El género es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Género")]
        public string Genre { get; set; }

        [Display(Name = "Archivo de libro")]
        public IFormFile BookFile { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
        public List<string> TeacherIds { get; set; }
        public BookViewModel()
        {
            TeacherIds = new List<string>();
        }
    }
 
}
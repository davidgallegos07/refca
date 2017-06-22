using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.MagazineViewModels
{
    public class MagazineViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El Índice es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 1)]
        [Display(Name = "Índice")]
        public string Index { get; set; }

        [Required(ErrorMessage = "El ISSN es requerido")]
        [Range(1, 99999999, ErrorMessage = "Inserte ISSN válido")]
        [Display(Name = "ISSN")]
        public int ISSN { get; set; }

        [Required(ErrorMessage = "El editor es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Editor")]
        public string Editor { get; set; }

        [Required(ErrorMessage = "La fecha de edición es requerida")]
        [Display(Name = "Fecha de edición")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EditionDate { get; set; }

        [Required(ErrorMessage = "El número de edición es requerido")]
        [Range(1, short.MaxValue, ErrorMessage = "Ingresa un número entero válido")]
        [Display(Name = "Número de edición")]
        public short Edition { get; set; } 

        [Display(Name = "Archivo de revista")]
        public IFormFile MagazineFile { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
        public List<string> TeacherIds { get; set; }
        public MagazineViewModel()
        {
            TeacherIds = new List<string>();
        }
    }
 
}
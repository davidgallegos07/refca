using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace refca.Models.AccountViewModels
{
    public class EditAcademicBodyViewModel
    {
        [Required(ErrorMessage="El nombre del cuerpo académico es requerido")]
        [StringLength(100, ErrorMessage="Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string Name {get; set;}

        [Required(ErrorMessage="El código Promep es requerido")]
        [StringLength(100, ErrorMessage="Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 2)]
        [Display(Name = "Código Promep")]
        public string PromepCode {get; set;}

        [Required(ErrorMessage="El grado de consolidación es requerido")]
        [Display(Name = "Grado de consolidación")]
        public byte ConsolidationGradeId {get; set;}

        public int Id {get ; set;}
    }
}

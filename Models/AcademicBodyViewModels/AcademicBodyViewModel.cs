using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace refca.Models.AcademicBodyViewModels
{
    public class AcademicBodyViewModel
    {
        [Required(ErrorMessage="El nombre del cuerpo academico es requerido")]
        [StringLength(100, ErrorMessage="Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string Name {get; set;}

        [Required(ErrorMessage="El PromepCode es requerido")]
        [StringLength(100, ErrorMessage="Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 2)]
        [Display(Name = "PromepCode")]
        public string PromepCode {get; set;}

        [Required(ErrorMessage="El ConsolidationGrade es requerido")]
        [Display(Name = "ConsolidationGrade")]
        public byte ConsolidationGradeId {get; set;}


    }
}

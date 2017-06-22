using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.TeacherViewModels
{
    public class EditTeacherViewModel
    {
        [StringLength(450)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El numero de empleado es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingresa un numero entero valido")]
        [Display(Name = "Número de Empleado")]
        public int TeacherCode { get; set; }

        [Required(ErrorMessage = "El SNI es requerido")]
        [Display(Name = "SNI")]
        public bool SNI { get; set; }

        [Required(ErrorMessage = "El PRODEP es requerido")]
        [Display(Name = "Perfil PRODEP")]
        public bool HasProdep { get; set; }

        [Required(ErrorMessage = "El docente investigador es requerido")]
        [Display(Name = "Docente investigador")]
        public bool IsResearchTeacher { get; set; }

        [Required(ErrorMessage = "El área de conocimiento es requerida")]
        [Display(Name = "Área de conocimiento")]
        public int KnowledgeAreaId { get; set; }

        [Required(ErrorMessage = "El cuerpo académico es requerido")]
        [Display(Name = "Cuerpo académico")]
        public int AcademicBodyId { get; set; }

        [Required(ErrorMessage = "El nivel es requerido")]
        [Display(Name = "Nivel")]
        public byte? LevelId { get; set; }

    }
}
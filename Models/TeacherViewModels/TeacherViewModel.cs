using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace refca.Models.TeacherViewModels
{
    public class TeacherViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(255, ErrorMessage = "Crear contraseña con al menos {2} caracteres y un máximo de {1}", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme contraseña")] 
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El número de empleado es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingresa un número entero válido")]
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

        [Required(ErrorMessage = "La área de conocimiento es requerida")]
        [Display(Name = "Área de conocimiento")]
        public int KnowledgeAreaId { get; set; }

        [Required(ErrorMessage = "El cuerpo académico es requerido")]
        [Display(Name = "Cuerpo académico")]
        public int AcademicBodyId { get; set; }

        [Required(ErrorMessage = "El nivel es requerido")]
        [Display(Name = "Nivel")]
        public byte LevelId { get; set; }

    }
}
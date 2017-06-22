using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.ResearchViewModels
{
    public class ResearchViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres,  Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "La clave es requerida")]
        [StringLength(100, ErrorMessage = " Máximo {1} caracteres")]
        [Display(Name = "Clave")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El tipo de investigación es requerida")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres,  Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Tipo de investigación")]
        public string ResearchType { get; set; }

        [Required(ErrorMessage = "El sector es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres,  Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Required(ErrorMessage = "Duración del proyecto")]
        [Range(1, byte.MaxValue, ErrorMessage = "Ingresa un numero entero valido")]
        [Display(Name = "Duración del proyecto")]
        public byte ResearchDuration { get; set; }
        
        [Required(ErrorMessage = "El periodo inicial es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres,  Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Periodo Inicial")]
        public string InitialPeriod { get; set; }

        [Required(ErrorMessage = "El periodo final es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres,  Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Periodo final")]
        public string FinalPeriod { get; set; }

        [Display(Name = "Archivo de Investigación")]
        public IFormFile ResearchFile { get; set; }

        [Required(ErrorMessage = "La línea de investigación es requerida")]
        [Display(Name = "Línea de investigación")]
        public int ResearchLineId { get; set; }

        [Required(ErrorMessage = "El cuerpo académico es requerido")]
        [Display(Name = "Cuerpo académico")]
        public int AcademicBodyId { get; set; }

        [Required(ErrorMessage = "La área de conocimiento es requerida")]
        [Display(Name = "Área de conocimiento")]
        public int KnowledgeAreaId { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
        public List<string> TeacherIds { get; set; }
        public ResearchViewModel()
        {
            TeacherIds = new List<string>();
        }
    }
 
}
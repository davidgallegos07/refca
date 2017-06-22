using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.TeacherViewModels
{
    public class TeacherProfileViewModel
    {
        [StringLength(450)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "Mínimo {2} caracteres, Máximo {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [RegularExpression(@"https:\/\/gravatar\.com\/avatar\/[a-f0-9]{32}", ErrorMessage = "Gravatar inválido")]
        public string Avatar { get; set; }

        [Url(ErrorMessage = "Url invalido")]
        [Display(Name = "Sitio web")]
        public string WebSite { get; set; }

        [RegularExpression(@"https:\/\/www\.facebook\.com\/[a-zA-Z0-9.]{1,50}", ErrorMessage = "Perfil de facebook inválido")]
        [Display(Name = "Facebook")]        
        public string FacebookProfile { get; set; } 

        [RegularExpression(@"https:\/\/twitter\.com\/[a-zA-Z0-9_]{1,15}", ErrorMessage = "Perfil de twitter inválido")]
        [Display(Name = "Twitter")]                
        public string TwitterProfile { get; set; }

        [StringLength(200, ErrorMessage = "Máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Biografia")]                
        public string Biography { get; set; }
           
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerdida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "No salir de la cuenta")]
        public bool RememberMe { get; set; }
    }
}
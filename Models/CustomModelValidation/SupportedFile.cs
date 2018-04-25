using System.ComponentModel.DataAnnotations;
using refca.Models.ArticleViewModels;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace refca.Models.CustomModelValidation
{
    public class SupportedFile : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;

            if(file == null 
            || file.Length > 31457280
            || file.Length == 0
            || !file.FileName.EndsWith(".pdf")) return new ValidationResult("Archivo no soportado");

            return ValidationResult.Success;
        }

    }
}
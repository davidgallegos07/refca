using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using refca.Models.CustomModelValidation;
namespace refca.Models.FileViewModel
{
    public class FileViewModel
    {
        public int Id { get; set; }
        
        [SupportedFile]
        [Display(Name = "Archivo")]
        public IFormFile File { get; set; }
        public string ControllerName { get; set; }
    }
}
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace refca.Core
{
    public class FileProductivityService : IFileProductivityService
    {
        private readonly IFileProductivityStorage productivityStorage;
        public FileProductivityService(IFileProductivityStorage productivityStorage)
        {
            this.productivityStorage = productivityStorage;
        }

        public void Move(string sourcePath, string destPath)
        {
            var fileUploadPath = Path.GetDirectoryName(destPath);
            if (!Directory.Exists(fileUploadPath))
                Directory.CreateDirectory(fileUploadPath);

            System.IO.File.Move(sourcePath, destPath);
        }
        public void Remove(string filePath)
        {
            if (Directory.Exists(filePath))
                System.IO.File.Delete(filePath);
        }

        public async Task<string> Storage(string uploadFilePath, IFormFile file)
        {
            var fileName = await productivityStorage.StorageProductivity(uploadFilePath, file);
            return fileName;
        }

    }
}
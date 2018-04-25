using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace refca.Core
{
    public interface IFileProductivityService
    {
        Task<string> Storage(string bucketPath, IFormFile file);
        void Move(string sourcePath, string destPath);
        void Remove(string filePath);
    }
}
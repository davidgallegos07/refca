using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace refca.Core
{
    public interface IFileProductivityStorage
    {
        Task<string> StorageProductivity(string bucketPath, IFormFile file);

    }
}
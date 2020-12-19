using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.Core.Interfaces.Storage
{
    public interface IStorageService
    {
        /// <summary>
        ///     Upload a file and returns the full path to retrieve the file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        Task<string> UploadFile(IFormFile file, string fullPath);

        /// <summary>
        ///     Get the file stream by full path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<Stream> GetFileStream(string filePath);
    }
}

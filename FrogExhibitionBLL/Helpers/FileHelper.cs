using FrogExhibitionBLL.Interfaces.IHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Office.Interop.Excel;

namespace FrogExhibitionBLL.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _environment;

        public FileHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        /// <summary>
        /// Takes file name and concat it with 4 symbols from generated Guid
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <returns>Unique file name</returns>
        public string GetUniqueFileName(string fileName)
        {
     
            fileName = Path.GetFileName(fileName);
            return string.Concat(Path.GetFileNameWithoutExtension(fileName),
                "_",
                Guid.NewGuid().ToString().AsSpan(0, 4),
                Path.GetExtension(fileName));

        }

        /// <summary>
        /// Takes exhibihion name and concat it with date
        /// </summary>
        /// <param name="exhibitionName"></param>
        /// <returns></returns>
        public string GetExhibitionReportFilePath(string exhibitionName)
        {
            return string.Concat(
                _environment.WebRootPath,
                "\\reports\\",
                exhibitionName.Replace(" ", "_"),
                "_",
                DateTime.Now.ToShortDateString().Replace(".","_"),
                ".xlsx");
        }
        /// <summary>
        /// Saves file with unique name in wwwroot directory
        /// </summary>
        /// <param name="file">IFormFile to save</param>
        /// <returns>Path to the saved file</returns>
        public async Task<string> SavePhotoAsync(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "images");
            return await SavePhotoAsync(file, uploads);
        }
        /// <summary>
        /// Saves file with unique name in specified directory
        /// </summary>
        /// <param name="file">IFormFile to save</param>
        /// <param name="directoryPath">Path to the directory where file will be saved</param>
        /// <returns>Path to the saved file</returns>
        public async Task<string> SavePhotoAsync(IFormFile file, string directoryPath)
        {
            var uniqueFileName = GetUniqueFileName(file.FileName);
            var filePath = Path.Combine(directoryPath, uniqueFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            return filePath;
        }


    }
}

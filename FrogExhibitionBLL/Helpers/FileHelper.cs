using FrogExhibitionBLL.Constants;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                FileRelatedConstants.WordSeparator,
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
            return Path.Combine(
                _environment.WebRootPath,
                FileRelatedConstants.ReportsFolderName,
                FileRelatedConstants.ExhibitionReportsFolderName,
                string.Concat(
                    exhibitionName.Replace(" ", FileRelatedConstants.WordSeparator),
                    FileRelatedConstants.WordSeparator,
                    DateTime.Now.ToShortDateString().Replace(".", FileRelatedConstants.WordSeparator),
                    FileRelatedConstants.ReportFileExtension)
                );
        }

        public string GetUserReportFilePath(string userName)
        {
            return Path.Combine(
                _environment.WebRootPath,
                FileRelatedConstants.ReportsFolderName,
                FileRelatedConstants.UserReportsFolderName,
                string.Concat(
                    userName.Replace(" ", FileRelatedConstants.WordSeparator),
                    FileRelatedConstants.WordSeparator,
                    DateTime.Now.ToShortDateString().Replace(".", FileRelatedConstants.WordSeparator),
                    FileRelatedConstants.ReportFileExtension)
                );
        }

        /// <summary>
        /// Saves file with unique name in wwwroot directory
        /// </summary>
        /// <param name="file">IFormFile to save</param>
        /// <returns>Path to the saved file</returns>
        public async Task<string> SavePhotoAsync(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, FileRelatedConstants.ImagesFolderName);
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

        public async Task<FileContentResult> GetFileContentResultAsync(string filePath, string contentType, bool deleteFileAfter = false)
        {
            if (!File.Exists(filePath))
            {
                throw new NotFoundException("Something went wrong");
            }
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            string fileName = Path.GetFileName(filePath);
            var fileContentResult = new FileContentResult(fileBytes, contentType)
            {
                FileDownloadName = fileName
            };
            if (deleteFileAfter)
            {
                File.Delete(filePath);
            }

            return fileContentResult;
        }


    }
}

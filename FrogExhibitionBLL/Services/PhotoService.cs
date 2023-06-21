using FrogExhibitionBLL.Helpers;
using FrogExhibitionBLL.Interfaces.IService;

namespace FrogExhibitionBLL.Services
{
    public class PhotoService : IPhotoService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PhotoService> _logger;
        private readonly IWebHostEnvironment _environment;

        public PhotoService(IUnitOfWork unitOfWork, ILogger<PhotoService> logger, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _environment = environment;
        }

        public async Task<string> SavePhotoAsync(IFormFile file)
        {
            var uniqueFileName = FileHelper.GetUniqueFileName(file.FileName);
            var uploads = Path.Combine(_environment.WebRootPath, "images");
            var filePath = Path.Combine(uploads, uniqueFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            return filePath;
        }
    }
}

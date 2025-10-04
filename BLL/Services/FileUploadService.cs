using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BLL.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadEventImageAsync(IFormFile imageFile);
        Task<bool> DeleteEventImageAsync(string imageUrl);
        bool IsValidImageFile(IFormFile file);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private const string EventImagesFolder = "images/events";

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadEventImageAsync(IFormFile imageFile)
        {
            if (!IsValidImageFile(imageFile))
                throw new ArgumentException("Invalid image file");

            // Create unique filename
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            // Ensure directory exists
            var uploadsFolder = Path.Combine(_environment.WebRootPath, EventImagesFolder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Save file
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return relative URL
            return $"/{EventImagesFolder}/{uniqueFileName}";
        }

        public Task<bool> DeleteEventImageAsync(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return Task.FromResult(true);

                // Extract filename from URL
                var fileName = Path.GetFileName(imageUrl);
                var filePath = Path.Combine(_environment.WebRootPath, EventImagesFolder, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }
            }
            catch
            {
                // Log error if needed
            }
            return Task.FromResult(false);
        }

        public bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length > MaxFileSize)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }
    }
}
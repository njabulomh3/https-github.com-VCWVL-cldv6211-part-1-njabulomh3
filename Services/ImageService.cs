using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EventEase.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string subFolder)
        {
            // 1. Create the full path to the images folder
            string imagesFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", subFolder);

            // 2. Create directory if it doesn't exist
            Directory.CreateDirectory(imagesFolder);

            // 3. Generate unique filename
            string uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            string fullPath = Path.Combine(imagesFolder, uniqueFileName);

            // 4. Save the file
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // 5. Return relative path for database storage
            return $"/images/{subFolder}/{uniqueFileName}";
        }

        public void DeleteImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                string fullPath = Path.Combine(_hostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }
    }
}



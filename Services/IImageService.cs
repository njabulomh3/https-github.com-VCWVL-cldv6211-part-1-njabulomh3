using Microsoft.AspNetCore.Http;

namespace EventEase.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string subFolder);
        void DeleteImage(string imagePath);
    }
}

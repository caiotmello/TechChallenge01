using ImageUploaderWebMVC.Models;

namespace ImageUploaderWebMVC.Services
{
    public interface IImageService
    {
        Task AddImageAsync(Image image);
        Task DeleteImageAsync(int id);
        Task UpdateImageAsync(int id, Image image);
        Task<Image> GetImageByIdAsync(int id);
        Task<IEnumerable<Image>> GetImagesAsync();
    }
}

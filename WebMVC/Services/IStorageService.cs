using Microsoft.EntityFrameworkCore;

namespace ImageUploaderWebMVC.Services
{
    public interface IStorageService
    {
        Task<(Uri, string)> Upload(IFormFile formFile);
        Task<Uri> UploadBase64(string base64Image);
        Task<bool> DeleteAsync(string blobFileName);
    }
}

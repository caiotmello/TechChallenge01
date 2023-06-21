using ImageUploaderWebMVC.Models;
using ImageUploaderWebMVC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImageUploaderWebMVC.Services
{
    public class ImageService : IImageService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IStorageService _storageService;
        readonly ILogger _logger;

        public ImageService(IUnitOfWork unitOfWork, IStorageService storageService, ILogger<ImageService> logger)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _logger = logger;
        }

        public async Task AddImageAsync(Image image)
        {
            try
            {
                //Add Image to Azure Blob
                (image.Url, image.BlobName) = await _storageService.Upload(image.ImageFile);

                //Add Images to database
                var result = await _unitOfWork.Images.AddAsync(image); ;
                _unitOfWork.Complete();

                if (result == null)
                    throw new ApplicationException("Failed to add image to Database");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task DeleteImageAsync(int id)
        {
            if (_unitOfWork.Images == null)
                throw new ApplicationException("Entity set 'AppDbContext.Images'  is null.");

            var image = await _unitOfWork.Images.GetAsync(id);
            if (image != null)
            {
                if (await _storageService.DeleteAsync(image.BlobName))
                {
                    await _unitOfWork.Images.RemoveAsync(image);
                }
                else
                {
                    throw new ApplicationException("Error to delete image from Azure Blob");
                }
                _unitOfWork.Complete();
                _logger.LogDebug($"Image {image.Id} deleted from database");
            }
            else
            {
                throw new ApplicationException("Image not found on Database");
            }
        }
        public async Task UpdateImageAsync(int id, Image image)
        {
            if (id != image.Id)
                throw new ApplicationException("Image not found");

            try
            {
                await _unitOfWork.Images.UpdateAsync(image);
                _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ImageExistsAsync(image.Id))
                {
                    throw new ApplicationException("Image not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            if (_unitOfWork.Images == null)
                throw new ApplicationException("Entity set 'AppDbContext.Images' is null.");

            var images = await _unitOfWork.Images.GetAllAsync();
            return images;

        }
        public async Task<Image> GetImageByIdAsync(int id)
        {
            if (id == null || _unitOfWork.Images == null)
                throw new ArgumentNullException();

            var image = await _unitOfWork.Images.GetAsync(id);
            if (image == null)
                throw new ApplicationException("Failed to get image from database");

            return image;
        }
        private async Task<bool> ImageExistsAsync(int id)
        {
            var image = await _unitOfWork.Images?.FindAsync(e => e.Id == id);
            return image.Count() != 0;
        }
    }
}

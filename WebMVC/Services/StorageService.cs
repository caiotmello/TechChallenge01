using Azure.Storage.Blobs;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Azure;
using Azure.Storage.Blobs.Models;
using ImageUploaderWebMVC.Repositories;

namespace ImageUploaderWebMVC.Services
{
    public class StorageService : IStorageService
    {
        private readonly ILogger<StorageService> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public StorageService(ILogger<StorageService> logger, BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }

        public async Task<(Uri, string)> Upload(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                _logger.LogError($"[{DateTime.UtcNow.ToLongTimeString()}] - Image file empty)");
                throw new ApplicationException("No Image file found.");
            }

            try
            {
                var containerName = _configuration.GetSection("AzureStorage:ContainerName").Value;
                _logger.LogDebug($"[{DateTime.UtcNow.ToLongTimeString()}] - blob Storage container name [{containerName}]");

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);



                // Upload File
                using (Stream stream = formFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                //Return File URL
                return (blobClient.Uri, fileName);
            }
            catch
            {
                _logger.LogError($"[{DateTime.UtcNow.ToLongTimeString()}] - Failed Upload file to blob Storage");
                throw new ApplicationException("Failed Upload file to blob Storage");
            }

        }

        /// <summary>
        /// Upload base64 files to blob storage
        /// </summary>
        /// <param name="base64Image"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Uri> UploadBase64(string base64Image)
        {
            if (base64Image == null)
            {
                _logger.LogError($"[{DateTime.UtcNow.ToLongTimeString()}] - Image file empty)");
                throw new ApplicationException("No Image file found.");
            }

            try
            {
                //Generate randon name to the image
                var fileName = Guid.NewGuid().ToString() + ".jpg";

                // Clean hash sent
                var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");

                //Generate one array of bytes
                byte[] imageBytes = Convert.FromBase64String(data);
                _logger.LogDebug($"[{DateTime.UtcNow.ToLongTimeString()}] - Array of bytes [{imageBytes}]");

                //Definition of blob where the file will be storaged
                var containerName = _configuration.GetSection("AzureStorage:ContainerName").Value;
                _logger.LogDebug($"[{DateTime.UtcNow.ToLongTimeString()}] - blob Storage container name [{containerName}]");

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                //Upload File
                using (Stream stream = new MemoryStream(imageBytes))
                {
                    await blobClient.UploadAsync(stream, true);
                }

                //return file URL
                return blobClient.Uri;
            }
            catch
            {
                _logger.LogError($"[{DateTime.UtcNow.ToLongTimeString()}] - Failed to connect to blob Storage");
                throw new ApplicationException("File to connect to blob Storage");
            }
        }

        /// <summary>
        /// Detele blob from Azure
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteAsync(string blobFileName)
        {
            var containerName = _configuration.GetSection("AzureStorage:ContainerName").Value;
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobFileName);

            try
            {
                //Delete blob file
                var response = await blobClient.DeleteIfExistsAsync();
                return response;
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                //File did not exit, log to console and return new response to requesting method
                _logger.LogError($"File {blobFileName} does not exist");
                return false;
            }

        }
    }
}

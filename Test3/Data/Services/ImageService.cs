using Microsoft.AspNetCore.Components.Forms;
using System.Net;

namespace Test3.Data.Services
{
    public class ImageService
    {
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string[] _allowedExtensions = { ".png", ".jpg", ".jpeg" };

        public async Task<(byte[] data, string contentType, string fileName)> ProcessUploadAsync(IBrowserFile file)
        {
            // Validate file size
            if (file.Size > _maxFileSize)
            {
                throw new InvalidOperationException($"File size cannot exceed {_maxFileSize / 1024 / 1024}MB");
            }

            // Validate file extension
            var fileExtension = Path.GetExtension(file.Name).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Only PNG, JPG, and JPEG files are allowed.");
            }

            // Read file into memory
            using var memoryStream = new MemoryStream();
            await file.OpenReadStream(_maxFileSize).CopyToAsync(memoryStream);
            var fileData = memoryStream.ToArray();

            // Determine content type
            var contentType = fileExtension switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };

            return (fileData, contentType, file.Name);
        }

        public string GetImageSrc(byte[]? imageData, string contentType)
        {
            if (imageData == null || imageData.Length == 0)
                return string.Empty;

            return $"data:{contentType};base64,{Convert.ToBase64String(imageData)}";
        }
    }
}

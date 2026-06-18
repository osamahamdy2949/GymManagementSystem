using GymManagement.BLL.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Attachments
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly long _attachmentSize = 5*1024*1024;
        private readonly ILogger<AttachmentServices> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions = { ".png", ".jpeg",".jpg"};

        public AttachmentServices(ILogger<AttachmentServices> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }
        public Result Delete(string fileName, string folderName)
        {
            var fullPath = Path.Combine(_env.ContentRootPath, folderName, fileName);


            try
            {
                if (!File.Exists(fullPath)) return Result.NotFound($"Path {fullPath} Not Found");

                File.Delete(fullPath);
                return Result.Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed To Delete Attachment With File Path {fullPath}");
                return Result.Fail($"Failed To Delete Attachment With File Path {fullPath}");
            }
        }

        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName))
                return null;

            var fullPath = Path.Combine(_env.ContentRootPath, folderName, fileName);

            if (!File.Exists(fullPath)) return null;

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);

            var fileExtention = Path.GetExtension(fullPath).ToLower();

            var contentType = fileExtention switch
            {
                ".png" => "image/png",
                ".jpg" or "jpeg" => "image/jpeg",
                _ => null
            };

            return (stream, contentType)!;
        }

        public async Task<Result<string?>> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead)
                return Result<string?>.Fail("Invalid file stream.");

            if (fileStream.Length == 0)
                return Result<string?>.Fail("File is empty.");

            if (fileStream.Length > _attachmentSize)
            {
                _logger.LogError($"Attachment size > {_attachmentSize}");

                return Result<string?>.Fail($"Attachment size > {_attachmentSize}");
            }

            var fileExtension = Path.GetExtension(fileName);

            if (string.IsNullOrWhiteSpace(fileExtension) ||
                !_allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
            {
                _logger.LogError($"File rejected: extension {fileExtension} not allowed");

                return Result<string?>.Fail($"Extension {fileExtension} is not allowed.");
            }

            var uploadsFolder = Path.Combine(_env.ContentRootPath, folderName);

            Directory.CreateDirectory(uploadsFolder);

            var storedFileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(uploadsFolder, storedFileName);

            try
            {
                using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(stream, ct);

                return Result<string?>.Ok(storedFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to upload file {fileName}");

                return Result<string?>.Fail($"Failed to upload file {fileName}");
            }
        }
    }
}

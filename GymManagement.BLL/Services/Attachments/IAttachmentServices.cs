using GymManagement.BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Attachments
{
    public interface IAttachmentServices
    {
        Task<Result<string?>> UploadAsync(Stream fileStream , string fileName , string foldername, CancellationToken ct =default);
        Result Delete(string fileName, string folderName);
        (Stream stream, string contentType)? GetFile(string fileName, string folderName);
    }
}

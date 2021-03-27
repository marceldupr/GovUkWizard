using System;
using System.IO;
using Dfe.Wizard.Prototype.Models;

namespace Dfe.Wizard.Prototype.Application.Services
{
    public interface IFileUploadService
    {
        FileUploadResult UploadFile(Stream file, string filename, string mimeType, string folderName);

        bool DeleteFile(Guid fileId);
    }
}

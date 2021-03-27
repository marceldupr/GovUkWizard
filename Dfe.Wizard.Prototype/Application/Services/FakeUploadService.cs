using System;
using System.IO;
using Dfe.Wizard.Prototype.Models;

namespace Dfe.Wizard.Prototype.Application.Services
{
    public class FakeUploadService : IFileUploadService
    {
        public FileUploadResult UploadFile(Stream file, string filename, string mimeType, string folderName)
        {
            return new FileUploadResult
            {
                FileId = Guid.NewGuid(),
                FileName = filename,
                FileSizeInBytes = 100100,
                FolderName = folderName
            };
        }

        public bool DeleteFile(Guid fileId)
        {
            return true;
        }
    }
}

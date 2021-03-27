using System;

namespace Dfe.Wizard.Prototype.Models
{
    public class FileUploadResult
    {
        public Guid FileId { get; set; }

        public long FileSizeInBytes { get; internal set; }

        public string FileName { get; internal set; }

        public string FolderName { get; internal set; }
    }

    public class FileValidationError
    {
        public FileValidationError(string title, string detail)
        {
            Title = title;
            Detail = detail;
        }

        public string Title { get; }
        public string Detail { get; }
    }
}

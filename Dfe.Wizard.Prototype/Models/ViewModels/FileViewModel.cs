using System.Collections.Generic;

namespace Dfe.Wizard.Prototype.Models.ViewModels
{
    public class FilesViewModel
    {
        public FilesViewModel()
        {
            Files = new List<FileViewModel>();
        }

        public List<FileViewModel> Files { get; set; }
    }

    public class FileViewModel
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FolderName { get; set; }
    }
}

using System;
using Dfe.Wizard.Prototype.Models;
using Microsoft.AspNetCore.Http;

namespace Dfe.Wizard.Prototype.Application.Services
{
    public interface IEvidenceService
    {
        FileUploadResult UploadEvidence(IFormFile file);

        FileUploadResult UploadEvidence(string folderName, IFormFile file);

        bool DeleteEvidenceFile(Guid fileId);

        void RelateEvidence(Guid amendmentId, string evidenceFolderName);
    }
}

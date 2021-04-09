using System.Collections.Generic;
using System.IO;
using MedicalInformationSystem.Persistant;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace MedicalInformationSystem.Helper
{
    public class FileHelper
    {
        private readonly MedicalSystemDbContext _context;

        public FileHelper(MedicalSystemDbContext context)
        {
            _context = context;
        }

        public static void UploadAll(List<IFormFile> files, string pathToSave)
        {
            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString()
                    .Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);

                using var stream = new FileStream(fullPath,FileMode.Create);
                file.CopyTo(stream);
            }
        }
    }
}

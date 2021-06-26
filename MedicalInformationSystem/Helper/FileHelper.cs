using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace MedicalInformationSystem.Helper
{
    public class FileHelper
    {
        public static List<string> UploadAll(List<IFormFile> formFiles, string pathToSave)
        {
            var files = new List<string>();
            foreach (var file in formFiles)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = Path.Combine(Guid.NewGuid() + fileExtension);
                var fileInDb = Path.Combine( pathToSave, fileName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", pathToSave, fileName);

                if (!Directory.Exists(Path.Combine("wwwroot", pathToSave)))
                    Directory.CreateDirectory(Path.Combine("wwwroot", pathToSave));

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                files.Add(fileInDb);
            }

            return files;
        }
    }
}

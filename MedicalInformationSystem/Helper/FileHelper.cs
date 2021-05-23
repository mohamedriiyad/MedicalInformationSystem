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
                var fileName = file.FileName;
                var fileExtension = Path.GetExtension(fileName);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);


                var fullPath = Path.Combine(pathToSave, fileNameWithoutExtension+Guid.NewGuid().ToString()+fileExtension);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                using var stream = new FileStream(fullPath,FileMode.Create);
                file.CopyTo(stream);

                files.Add(fullPath);
            }

            return files;
        }
    }
}

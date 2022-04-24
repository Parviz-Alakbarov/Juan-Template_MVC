using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace JuanShoesStore.Util
{
    public static class FileManager
    {
        public static string Save(string rootPath, string folderPath, IFormFile file)
        {
            string fileName = file.FileName;

            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length-62, 62);
            }
            fileName = $"{ Guid.NewGuid()}--{fileName}";

            string fileNewPath = Path.Combine(rootPath, folderPath, fileName);

            using (FileStream stream = new FileStream(fileNewPath,FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }
        public static bool Delete(string rootPath, string folderPath, string fileName)
        {
            string filePath = Path.Combine(rootPath, folderPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
    }
}

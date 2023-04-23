using Practice.Models;

namespace Practice.Helpers
{
    public static class FileHelper
    {
        public static bool CheckFileType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
        public static bool CheckFileSize(this IFormFile file, long size)
        {
            return file.Length / 1024 < size;
        }
        public static string GetFilePath(string root,string folder,string fileName)
        {
           return Path.Combine(root,folder,fileName);
        }
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
               File.Delete(path);
            }
        }
        public static string CreateFile(this IFormFile file,IWebHostEnvironment env, string folderName)
        {
            string fileName = String.Concat(Guid.NewGuid().ToString(), file.FileName);
            string path = GetFilePath(env.WebRootPath, "img", fileName);
            using (FileStream stream = new(path, FileMode.Create))
            {
                 file.CopyTo(stream);
            }
            return fileName;
        }
    }
}

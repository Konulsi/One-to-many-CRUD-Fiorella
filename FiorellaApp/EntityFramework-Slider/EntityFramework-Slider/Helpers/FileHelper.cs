using EntityFramework_Slider.Models;
using System.IO;

namespace EntityFramework_Slider.Helpers
{
    public static class FileHelper
    {

        public static bool CheckFileType(this IFormFile file, string pattern )  // pattern image word ve s olmasini yoxlamaq uchun yaziriq
        {
            return file.ContentType.Contains( pattern );
        }


        public static bool CheckFileSize(this IFormFile file, long size)
        {
            return file.Length/1024 < size ;   
        }


        public static void DeleteFile(string path)  
        {
            if (File.Exists(path)) // yoxlayiriq bazada olan yeni sileceyimiz wekil yenii -- eger bizim sistemimizde  path varsa gel onu sil
            {
                File.Delete(path);
            }
        }


        public static string GetFilePath(string root, string folder, string file)
        {
            return Path.Combine(root, folder, file);
        }

        public static async Task SaveFileAsync(string path,IFormFile file)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))     // streama copy edirik patha qoymaq uchun
            {
                await file.CopyToAsync(stream);
            }
        }

    }
}

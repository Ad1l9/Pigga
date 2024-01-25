namespace Pigga.Utilities.Extension
{
    public static class FileValidator
    {
        public static bool ValidateType(this IFormFile file, string type = "image/")
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }
        public static bool ValidateSize(this IFormFile file, int limitMb = 2)
        {
            if (file.Length<=limitMb*1024*1024)
            {
                return true;
            }
            return false;
        }
        private static string CreatePath(this string root, string fileName,params string[] folders)
        {
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, fileName);
            return path;
        }
        public static async Task<string> CreateFileAsync(this IFormFile file,string root,params string[] folders)
        {
            string fileExtension=Path.GetExtension(file.FileName);
            string fileName = $"{Guid.NewGuid()}{fileExtension}";

            string path=CreatePath(root,fileName,folders);
            using (var fs=new FileStream(path,FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            return fileName;
        }
        public static void DeleteFile(this string root, string fileName, params string[] folders)
        {
            string path=CreatePath(root, fileName,folders);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}

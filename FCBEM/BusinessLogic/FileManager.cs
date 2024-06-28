using OfficeOpenXml;
using Core.Models.Utility;

namespace FCBEM24.BusinessLogic
{
    public static class FileManager
    {
        public delegate StatusMessage GetAuthorDelegate<T>(ExcelWorksheets worksheet, out List<T> values);

        public static StatusMessage ReadFileToListAsync<T>(IFormFile file, GetAuthorDelegate<T> getAuthorDelegate, out List<T> authors)
        {
            authors = [];
            StatusMessage message = new()
            {
                IsSuccess = false
            };
            try
            {
                if (file == null || file.Length <= 0)
                    return message;

                if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    return message;

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    using var package = new ExcelPackage(stream);
                    int ok = package.Workbook.Worksheets.Count;
                    ExcelWorksheets worksheets = package.Workbook.Worksheets;

                    // loop through the worksheet rows and columns
                    return getAuthorDelegate(worksheets, out authors);
                }
            }
            catch (Exception e)
            {
                message.IsSuccess = false;
                message.Message = "Error occurred";
            }
            return message;
        }

        public static async Task<List<string>> SaveFilesAsync(List<IFormFile> formFiles, string pathUpload, string webRootPath)
        {
            List<string> urls = [];
            foreach (var formFile in formFiles)
            {
                if (formFile.Length > 0)
                {
                    urls.Add(await SaveFileAsync(formFile, pathUpload, webRootPath));
                }
            }
            return urls;
        }
        public static async Task<List<string>> SaveFilesAsyncToServer(List<IFormFile> formFiles,
                                                                string pathUpload,
                                                                string webRootPath)
        {
            List<string> urls = [];


            foreach (var formFile in formFiles)
            {
                if (formFile.Length > 0)
                {
                    urls.Add(await SaveFileFCBEMAsync(formFile, pathUpload, webRootPath));
                }
            }

            return urls;
        }

        public static async Task<string> SaveFileAsync(IFormFile formFile, string pathUpload, string webRootPath, string? inputFileName = null)
        {
            // Determine the file name to be saved
            string fileName = !string.IsNullOrEmpty(inputFileName) ? inputFileName : DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + formFile.FileName;
            bool exists = System.IO.Directory.Exists(Path.Combine(webRootPath, pathUpload));

            if (!exists)
                System.IO.Directory.CreateDirectory(Path.Combine(webRootPath, pathUpload));
            // Combine the pathUpload and fileName to get the full path
            string path = Path.Combine(pathUpload, fileName);

            // Create a FileStream to create the new file at the specified path
            using (FileStream stream = System.IO.File.Create(Path.Combine(webRootPath, path)))
            {
                // Copy the data from formFile to the stream using CopyToAsync() method
                await formFile.CopyToAsync(stream);
            }

            // Return the saved file name
            return fileName;
        }

        public static async Task<string> SaveFileFCBEMAsync(IFormFile formFile, string pathUpload, string webRootPath, string? inputFileName = null)
        {

            // Determine the file name to be saved
            string fileName = !string.IsNullOrEmpty(inputFileName) ? inputFileName : DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + formFile.FileName;

            // Combine the pathUpload and fileName to get the full path
            string path = Path.Combine(pathUpload, fileName);

            // Create a FileStream to create the new file at the specified path
            using (FileStream stream = System.IO.File.Create(Path.Combine(webRootPath, path)))
            {
                // Copy the data from formFile to the stream using CopyToAsync() method
                await formFile.CopyToAsync(stream);
            }

            // Return the saved file name
            return fileName;
        }


        public static List<string> SaveFile(List<IFormFile> formFiles, string pathUpload, string webRootPath)
        {
            List<string> urls = [];
            foreach (var formFile in formFiles)
            {
                if (formFile.Length > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + formFile.FileName;

                    string path = Path.Combine(pathUpload, fileName);
                    using (FileStream stream = System.IO.File.Create(Path.Combine(webRootPath, path)))
                    {
                        formFile.CopyTo(stream);
                    }
                    urls.Add(fileName);//lấy dòng này đưa vào img src
                }
            }
            return urls;
        }


        public static bool CreateFolderIfNotExists(string folderPath)
        {
            // Kiểm tra xem thư mục có tồn tại không
            if (!Directory.Exists(folderPath))
            {
                // Nếu không tồn tại, tạo thư mục mới
                Directory.CreateDirectory(folderPath);
                return true;
            }
            else
            {
               return false;
            }
        }
    }
}

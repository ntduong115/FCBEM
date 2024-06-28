using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace Core.Commons.Validations
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        public string[]? Extensions { get; set; }

        public override bool IsValid(object? value)
        {
            List<IFormFile> files = (value as List<IFormFile>) ?? [];
            var file = value as IFormFile;
            if (file != null)
            {
                files.Add(file);

            }
            foreach (var formfile in files)
            {
                var extension = Path.GetExtension(formfile.FileName);
                if (Extensions != null && !Extensions.Contains(extension.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

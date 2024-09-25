using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using Microsoft.Extensions.Localization;
using Core.Models.Utility;
using Microsoft.EntityFrameworkCore;

namespace Core.Commons
{
    public static class Methods
    {
        public static IStringLocalizer GetStringLocalizer<T>(IStringLocalizerFactory factory)
        {
            var type = typeof(T).GetTypeInfo();
            var assemblyName = new AssemblyName(typeof(T).GetTypeInfo().Assembly.FullName);
            IStringLocalizer localizer = factory.Create(type.Name, assemblyName.Name);
            return localizer;
        }

        public static IStringLocalizer GetSubPathStringLocalizer<T>(IStringLocalizerFactory factory)
        {
            var type = typeof(T).GetTypeInfo();
            var assemblyName = new AssemblyName(typeof(T).GetTypeInfo().Assembly.FullName);
            string location = type.FullName.Replace(assemblyName.Name + ".Resources.", "");
            IStringLocalizer localizer = factory.Create(location, assemblyName.Name);
            return localizer;
        }

        public static T MapProperties<T>(object fromObject, T toObject, bool allowNull = false)
        {
            if (fromObject == null || toObject == null)
            {
                return toObject;
            }
            foreach (var prop in fromObject.GetType().GetProperties())
            {
                Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(fromObject, null));
                var toObjectProp = toObject.GetType().GetProperty(prop.Name);
                if (toObjectProp != null && toObjectProp.CanWrite && (prop.GetValue(fromObject, null) != null || allowNull))
                {
                    toObjectProp.SetValue(toObject, prop.GetValue(fromObject, null), null);
                }

            }
            return (T)toObject;
        }

        public static string RandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            Random random = new();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>  
        /// Removes all accents from the input string.  
        /// </summary>  
        /// <param name="text">The input string.</param>  
        /// <returns></returns>  
        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        /// <summary>  
        /// Turn a string into a slug by removing all accents,   
        /// special characters, additional spaces, substituting   
        /// spaces with hyphens & making it lower-case.  
        /// </summary>  
        /// <param name="phrase">The string to turn into a slug.</param>  
        /// <returns></returns>  
        public static string Slugify(this string phrase)
        {
            // Remove all accents and make the string lower case.  
            string output = phrase.RemoveAccents().ToLower();

            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.  
            return output;
        }

        public static string? GetDisplayName(this Enum enumValue)
        {
            if (enumValue == null)
            {
                return string.Empty;
            }
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static string? GetDisplayName(this object enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static string? GetJsonContentByLang(this string text, string lang)
        {
            if (text == null)
            {
                return null;
            }
            var lst = JsonConvert.DeserializeObject<List<Translation>>(text);
            foreach (var item in lst)
            {
                if (item.Language == lang)
                {
                    return item.Content;
                }
            }
            return string.Empty;
        }

        public static Image ResizeImage(Image original, Size size, bool IsCropped)
        {
            if (size.Width < original.Width || size.Height < original.Height)
            {
                //resize theo định dạng có sẵn
                var resized = new Bitmap(original, size.Width, original.Height * size.Width / original.Width);
                //nếu crop center
                if (resized.Height > size.Height && IsCropped)
                {
                    var cropArea = new Rectangle(0, (resized.Height - size.Height) / 2, size.Width, size.Height);
                    resized = resized.Clone(cropArea, resized.PixelFormat);
                }
                return resized;
            }
            else
            {
                return new Bitmap(original);
            }
        }

        public static ImageCodecInfo? GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static void TryUpdateManyToMany<T, TKey>(this DbContext context, IEnumerable<T> currentItems, IEnumerable<T> newItems, Func<T, TKey> getKey) where T : class
        {
            context.Set<T>().RemoveRange(Except(currentItems, newItems, getKey));
            context.Set<T>().AddRange(Except(newItems, currentItems, getKey));
        }

        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> getKeyFunc)
        {
            return items
                .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems })
                .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) => new { t, temp })
                .Where(t => ReferenceEquals(null, t.temp) || t.temp.Equals(default(T)))
                .Select(t => t.t.item);
        }

        public static string CombineSHA256(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace LabsWebApp5.Helpers
{
    public static class Extensions
    {
        public static string CutController(this string str) => str.Replace("Controller", "");
        public static string ToShortDateTimeString(this DateTime dateTime)
        {
            string result = dateTime.ToShortDateString();
            if (dateTime.Hour == 0)
            {
                if (dateTime.Minute == 0)
                    return result;
                if (dateTime.Minute == 1)
                    dateTime = dateTime.AddMinutes(-1);
            }
            return result + $" {dateTime.ToShortTimeString()}";
        }

        public static string ToWillWasString(this DateTime dateTime)
        {
            string result = dateTime > DateTime.Now ? "Состоится " : "Состоялось ";
            if (dateTime.Hour == 0)
            {
                if (dateTime.Minute == 0)
                    return $"{result} {dateTime.ToShortDateString()}";
                if (dateTime.Minute == 1)
                    return result + $"{dateTime.ToShortDateString()} в полночь";
            }
            return result + $"{dateTime.ToShortDateString()} в {dateTime.ToShortTimeString()}.";
        }

        public static string ToSaveFileName(this IFormFile titleImageFile, out bool notExist)
        {
            notExist = true;
            string result = Path.Combine(Config.WebRootPath, "images/DB", titleImageFile.FileName);
            if (File.Exists(result))
            {
                if (new FileInfo(result).Length == titleImageFile.Length)
                {
                    notExist = false;
                    return null;
                }
                else
                {
                    return string.Empty;
                }
            }
            return result;
        }
    }
}

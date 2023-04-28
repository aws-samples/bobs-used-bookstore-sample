using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Bookstore.Web.Helpers
{
    public class ImageTypesAttribute : ValidationAttribute
    {
        private readonly string[] imageTypes;

        public ImageTypesAttribute(string[] imageTypes)
        {
            this.imageTypes = imageTypes;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            if (value is not IFormFile file) return base.IsValid(value);

            var extension = Path.GetExtension(file.FileName);

            return imageTypes.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a PNG or JPG image.";
        }
    }
}
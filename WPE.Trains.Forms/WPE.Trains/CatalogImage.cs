﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public class CatalogImage
    {
        public string ImageUrl { get; set; }
        public bool Double { get; internal set; }

        public string GetFilename()
        {
            return ImageFile.GetFileNameFromUrl(ImageUrl);
        }

        public int GetImageNumberFromFilename()
        {
            string filename = ImageFile.GetFileNameFromUrl(ImageUrl);
            var prefix = string.Concat(filename.TakeWhile(c => !char.IsDigit(c)));
            string numberString = filename.Substring(prefix.Length);
            numberString = string.Concat(numberString.TakeWhile(c => char.IsDigit(c)));
            if (string.IsNullOrEmpty(numberString))
            {
                return -1;
            }
            numberString = numberString.TrimStart('0');
            if (string.IsNullOrEmpty(numberString))
            {
                numberString = "0";
            }
            return int.Parse(numberString);
        }
        public ImageFile GetImageFile()
        {
            return ImageFile.FromUrl(this.ImageUrl);
        }
    }
}

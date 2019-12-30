using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public class ImageFile
    {
        public byte[] FileData { get; set; }
        public string Extension { get; set; }

        public static ImageFile FromUrl(string url)
        {
            if (!url.ToLowerInvariant().StartsWith("http"))
            {
                if (File.Exists(url))
                {
                    var bytes = File.ReadAllBytes(url);
                    var extension = Path.GetExtension(url);
                    return new ImageFile()
                    {
                        Extension = extension,
                        FileData = bytes
                    };
                }
            }
            try
            {
                WebClient client = new WebClient();
                var data = client.DownloadData(url);
                string contentType = client.ResponseHeaders["Content-Type"];
                if (!contentType.StartsWith("image"))
                {
                    throw new Exception("Couldn't get image content type from image at url " + url);
                }
                contentType = contentType.Replace("image/", "");
                contentType = "." + contentType;
                return new ImageFile()
                {
                    Extension = contentType,
                    FileData = data
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetFileNameFromUrl(string url)
        {
            if (!url.ToLowerInvariant().StartsWith("http"))
            {
                if (File.Exists(url))
                {
                    return Path.GetFileNameWithoutExtension(url);
                }
            }
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                uri = new Uri(url);
            }

            return Path.GetFileNameWithoutExtension(uri.LocalPath);
        }
    }
}

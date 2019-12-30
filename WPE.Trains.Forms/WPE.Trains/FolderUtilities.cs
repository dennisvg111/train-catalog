using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace WPE.Trains
{
    public static class FolderUtilities
    {
        private static string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WPE_Trains");

        private static List<string> catalogListFolders = new List<string>();
        
        static FolderUtilities()
        {
            if (!Directory.Exists(appdataFolder))
            {
                Directory.CreateDirectory(appdataFolder);
            }
            catalogListFolders = Directory.GetDirectories(appdataFolder).Select(p => Path.GetFileName(p)).ToList();
            if (catalogListFolders.Count < 1)
            {
                catalogListFolders.Add("fleischmann_katalogservice");
                catalogListFolders.Add("lehmann-lgb-katalogservice");
                foreach (var folder in catalogListFolders)
                {
                    Directory.CreateDirectory(Path.Combine(appdataFolder, folder));
                }
            }
        }

        private static JsonSerializerSettings GetDefaultJsonSettigns()
        {
            return new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Formatting = Formatting.Indented
            };
        }

        private static ImageFile GetImageFromUrl(string url)
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

        public static IReadOnlyList<string> GetCatalogLists()
        {
            return catalogListFolders;
        }

        public static IReadOnlyList<CatalogInfo> GetCatalogListItems(string catalogList)
        {
            if (!catalogListFolders.Contains(catalogList))
            {
                return new List<CatalogInfo>();
            }
            string catalogListFolder = Path.Combine(appdataFolder, catalogList);
            var folders = Directory.GetDirectories(catalogListFolder).Select(p => Path.GetFileName(p)).ToList();
            List<CatalogInfo> catalogs = new List<CatalogInfo>();
            foreach (var folder in folders)
            {
                string catalogFolder = Path.Combine(catalogListFolder, folder);
                if (!File.Exists(Path.Combine(catalogFolder, "info.json")))
                {
                    continue;
                }
                var json = File.ReadAllText(Path.Combine(catalogFolder, "info.json"));
                CatalogInfo catalog = JsonConvert.DeserializeObject<CatalogInfo>(json, GetDefaultJsonSettigns());
                catalogs.Add(catalog);

                var thumbnail = Directory.EnumerateFiles(catalogFolder, "thumbnail.*", SearchOption.TopDirectoryOnly).FirstOrDefault();
                if (!string.IsNullOrEmpty(thumbnail))
                {
                    catalog.ThumbnailUrl = thumbnail;
                }
            }
            return catalogs;
        }

        public static void SaveCatalogInfo(string catalogList, CatalogInfo info)
        {
            if (!catalogListFolders.Contains(catalogList))
            {
                throw new ArgumentException($"Catalog {catalogList} does not exists");
            }
            string catalogListFolder = Path.Combine(appdataFolder, catalogList);
            string catalogFolder = Path.Combine(catalogListFolder, info.Identifier);
            Directory.CreateDirectory(catalogFolder);
            JsonSerializer serializer = new JsonSerializer();
            var json = JsonConvert.SerializeObject(info, GetDefaultJsonSettigns());
            File.WriteAllText(Path.Combine(catalogFolder, "info.json"), json);

            ImageFile image = null;
            try
            {
                image = GetImageFromUrl(info.ThumbnailUrl);
            }
            catch (Exception) { }
            if (image != null && image.FileData != null && image.FileData.Length > 0)
            {
                File.WriteAllBytes(Path.Combine(catalogFolder, "thumbnail" + image.Extension), image.FileData);
            }
        }
    }
}

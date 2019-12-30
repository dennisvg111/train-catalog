using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WPE.Trains
{
    public static class FolderUtilities
    {
        private static string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WPE_Trains");

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

        public static IReadOnlyList<CatalogImage> GetCatalogImages(string catalogList, string catalogIdentifier)
        {
            if (!catalogListFolders.Contains(catalogList))
            {
                return new List<CatalogImage>();
            }
            string catalogListFolder = Path.Combine(appdataFolder, catalogList);
            string catalogFolder = Path.Combine(catalogListFolder, catalogIdentifier);
            string imagesFolder = Path.Combine(catalogFolder, "images");
            if (!Directory.Exists(imagesFolder))
            {
                return new List<CatalogImage>();
            }
            var imageOrderFile = Path.Combine(catalogFolder, "image-order.json");

            List<CatalogImage> images = new List<CatalogImage>();
            var imagePaths = Directory.EnumerateFiles(imagesFolder, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var image in imagePaths)
            {
                images.Add(new CatalogImage()
                {
                    ImageUrl = image
                });
            }
            if (File.Exists(imageOrderFile))
            {
                var json = File.ReadAllText(imageOrderFile);
                List<string> orderedImagePaths = JsonConvert.DeserializeObject<List<string>>(json, GetDefaultJsonSettigns());
                images.Sort((i1, i2) =>
                {
                    if (orderedImagePaths.Contains(Path.GetFileName(i1.ImageUrl)) && orderedImagePaths.Contains(Path.GetFileName(i2.ImageUrl)))
                    {
                        return orderedImagePaths.IndexOf(Path.GetFileName(i1.ImageUrl)).CompareTo(orderedImagePaths.IndexOf(Path.GetFileName(i2.ImageUrl)));
                    }
                    if (orderedImagePaths.Contains(Path.GetFileName(i1.ImageUrl)))
                    {
                        return -1;
                    }
                    if (orderedImagePaths.Contains(Path.GetFileName(i2.ImageUrl)))
                    {
                        return 1;
                    }
                    return 0;
                });
            }
            return images;
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
                image = ImageFile.FromUrl(info.ThumbnailUrl);
            }
            catch (Exception) { }
            if (image != null && image.FileData != null && image.FileData.Length > 0)
            {
                File.WriteAllBytes(Path.Combine(catalogFolder, "thumbnail" + image.Extension), image.FileData);
            }
        }

        public static string SaveCatalogImage(string catalogList, string catalogIdentifier, string imageUrl)
        {
            string fileName = ImageFile.GetFileNameFromUrl(imageUrl);
            string catalogListFolder = Path.Combine(appdataFolder, catalogList);
            string catalogFolder = Path.Combine(catalogListFolder, catalogIdentifier);
            string imageFolder = Path.Combine(catalogFolder, "images");
            if (!Directory.Exists(imageFolder))
            {
                Directory.CreateDirectory(imageFolder);
            }
            var image = ImageFile.FromUrl(imageUrl);
            string imagePath = Path.Combine(imageFolder, fileName + image.Extension);
            File.WriteAllBytes(imagePath, image.FileData);
            return imagePath;
        }

        public static void SaveCatalogImagesList(string catalogList, string catalogIdentifier, List<string> images)
        {
            string catalogListFolder = Path.Combine(appdataFolder, catalogList);
            string catalogFolder = Path.Combine(catalogListFolder, catalogIdentifier);
            Directory.CreateDirectory(catalogFolder);
            JsonSerializer serializer = new JsonSerializer();
            var json = JsonConvert.SerializeObject(images, GetDefaultJsonSettigns());
            File.WriteAllText(Path.Combine(catalogFolder, "image-order.json"), json);
        }
    }
}

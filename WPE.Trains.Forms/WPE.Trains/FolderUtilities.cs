using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;

namespace WPE.Trains
{
    internal static class FolderUtilities
    {
        private static string appdataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WPE_Trains");
        private static string sitePath = Path.Combine(appdataFolder, "site.html");
        internal static string BaseFolder { get { return appdataFolder; } }
        internal static string SitePath { get { return sitePath; } }

        static FolderUtilities()
        {
            if (!Directory.Exists(appdataFolder))
            {
                Directory.CreateDirectory(appdataFolder);
            }
            if (GetCatalogLists().Count < 1)
            {
                File.WriteAllText(Path.Combine(appdataFolder, "catalog-lists.txt"), string.Join(Environment.NewLine, new List<string>() { "fleischmann_katalogservice", "lehmann-lgb-katalogservice" }));

                foreach (var folder in GetCatalogLists())
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



        internal static IReadOnlyList<string> GetCatalogLists()
        {
            if (!File.Exists(Path.Combine(appdataFolder, "catalog-lists.txt")))
            {
                return new List<string>();
            }
            string catalogLists = File.ReadAllText(Path.Combine(appdataFolder, "catalog-lists.txt"));
            return catalogLists.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        internal static IReadOnlyList<CatalogInfo> GetCatalogListItems(string catalogList)
        {
            if (!GetCatalogLists().Contains(catalogList))
            {
                return new List<CatalogInfo>();
            }
            string catalogListFolder = Path.Combine(appdataFolder, catalogList);
            if (!Directory.Exists(catalogListFolder))
            {
                Directory.CreateDirectory(catalogListFolder);
            }
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

        internal static IReadOnlyList<CatalogImage> GetCatalogImages(string catalogList, string catalogIdentifier)
        {
            if (!GetCatalogLists().Contains(catalogList))
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

        internal static void SaveCatalogInfo(string catalogList, CatalogInfo info, out string thumbnailUrl)
        {
            thumbnailUrl = info.ThumbnailUrl;
            if (!GetCatalogLists().Contains(catalogList))
            {
                throw new ArgumentException($"Catalog {catalogList} does not exists");
            }
            string catalogListFolder = Path.Combine(appdataFolder, catalogList);
            if (!Directory.Exists(catalogListFolder))
            {
                Directory.CreateDirectory(catalogListFolder);
            }
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
                try
                {
                    Image netImage = image.ToDotNetImage();
                    Bitmap bitmap = new Bitmap(netImage);
                    try
                    {
                        bitmap = Cropper.Crop(bitmap);
                    }
                    catch (Exception) { }
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        image.FileData = memoryStream.ToArray();
                        image.Extension = ".png";
                    }
                }
                catch (Exception) { }
                File.WriteAllBytes(Path.Combine(catalogFolder, "thumbnail" + image.Extension), image.FileData);
                thumbnailUrl = Path.Combine(catalogFolder, "thumbnail" + image.Extension);
            }
        }

        internal static string SaveCatalogImage(string catalogList, string catalogIdentifier, string imageUrl)
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

        internal static void SaveCatalogImagesList(string catalogList, string catalogIdentifier, List<string> images)
        {
            string catalogListFolder = Path.Combine(appdataFolder, catalogList);
            string catalogFolder = Path.Combine(catalogListFolder, catalogIdentifier);
            Directory.CreateDirectory(catalogFolder);
            JsonSerializer serializer = new JsonSerializer();
            var json = JsonConvert.SerializeObject(images, GetDefaultJsonSettigns());
            File.WriteAllText(Path.Combine(catalogFolder, "image-order.json"), json);
        }

        internal static void WriteBookletResources()
        {
            string bookletFolder = Path.Combine(appdataFolder, "booklet");
            Directory.CreateDirectory(bookletFolder);
            File.WriteAllText(Path.Combine(bookletFolder, "jquery.booklet.latest.css"), Properties.Resources.jquery_booklet_latestCss);
            File.WriteAllText(Path.Combine(bookletFolder, "jquery.booklet.latest.js"), Properties.Resources.jquery_booklet_latestJs);
            File.WriteAllText(Path.Combine(bookletFolder, "jquery.booklet.latest.min.js"), Properties.Resources.jquery_booklet_latest_min);
            File.WriteAllText(Path.Combine(bookletFolder, "jquery.easing.1.3.js"), Properties.Resources.jquery_easing_1_3);
            File.WriteAllText(Path.Combine(bookletFolder, "jquery-2.1.0.min.js"), Properties.Resources.jquery_2_1_0_min);
            File.WriteAllText(Path.Combine(bookletFolder, "jquery-ui-1.10.4.min.js"), Properties.Resources.jquery_ui_1_10_4_min);
            string bookletImagesFolder = Path.Combine(bookletFolder, "images");
            Directory.CreateDirectory(bookletImagesFolder);
            Properties.Resources.arrow_next.Save(Path.Combine(bookletImagesFolder, "arrow-next.png"), System.Drawing.Imaging.ImageFormat.Png);
            Properties.Resources.arrow_prev.Save(Path.Combine(bookletImagesFolder, "arrow-prev.png"), System.Drawing.Imaging.ImageFormat.Png);
            Properties.Resources.shadow.Save(Path.Combine(bookletImagesFolder, "shadow.png"), System.Drawing.Imaging.ImageFormat.Png);
            Properties.Resources.shadow_top_back.Save(Path.Combine(bookletImagesFolder, "shadow-top-back.png"), System.Drawing.Imaging.ImageFormat.Png);
            Properties.Resources.shadow_top_forward.Save(Path.Combine(bookletImagesFolder, "shadow-top-forward.png"), System.Drawing.Imaging.ImageFormat.Png);
            File.WriteAllBytes(Path.Combine(bookletImagesFolder, "closedhand.cur"), Properties.Resources.closedhand);
            File.WriteAllBytes(Path.Combine(bookletImagesFolder, "openhand.cur"), Properties.Resources.openhand);
            string fontsFolder = Path.Combine(appdataFolder, "fonts");
            Directory.CreateDirectory(fontsFolder);
            File.WriteAllBytes(Path.Combine(fontsFolder, "Courgette-Regular.ttf"), Properties.Resources.Courgette_Regular);
            File.WriteAllBytes(Path.Combine(fontsFolder, "Raleway-Regular.ttf"), Properties.Resources.Raleway_Regular);
            string imagesFolder = Path.Combine(appdataFolder, "images");
            Directory.CreateDirectory(imagesFolder);
            Properties.Resources.left_bg.Save(Path.Combine(imagesFolder, "left_bg.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            Properties.Resources.right_bg.Save(Path.Combine(imagesFolder, "right_bg.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);

        }

        internal static string WriteSiteHtml(string html)
        {
            File.WriteAllText(sitePath, html);
            return sitePath;
        }

        internal static bool SiteExists()
        {
            return File.Exists(sitePath);
        }
    }
}

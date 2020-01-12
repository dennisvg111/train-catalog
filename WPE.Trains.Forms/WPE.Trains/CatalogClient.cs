using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WPE.Trains
{
    public class CatalogClient : SiteClient
    {
        private string catalogListName;
        public string CatalogList { get { return catalogListName; } }
        public CatalogClient(string catalogListName, string userAgent = null) : base(userAgent)
        {
            this.catalogListName = catalogListName;
            GetDefaultClient("https://www.conradantiquario.de/");
        }

        public List<CatalogInfo> GetCatalogList()
        {
            List<CatalogInfo> catalogs = FolderUtilities.GetCatalogListItems(this.catalogListName).ToList();

            HtmlDocument document = null;
            try
            {
                document = DownloadHtmlDocument($"content/{this.catalogListName}.html");
            }
            catch (Exception e)
            {
                document = null;
            }
            if (document != null)
            {
                var documentNode = document.DocumentNode;

                var catalogNodes = document.DocumentNode.SelectNodes("//*[@id='content']//*[contains(@class, 'table-fill')]//tbody//tr");
                if (catalogNodes != null)
                {
                    foreach (var node in catalogNodes)
                    {
                        var url = node.SelectSingleNode("(.//td)[2]//a").GetAttributeValue("href", null);
                        url = url.Replace(".html", "");
                        string catalogName = url.Replace("katalog/", "");
                        var cachedCatalog = catalogs.FirstOrDefault(c => c.Identifier.ToLowerInvariant() == catalogName.ToLowerInvariant());
                        if (cachedCatalog != null && !string.IsNullOrEmpty(cachedCatalog.ThumbnailUrl))
                        {
                            continue;
                        }
                        var manufacturer = node.SelectSingleNode("(.//td)[1]").InnerText;
                        var thumbnail = node.SelectSingleNode("(.//td)[2]//img").GetAttributeValue("src", null);
                        if (!string.IsNullOrEmpty(thumbnail))
                        {
                            thumbnail = thumbnail.Replace("..", "https://www.conradantiquario.de");
                        }
                        var years = node.SelectSingleNode("(.//td)[3]").InnerText;
                        var description = node.SelectSingleNode("(.//td)[4]").InnerText;
                        var language = node.SelectSingleNode("(.//td)[5]");
                        CatalogInfo catalog = new CatalogInfo()
                        {
                            Identifier = catalogName,
                            Description = description,
                            ThumbnailUrl = thumbnail,
                            Manufacturer = manufacturer,
                            Year = years
                        };
                        catalogs.Add(catalog);
                        string newThumbnailUrl;
                        FolderUtilities.SaveCatalogInfo(this.catalogListName, catalog, out newThumbnailUrl);
                        catalog.ThumbnailUrl = newThumbnailUrl;
                    }
                }
            }
            foreach (var catalog in catalogs)
            {
                catalog.CatalogList = this.catalogListName;
            }
            catalogs.Sort();
            return catalogs;
        }

        public List<CatalogImage> GetCatalogImages(string catalogIdentifier)
        {
            List<CatalogImage> images = FolderUtilities.GetCatalogImages(this.catalogListName, catalogIdentifier).ToList();
            HtmlDocument document = null;
            try
            {
                document = DownloadHtmlDocument($"content/katalog/{catalogIdentifier}.html");
            }
            catch (Exception)
            {
                document = null;
            }
            if (document != null)
            {
                var documentNode = document.DocumentNode;

                var highResNodes = documentNode.SelectNodes(".//*[contains(@class, 'high-res')]");
                if (highResNodes != null)
                {
                    foreach (var highRes in highResNodes)
                    {
                        var imageNodes = highRes.SelectNodes(".//*[@data-lightbox]");
                        foreach (var node in imageNodes)
                        {
                            var imageUrl = node.GetAttributeValue("href", null).TrimStart('/', '.');
                            imageUrl = "https://www.conradantiquario.de/" + imageUrl;
                            AddCatalogImageToList(images, imageUrl, catalogIdentifier);
                        }
                    }
                }

                if (images.Count == 0)
                {
                    var articleNodes = document.DocumentNode.SelectNodes("//article");
                    if (articleNodes != null)
                    {
                        foreach (var articleNode in articleNodes)
                        {
                            var imageNodes = articleNode.SelectNodes(".//*[@data-lightbox]");
                            if (imageNodes != null)
                            {
                                foreach (var node in imageNodes)
                                {
                                    var imageUrl = node.GetAttributeValue("href", null).TrimStart('/', '.');
                                    imageUrl = "https://www.conradantiquario.de/" + imageUrl;
                                    AddCatalogImageToList(images, imageUrl, catalogIdentifier);
                                }
                            }
                        }
                    }
                }
                if (images.Count == 0)
                {
                    var articleNodes = document.DocumentNode.SelectNodes("//*[@id='content']");
                    if (articleNodes != null)
                    {
                        foreach (var articleNode in articleNodes)
                        {
                            var imageNodes = articleNode.SelectNodes(".//*[@data-lightbox]");
                            if (imageNodes != null)
                            {
                                foreach (var node in imageNodes)
                                {
                                    var imageUrl = node.GetAttributeValue("href", null).TrimStart('/', '.');
                                    imageUrl = "https://www.conradantiquario.de/" + imageUrl;
                                    AddCatalogImageToList(images, imageUrl, catalogIdentifier);
                                }
                            }
                            imageNodes = articleNode.SelectNodes(".//*[@data-full]");
                            if (imageNodes != null)
                            {
                                foreach (var node in imageNodes)
                                {
                                    var imageUrl = node.GetAttributeValue("data-full", null).TrimStart('/', '.');
                                    imageUrl = "https://www.conradantiquario.de/" + imageUrl;

                                    AddCatalogImageToList(images, imageUrl, catalogIdentifier);
                                }
                            }
                        }
                    }
                }
            }

            FolderUtilities.SaveCatalogImagesList(this.catalogListName, catalogIdentifier, images.Select(i => Path.GetFileName(i.ImageUrl)).ToList());

            if (images.Count > 0)
            {
                Dictionary<AspectRatio, int> sizeCounts = new Dictionary<AspectRatio, int>();
                Dictionary<CatalogImage, AspectRatio> imageSizes = new Dictionary<CatalogImage, AspectRatio>();
                foreach (var image in images)
                {
                    var actualImage = image.GetImageFile().ToDotNetImage();
                    var aspectRatio = AspectRatio.FromImage(actualImage);
                    if (aspectRatio.OriginalRatio >= 1.25)
                    {
                        image.Double = true;
                    }
                    imageSizes[image] = aspectRatio;
                    var existing = sizeCounts.Keys.FirstOrDefault(s => s.OriginalRatio == aspectRatio.OriginalRatio);
                    if (existing == null)
                    {
                        existing = aspectRatio;
                        sizeCounts[existing] = 0;
                    }
                    sizeCounts[existing]++;
                }
                if (sizeCounts.Values.Distinct().Count() > 1 || sizeCounts.Keys.Count() == 1)
                {
                    var defaultRatio = sizeCounts.OrderByDescending(kv => kv.Value).First().Key;
                    foreach (var image in images)
                    {
                        var aspectRatio = imageSizes[image];
                        var ratio = aspectRatio.OriginalRatio / defaultRatio.OriginalRatio;
                        if (ratio > 1.9 && ratio < 2.1)
                        {
                            image.Double = true;
                        }
                    }
                }
            }

            return images;
        }

        private void AddCatalogImageToList(List<CatalogImage> images, string imageUrl, string catalogIdentifier)
        {
            CatalogImage image = new CatalogImage()
            {
                ImageUrl = imageUrl
            };
            if (images.Any(i => image.GetFilename() == i.GetFilename()))
            {
                return;
            }
            bool inserted = false;
            if (image.GetImageNumberFromFilename() > 0)
            {
                var imageBefore = images.FirstOrDefault(i => i.GetImageNumberFromFilename() == image.GetImageNumberFromFilename() - 1);
                if (imageBefore != null)
                {
                    images.Insert(images.IndexOf(imageBefore) + 1, image);
                    inserted = true;
                }
            }
            if (!inserted)
            {
                images.Add(image);
            }
            try
            {
                string newImagePath = FolderUtilities.SaveCatalogImage(this.catalogListName, catalogIdentifier, imageUrl);
                image.ImageUrl = newImagePath;
            }
            catch (Exception)
            {
                images.Remove(image);
            }
        }
    }
}

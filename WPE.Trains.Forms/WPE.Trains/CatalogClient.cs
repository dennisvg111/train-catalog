using System;
using System.Collections.Generic;
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
            catch (Exception)
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
                        FolderUtilities.SaveCatalogInfo(this.catalogListName, catalog);
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

        public void GetCatalogImages(string catalogIdentifier)
        {
            var document = DownloadHtmlDocument($"content/katalog/{catalogIdentifier}.html");
            var documentNode = document.DocumentNode;

            var highRes = documentNode.SelectSingleNode(".//*[contains(@class, 'high-res')]");
            if (highRes != null)
            {
                var imageNodes = highRes.SelectNodes(".//*[@data-lightbox]");
                foreach (var node in imageNodes)
                {
                    var thumbnailUrl = node.GetAttributeValue("href", null).TrimStart('/', '.');
                    thumbnailUrl = "https://www.conradantiquario.de/" + thumbnailUrl;
                }
            }

            var bookNode = document.DocumentNode.SelectSingleNode("//*[@id='mybook']");
            if (bookNode != null)
            {

            }
        }
    }
}

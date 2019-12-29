using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WPE.Trains
{
    public class CatalogListClient : SiteClient
    {
        public CatalogListClient(string userAgent = null) : base(userAgent)
        {
            GetDefaultClient("https://www.conradantiquario.de/");
        }

        public List<CatalogInfo> GetCatalogList()
        {
            List<CatalogInfo> catalogs = new List<CatalogInfo>();
            var document = DownloadHtmlDocument("content/fleischmann_katalogservice.html");
            var documentNode = document.DocumentNode;

            var catalogNodes = document.DocumentNode.SelectNodes("//*[@id='content']//*[contains(@class, 'table-fill')]//tbody//tr");
            if (catalogNodes == null || catalogNodes.Count == 0)
            {
                return catalogs;
            }
            foreach (var node in catalogNodes)
            {
                var url = node.SelectSingleNode("(.//td)[2]//a").GetAttributeValue("href", null);
                url = url.Replace(".html", "");
                string catalogName = url.Replace("katalog/", "");
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
            }
            return catalogs;
        }
    }
}

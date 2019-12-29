using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public class CatalogClient : SiteClient
    {
        public CatalogClient(string userAgent = null) : base(userAgent)
        {
            GetDefaultClient("https://www.conradantiquario.de/");
        }

        public void GetCatalogImages()
        {
            string catalogName = "booklet_fleischmann_dampf1950";
            var document = DownloadHtmlDocument($"content/katalog/{catalogName}.html");
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

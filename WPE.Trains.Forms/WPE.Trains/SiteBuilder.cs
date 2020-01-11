using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public class SiteBuilder
    {
        private CatalogClient client;
        public string CatalogList { get { return client.CatalogList; } }

        public SiteBuilder(string catalogListName)
        {
            client = new CatalogClient(catalogListName);
        }

        public void BuildSite()
        {
            FolderUtilities.WriteBookletResources();
            var catalogs = client.GetCatalogList();
            Dictionary<string, List<CatalogImage>> catalogImages = new Dictionary<string, List<CatalogImage>>();
            foreach (var catalog in catalogs)
            {
                List<CatalogImage> images = client.GetCatalogImages(catalog.Identifier);
                catalogImages[catalog.Identifier] = images;
            }

            string mainHtml = Properties.Resources.SiteLayout;
            string bookItemsHtml = "";
            string bookletsHtml = "";
            foreach (var catalog in catalogs)
            {
                string bookItemHtml = Properties.Resources.BookItem;
                bookItemHtml = bookItemHtml.Replace("{Identifier}", catalog.Identifier);
                bookItemHtml = bookItemHtml.Replace("{Description}", catalog.Description);
                bookItemHtml = bookItemHtml.Replace("{Year}", catalog.Year);
                bookItemHtml = bookItemHtml.Replace("{Manufacturer}", catalog.Manufacturer);
                string thumbnailUrl = catalog.ThumbnailUrl;
                if (catalogImages.ContainsKey(catalog.Identifier) && catalogImages[catalog.Identifier].Count > 0)
                {
                    string newThumbnailUrl = catalogImages[catalog.Identifier][0].ImageUrl;
                    if (!string.IsNullOrEmpty(newThumbnailUrl))
                    {
                        thumbnailUrl = newThumbnailUrl;
                    }
                    bookItemHtml = bookItemHtml.Replace("{Pages}", catalogImages[catalog.Identifier].Count.ToString());
                }
                else
                {
                    bookItemHtml = bookItemHtml.Replace("{Pages}", "-");
                }
                thumbnailUrl = thumbnailUrl.Replace(FolderUtilities.BaseFolder, "").TrimStart('\\', '/');
                bookItemHtml = bookItemHtml.Replace("{Thumbnail}", thumbnailUrl);
                bookItemsHtml += bookItemHtml + Environment.NewLine;

                string bookletHtml = Properties.Resources.BookletImages;
                bookletHtml = bookletHtml.Replace("{Identifier}", catalog.Identifier);
                bookletHtml = bookletHtml.Replace("{Description}", catalog.Description);
                bookletHtml = bookletHtml.Replace("{Year}", catalog.Year);
                bookletHtml = bookletHtml.Replace("{Manufacturer}", catalog.Manufacturer);
                string imagesHtml = "";
                if (catalogImages.ContainsKey(catalog.Identifier) && catalogImages[catalog.Identifier].Count > 0)
                {
                    foreach (var image in catalogImages[catalog.Identifier])
                    {
                        string imageUrl = image.ImageUrl.Replace(FolderUtilities.BaseFolder, "").TrimStart('\\', '/');
                        string imageHtml = Properties.Resources.BookletImage;
                        imageHtml = imageHtml.Replace("{DoubleIndicator}", image.Double ? "double" : "");
                        imageHtml = imageHtml.Replace("{ImageUrl}", imageUrl);
                        if (image.Double)
                        {
                            imageHtml += imageHtml;
                        }
                        imagesHtml += imageHtml;
                    }
                }
                bookletHtml = bookletHtml.Replace("{Images}", imagesHtml);
                bookletsHtml += bookletHtml;
            }
            mainHtml = mainHtml.Replace("{BookItems}", bookItemsHtml);
            mainHtml = mainHtml.Replace("{Booklets}", bookletsHtml);
            FolderUtilities.WriteSiteHtml(mainHtml);
        }
    }
}

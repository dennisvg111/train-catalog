using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public class SiteBuilder
    {
        public delegate void FinishedBuildingHandler(string htmlLocation);

        public event CatalogClient.CatalogListLoadHandler CatalogListLoading;
        public event FinishedBuildingHandler FinishedBuilding;

        private CatalogClient client;
        public string CatalogList { get { return client.CatalogList; } }

        public SiteBuilder(string catalogListName)
        {
            client = new CatalogClient(catalogListName);
            client.CatalogListLoading += Client_CatalogListLoading;
        }

        private void Client_CatalogListLoading(string message, float progress)
        {
            CatalogListLoading?.Invoke(message, progress);
        }

        public void BuildSite()
        {
            FolderUtilities.WriteBookletResources();
            var catalogs = client.GetCatalogList();
            CatalogListLoading?.Invoke("Downloading catalog images", 0);
            int index = 0;
            Dictionary<string, List<CatalogImage>> catalogImages = new Dictionary<string, List<CatalogImage>>();
            foreach (var catalog in catalogs)
            {
                List<CatalogImage> images = client.GetCatalogImages(catalog.Identifier);
                catalogImages[catalog.Identifier] = images;

                CatalogListLoading?.Invoke("Downloaded images for catalog " + catalog.Identifier, (index + 1) / (float)catalogs.Count);
                index++;
            }

            CatalogListLoading?.Invoke("Generating HTML ", 0);
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
                }
                thumbnailUrl = thumbnailUrl.Replace(FolderUtilities.BaseFolder, "").TrimStart('\\', '/');
                bookItemHtml = bookItemHtml.Replace("{Thumbnail}", thumbnailUrl);

                string bookletHtml = Properties.Resources.BookletImages;
                bookletHtml = bookletHtml.Replace("{Identifier}", catalog.Identifier);
                bookletHtml = bookletHtml.Replace("{Description}", catalog.Description);
                bookletHtml = bookletHtml.Replace("{Year}", catalog.Year);
                bookletHtml = bookletHtml.Replace("{Manufacturer}", catalog.Manufacturer);
                string imagesHtml = "";
                if (catalogImages.ContainsKey(catalog.Identifier) && catalogImages[catalog.Identifier].Count > 0)
                {
                    int page = 1;
                    foreach (var image in catalogImages[catalog.Identifier])
                    {
                        string imageUrl = image.ImageUrl.Replace(FolderUtilities.BaseFolder, "").TrimStart('\\', '/');
                        string imageHtml = Properties.Resources.BookletImage;
                        imageHtml = imageHtml.Replace("{DoubleIndicator}", image.Double ? "double" : "");
                        imageHtml = imageHtml.Replace("{ImageUrl}", imageUrl);
                        if (image.Double)
                        {
                            if (page % 2 == 1)
                            {
                                imagesHtml += Properties.Resources.BookletEmptyPage;
                                page++;
                            }
                            imageHtml += imageHtml;
                            page++;
                        }
                        imagesHtml += imageHtml;
                        page++;
                    }
                    if (page % 2 == 1)
                    {
                        page++;
                    }
                    bookItemHtml = bookItemHtml.Replace("{Pages}", catalogImages[catalog.Identifier].Count.ToString());
                }
                else
                {
                    bookItemHtml = bookItemHtml.Replace("{Pages}", "-");
                }
                bookItemsHtml += bookItemHtml + Environment.NewLine;

                bookletHtml = bookletHtml.Replace("{Images}", imagesHtml);
                bookletsHtml += bookletHtml + Environment.NewLine;
            }
            mainHtml = mainHtml.Replace("{BookItems}", bookItemsHtml);
            mainHtml = mainHtml.Replace("{Booklets}", bookletsHtml);
            string path = FolderUtilities.WriteSiteHtml(mainHtml);
            FinishedBuilding?.Invoke(path);
        }


    }
}

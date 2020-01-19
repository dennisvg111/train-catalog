using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public class SiteBuilder
    {
        private static SiteBuilder _instance;
        private bool busy = false;
        public bool IsBusy { get { return busy; } }
        private IReadOnlyList<string> catalogLists;
        public delegate void FinishedBuildingHandler(string htmlLocation);

        private int currentCatalogListIndex;

        public event CatalogClient.CatalogListLoadHandler CatalogListLoading;
        public event CatalogClient.CatalogLoadHandler CatalogLoading;
        public event FinishedBuildingHandler FinishedBuilding;

        private CatalogClient client;

        private SiteBuilder()
        {
            catalogLists = FolderUtilities.GetCatalogLists();
            client = new CatalogClient();
            client.CatalogListLoading += Client_CatalogListLoading;
            client.CatalogLoading += Client_CatalogLoading;
        }

        public static SiteBuilder GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SiteBuilder();
            }
            return _instance;
        }

        private void Client_CatalogLoading(string message, float progress)
        {
            CatalogLoading?.Invoke(message, progress);
        }

        private void Client_CatalogListLoading(string message, float progress)
        {
            CatalogListLoading?.Invoke(message, (1 / (float)catalogLists.Count * (currentCatalogListIndex)) + progress / (float)catalogLists.Count / 2);
        }

        public void BuildSite()
        {
            if (busy)
            {
                return;
            }
            currentCatalogListIndex = 0;
            busy = true;
            FolderUtilities.WriteBookletResources();
            string mainHtml = Properties.Resources.SiteLayout;

            string bookItemsHtml = "";
            string bookletsHtml = "";

            foreach (var catalogList in catalogLists)
            {
                var html = BuildHtmlForCatalogList(catalogList);
                bookItemsHtml += html.BookItemsHtml + Environment.NewLine;
                bookletsHtml += html.BookletsHtml + Environment.NewLine;
                currentCatalogListIndex++;
            }

            mainHtml = mainHtml.Replace("{BookItems}", bookItemsHtml);
            mainHtml = mainHtml.Replace("{Booklets}", bookletsHtml);
            string path = FolderUtilities.WriteSiteHtml(mainHtml);
            busy = false;
            FinishedBuilding?.Invoke(path);
        }

        public static bool SiteExists()
        {
            return FolderUtilities.SiteExists();
        }

        public static void OpenSite()
        {
            if (!SiteExists())
            {
                return;
            }
            Process.Start(FolderUtilities.SitePath);
        }

        public static void OpenExplorer()
        {
            Process.Start("explorer.exe", FolderUtilities.BaseFolder);
        }

        private class CatalogListHtml
        {
            public string BookItemsHtml { get; private set; }
            public string BookletsHtml { get; private set; }
            internal CatalogListHtml(string bookItemsHtml, string bookletsHtml)
            {
                this.BookItemsHtml = bookItemsHtml;
                this.BookletsHtml = bookletsHtml;
            }
        }

        private CatalogListHtml BuildHtmlForCatalogList(string catalogListName)
        {
            var catalogs = client.GetCatalogList(catalogListName);
            int index = 0;
            Dictionary<string, List<CatalogImage>> catalogImages = new Dictionary<string, List<CatalogImage>>();
            foreach (var catalog in catalogs)
            {
                List<CatalogImage> images = client.GetCatalogImages(catalogListName, catalog.Identifier);
                catalogImages[catalog.Identifier] = images;

                CatalogListLoading?.Invoke("Downloaded " + images.Count + " images for catalog " + catalog.Identifier, (1 / (float)catalogLists.Count * (currentCatalogListIndex)) + (0.5f * (1 / (float)catalogLists.Count)) + ((index + 1) / (float)catalogs.Count) / 2 / (float)catalogLists.Count);
                index++;
            }

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
            return new CatalogListHtml(bookItemsHtml, bookletsHtml);
        }
    }
}

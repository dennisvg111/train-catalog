using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WPE.Trains.Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void LoadGalleriesButton_Click(object sender, EventArgs e)
        {
            SiteBuilder siteBuilder = new SiteBuilder("fleischmann_katalogservice");
            siteBuilder.BuildSite();
            //CatalogClient catalogListClient = new CatalogClient("fleischmann_katalogservice");
            //GalleryListProgress.Value = 0;
            //var catalogs = catalogListClient.GetCatalogList();
            //GalleryListProgress.Value = 100;

            //int i = 0;
            //Dictionary<string, List<CatalogImage>> catalogImages = new Dictionary<string, List<CatalogImage>>(); 
            //foreach (var catalog in catalogs)
            //{
            //    GalleryName.Text = catalog.Description;
            //    GalleryListProgressText.Text = $"loaded {i}/{catalogs.Count}";
            //    GalleryProgress.Value = 0;
            //    List<CatalogImage> images = catalogListClient.GetCatalogImages(catalog.Identifier);
            //    catalogImages[catalog.Identifier] = images;

            //    GalleryProgress.Value = 100;
            //    i++;
            //}
            //GalleryName.Text = "";
            //GalleryListProgressText.Text = $"Building site";
        }
    }
}

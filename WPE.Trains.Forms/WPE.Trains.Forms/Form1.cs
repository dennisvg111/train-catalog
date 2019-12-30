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
            CatalogClient catalogListClient = new CatalogClient("fleischmann_katalogservice");
            var catalogs = catalogListClient.GetCatalogList();

            int y = 0;
            foreach (var catalog in catalogs)
            {
                Label year = new Label();
                year.AutoSize = true;
                year.Text = catalog.Year;
                year.Font = new Font("Arial", 20, FontStyle.Regular);
                year.Anchor = AnchorStyles.Left;
                catalogListContainer.Controls.Add(year);
                PictureBox thumbnail = new PictureBox();
                thumbnail.SizeMode = PictureBoxSizeMode.Zoom;
                thumbnail.ImageLocation = catalog.ThumbnailUrl;
                thumbnail.Width = 150;
                thumbnail.Height = 150;
                catalogListContainer.Controls.Add(thumbnail);
                Label description = new Label();
                description.AutoSize = true;
                description.Text = catalog.Description;
                description.Font = new Font("Arial", 18, FontStyle.Regular);
                description.Anchor = AnchorStyles.Left;
                catalogListContainer.Controls.Add(description);
            }
        }
    }
}

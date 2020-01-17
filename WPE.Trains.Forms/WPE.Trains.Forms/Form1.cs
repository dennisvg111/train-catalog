using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WPE.Trains.Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBoxTrain.Parent = pictureBox1;
            pictureBoxTrain.BackColor = Color.Transparent;
            pictureBoxTrain.Top = 0;
            pictureBoxTrain.Left = 0;
            pictureBoxForeground.Parent = pictureBox1;
            pictureBoxForeground.BackColor = Color.Transparent;
            pictureBoxForeground.Top = 0;
            pictureBoxForeground.Left = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void LoadGalleriesButton_Click(object sender, EventArgs e)
        {
            SiteBuilder siteBuilder = new SiteBuilder("fleischmann_katalogservice");
            siteBuilder.CatalogListLoading += SiteBuilder_CatalogListLoading;
            siteBuilder.FinishedBuilding += SiteBuilder_FinishedBuilding;
            new Thread(() =>
            {
                siteBuilder.BuildSite();
            }).Start();
        }

        private void SiteBuilder_FinishedBuilding(string htmlLocation)
        {
                this.Invoke((MethodInvoker)delegate ()
                {
                    pictureBoxTrain.Left = Convert.ToInt32(pictureBox1.Width - pictureBoxTrain.Width);
                    textBoxLog.AppendText("Finished generating HTML");
                    this.Invalidate();
                    Process.Start(htmlLocation);
                });
        }

        private void SiteBuilder_CatalogListLoading(string message, float progress)
        {
                this.Invoke((MethodInvoker)delegate ()
                {
                    pictureBoxTrain.Left = Convert.ToInt32((pictureBox1.Width - pictureBoxTrain.Width) * progress);
                    textBoxLog.AppendText(message + Environment.NewLine);
                    this.Invalidate();
                });
        }
    }
}

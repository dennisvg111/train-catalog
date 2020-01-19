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
        private Thread siteBuilderThread;
        public Form1()
        {
            InitializeComponent();
            pictureBoxForeground.Parent = pictureBox1;
            pictureBoxForeground.BackColor = Color.Transparent;
            pictureBoxForeground.Top = 0;
            pictureBoxForeground.Left = 0;
            LoadGalleriesButton.BackColor = Color.FromArgb(255, 165, 0);
            linkLabelOpenSite.Enabled = SiteBuilder.SiteExists();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowTrainProgress(SiteBuilder.SiteExists() ? 1 : 0);
        }

        private void LoadGalleriesButton_Click(object sender, EventArgs e)
        {
            SiteBuilder siteBuilder = SiteBuilder.GetInstance();
            if (siteBuilder.IsBusy)
            {
                return;
            }
            siteBuilder.CatalogListLoading += SiteBuilder_CatalogListLoading;
            siteBuilder.FinishedBuilding += SiteBuilder_FinishedBuilding;
            ShowTrainProgress(0);
            siteBuilderThread = new Thread(() =>
            {
                siteBuilder.BuildSite();
            });
            siteBuilderThread.Start();
        }

        private void SiteBuilder_FinishedBuilding(string htmlLocation)
        {
                this.Invoke((MethodInvoker)delegate ()
                {
                    textBoxLog.AppendText("Finished generating HTML");
                    ShowTrainProgress(1);
                    linkLabelOpenSite.Enabled = SiteBuilder.SiteExists();
                    SiteBuilder.OpenSite();
                });
        }

        private void SiteBuilder_CatalogListLoading(string message, float progress)
        {
                this.Invoke((MethodInvoker)delegate ()
                {
                    textBoxLog.AppendText(message + Environment.NewLine);
                    ShowTrainProgress(progress);
                });
        }

        private void ShowTrainProgress(float progress)
        {
            var train = Properties.Resources.landscape_train;
            var foreground = Properties.Resources.landscape_foreground;

            var position = Convert.ToInt32((foreground.Width - train.Width) * progress);

            var bitmap = new Bitmap(foreground.Width, foreground.Height);
            bitmap.MakeTransparent();
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(Properties.Resources.landscape_train, new Point(position, 0));
                g.DrawImage(Properties.Resources.landscape_foreground, Point.Empty);
            }
            pictureBoxForeground.BackgroundImage = bitmap;
        }

        private void linkLabelOpenSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (SiteBuilder.SiteExists())
            {
                SiteBuilder.OpenSite();
            }
        }

        private void linkLabelOpenExplorer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SiteBuilder.OpenExplorer();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (siteBuilderThread != null)
            {
                siteBuilderThread.Abort();
            }
        }
    }
}

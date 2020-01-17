using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WPE.Trains.Forms
{
    //based on https://codepen.io/codyogden/pen/OpyPoN
    //based on https://stackoverflow.com/a/41531530/5022761
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public partial class Form1 : Form
    {
        private readonly ChromiumWebBrowser browser;
        public Form1()
        {
            InitializeComponent();
            browser = new ChromiumWebBrowser(new CefSharp.Web.HtmlString(Properties.Resources.TrainLoadingIndicator));
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            panel1.Controls.Add(browser);
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            Console.WriteLine(args.Message);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void LoadGalleriesButton_Click(object sender, EventArgs e)
        {
            browser.ExecuteScriptAsync("alert('All Resources Have Loaded');");
            return;
            SiteBuilder siteBuilder = new SiteBuilder("fleischmann_katalogservice");
            siteBuilder.BuildSite();
        }
    }
}

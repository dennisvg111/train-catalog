using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WPE.Trains
{
    public abstract class SiteClient
    {
        protected string userAgent;
        protected HttpClient client;
        protected CookieContainer cookieContainer;

        public virtual bool HasClient { get { return client != null; } }

        internal SiteClient(string userAgent = null)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                //default user agent if none is given
                userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36";
            }
            this.userAgent = userAgent;
        }

        #region helper methods

        protected HttpClient GetDefaultClient(string baseAddress)
        {
            var baseUri = new Uri(baseAddress);
            cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            var newClient = new HttpClient(handler) { BaseAddress = baseUri };
            newClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            this.client = newClient;
            return newClient;
        }

        protected HttpClient LogIn(Dictionary<string, string> formValues, string formPostUrl, string baseAddress, out string responseHtml)
        {
            var formContent = new FormUrlEncodedContent(formValues);

            var baseUri = new Uri(baseAddress);
            cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            var newClient = new HttpClient(handler) { BaseAddress = baseUri };
            newClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

            var loginResult = newClient.PostAsync(formPostUrl, formContent).Result;
            loginResult.EnsureSuccessStatusCode();
            responseHtml = loginResult.Content.ReadAsStringAsync().Result;
            this.client = newClient;

            return newClient;
        }

        protected byte[] DownloadResource(string url)
        {
            Uri myUri = new Uri(url, UriKind.RelativeOrAbsolute);
            return DownloadResource(myUri);
        }

        protected byte[] DownloadResource(Uri url)
        {
            if (url.ToString().StartsWith("data"))
            {
                string dataUrl = url.ToString().Substring(url.ToString().IndexOf(',')).TrimStart(',');
                using (MemoryStream st = new MemoryStream(Convert.FromBase64String(dataUrl)))
                {
                    return st.ToArray();
                }
            }
            var oldSecurity = ServicePointManager.SecurityProtocol;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = client.GetAsync(url).Result;

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.Content.ReadAsStreamAsync().Result)
                    {
                        var memStream = new MemoryStream();
                        stream.CopyTo(memStream);
                        memStream.Position = 0;
                        return memStream.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                ServicePointManager.SecurityProtocol = oldSecurity;
            }
            return null;
        }

        protected Bitmap DownloadImage(string url)
        {
            var bytes = DownloadResource(url);
            var ms = new MemoryStream(bytes);
            return new Bitmap(ms);
        }

        protected string DownloadHtml(string url)
        {
            string html = "";

            if (!HasClient)
            {
                return null;
            }

            var oldSecurity = ServicePointManager.SecurityProtocol;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var pageResult = client.GetAsync(url).Result;
                pageResult.EnsureSuccessStatusCode();
                html = pageResult.Content.ReadAsStringAsync().Result;
                return html;
            }
            catch (Exception e)
            {
                throw new WebException("Couldn't load html for url: " + url + Environment.NewLine + e.Message, e);
            }
            finally
            {
                ServicePointManager.SecurityProtocol = oldSecurity;
            }
        }

        protected HtmlDocument DownloadHtmlDocument(string url)
        {
            var html = DownloadHtml(url);
            var document = new HtmlDocument();
            document.LoadHtml(html);
            return document;
        }

        protected string GetContentTypeFromImage(byte[] image, out byte[] newImage)
        {
            string contentType = null;
            Image temp = null;
            using (MemoryStream stream = new MemoryStream(image))
            {
                temp = Image.FromStream(stream);
                if (ImageFormat.Jpeg.Equals(temp.RawFormat))
                {
                    newImage = image;
                    contentType = "image/jpeg";
                }
                else if (ImageFormat.Png.Equals(temp.RawFormat))
                {
                    newImage = image;
                    contentType = "image/png";
                }
                else if (ImageFormat.Gif.Equals(temp.RawFormat))
                {
                    newImage = image;
                    contentType = "image/gif";
                }
                else
                {
                    //just convert to jpg
                    contentType = "image/jpeg";
                    using (MemoryStream tempStream = new MemoryStream())
                    {
                        temp.Save(tempStream, ImageFormat.Jpeg);
                        newImage = tempStream.ToArray();
                    }
                }
                return contentType;
            }
        }

        protected string SubstringBetween(string s, string prefix, string suffix)
        {
            int pFrom = s.IndexOf(prefix) + prefix.Length;
            int pTo = s.IndexOf(suffix, pFrom);

            if (pFrom < 0 || pTo < 0)
            {
                return null;
            }

            string result = s.Substring(pFrom, pTo - pFrom);
            return result;
        }
        #endregion
    }
}

using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace PC
{
    public static class Help
    {
        public static void GetPageViewWikipedia(List<PageView> list, DateTime date, int hours)
        {
            try
            {
                if (hours == -1)
                {
                    return;
                }
                DateTime current = date.AddHours(-hours);
                string url = ConfigurationManager.AppSettings["url"];
                string file = "pageviews-" + current.Year.ToString() + current.Month.ToString("00") + current.Day.ToString("00") + "-" + current.Hour.ToString("00") + "0000";
                url += current.Year + "/" + current.Year + "-" + current.Month.ToString("00") + "/" + file + ".gz";
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(url);
                httpReq.AllowAutoRedirect = false;
                HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse();
                if (httpRes.StatusCode == HttpStatusCode.OK)
                {
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(url);
                    Stream stream = new MemoryStream(bytes);
                    string path = Path.GetTempPath() + file + ".txt";
                    using (FileStream outputfileStream = File.Create(path))
                    {
                        using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress))
                        {
                            gZipStream.CopyTo(outputfileStream);
                        }
                    }
                    string[] text = File.ReadAllLines(path);
                    int entries = ConfigurationManager.AppSettings["entries"] == "all" ? text.Length : (int.TryParse(ConfigurationManager.AppSettings["entries"], out int result) ? result : 0);
                    entries = entries < 0 ? 0 : entries;
                    for (int i = 0; i < entries; i++)
                    {
                        PageView pageView = new PageView()
                        {
                            date = current,
                            domain_code = text[i].Split()[0],
                            page_title = text[i].Split()[1],
                            max_count_views = int.Parse(text[i].Split()[2])
                        };
                        if (list.Exists(x => x.page_title == pageView.page_title))
                        {
                            list.Find(x => x.page_title == pageView.page_title).date = current;
                            list.Find(x => x.page_title == pageView.page_title).max_count_views = pageView.max_count_views;
                        }
                        else
                        {
                            list.Add(pageView);
                        }
                    }
                    File.Delete(path);
                    GetPageViewWikipedia(list, date, hours - 1);
                }
                else
                {
                    GetPageViewWikipedia(list, date, hours - 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

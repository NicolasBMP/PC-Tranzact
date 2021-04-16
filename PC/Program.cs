using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                List<PageView> list = new List<PageView>();
                int hours = int.TryParse(ConfigurationManager.AppSettings["hours"].ToString(), out int result) ? (result <= 0 ? 0 : result) : 0;
                int show = int.Parse(ConfigurationManager.AppSettings["show"].ToString());
                DateTime date = DateTime.Now;
                Console.WriteLine("-------------------------------------------------------------------------------LOADING[...]");
                Help.GetPageViewWikipedia(list, date, hours);
                list = list.OrderByDescending(x => x.max_count_views).ToList();
                show = show > list.Count ? list.Count : show;
                list.RemoveRange(show - 1, list.Count - show);
                Console.Clear();
                Console.WriteLine("-------------------------------------------------------------------------------------------");
                Console.WriteLine("------------------------------------------BEGINING-----------------------------------------");
                Console.WriteLine("-------------------------------------------------------------------------------------------");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("domain_code           page_title                        max_count_views                              ");
                list.ForEach(x => Console.WriteLine(x.domain_code + x.domain_code.PadRight(20) + x.page_title + x.page_title.PadRight(30) + x.max_count_views + x.max_count_views.ToString().PadRight(20) + Environment.NewLine));
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("# of Entries: " + list.Count);
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("-------------------------------------------------------------------------------------------");
                Console.WriteLine("---------------------------------------------END-------------------------------------------");
                Console.WriteLine("-------------------------------------------------------------------------------------------");
            }
            catch (Exception)
            {
                Console.Clear();
                throw;
            }
        }
    }
}

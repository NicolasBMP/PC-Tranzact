using System;

namespace DTO
{
    public class PageView
    {
        public string domain_code { get; set; } = string.Empty;
        public string page_title { get; set; } = string.Empty;
        public int max_count_views { get; set; } = 0;
        public DateTime date { get; set; }
    }
}

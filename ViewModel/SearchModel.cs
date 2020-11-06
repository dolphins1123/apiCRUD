using System;

namespace apiCRUD.ViewModel
{
    public class SearchModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public int? limit { get; set; }
        public int? offset { get; set; }
        public string orderby { get; set; }
        public string sortby { get; set; }
    }
}
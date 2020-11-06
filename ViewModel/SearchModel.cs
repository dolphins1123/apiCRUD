using System;

namespace apiCRUD.ViewModel
{
    public class SearchModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string name { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int? limit { get; set; }

        /// <summary>
        /// offset
        /// </summary>
        public int? offset { get; set; }

        /// <summary>
        /// 正反排 asc/desc
        /// </summary>
        public string orderby { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string sortby { get; set; }
    }
}
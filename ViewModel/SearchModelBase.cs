namespace apiCRUD.ViewModel
{
    public class SearchModelBase
    {
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
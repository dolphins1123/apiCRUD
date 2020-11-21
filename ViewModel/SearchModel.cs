namespace apiCRUD.ViewModel
{
    /// <summary>
    /// 改版  抽共用
    /// 改收  前端傳送過來的JSON filter 條件
    /// </summary>
    public class SearchModel : SearchModelBase
    {
        /// <summary>
        /// 名稱 .FOR    普通的查詢方式
        /// 其他欄位請自己擴充
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// JSON  query  case ,前端請用 JSON.stringify(filters)
        /// </summary>
        public string filters { get; set; }
    }

    /// <summary>
    /// 傳遞頁數 版本的SearchModel ,pageSize   ,pageIndex 必填
    /// </summary>
    public class QueryModel
    {
        /// <summary>
        /// 每頁筆數
        /// </summary>
        public uint pageSize { get; set; }

        /// <summary>
        /// 第幾頁
        /// </summary>
        public uint pageIndex { get; set; }

        /// <summary>
        /// 正反排 asc/desc
        /// </summary>
        public string orderby { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string sortby { get; set; }

        /// <summary>
        /// 名稱 .FOR    普通的查詢方式
        /// 其他欄位請自己擴充
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// JSON  query  case ,前端請用 JSON.stringify(filters)
        /// </summary>
        public string filters { get; set; }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public uint? limit { get; set; }

        /// <summary>
        /// offset
        /// </summary>
        public uint? offset { get; set; }
    }
}
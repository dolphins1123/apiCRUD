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
}
using System.Collections.Generic;
using apiCRUD.DOMAIN;

namespace apiCRUD.ViewModel
{
    public class ResultModel
    {
        public ResultModel()
        {
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 回傳結果
        /// </summary>
        public Result result { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string error_code { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string message { get; set; }
    }

    /// <summary>
    /// 回傳結果
    /// </summary>
    public class Result
    {
        public Result()
        {
            this.DataList = new List<Customers>();
        }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public int rowCount { get; set; }

        /// <summary>
        /// 清單
        /// </summary>
        public List<Customers> DataList { get; set; }
    }
}
using apiCRUD.DOMAIN;
using System.Collections.Generic;

namespace apiCRUD.ViewModel
{
    public class ReturnModel
    {
        public bool isSuccess { get; set; }

        public ReturnSuccess returnSuccess { get; set; }
        public ReturnFail returnFail { get; set; }
    }




    /// <summary>
    /// 回傳成功
    /// </summary>
    public class ReturnSuccess
    {
        public ReturnSuccess()
        {
        }

        public bool success { get; set; }
        public List<Customers> result { get; set; }

        /// <summary>
        /// 總筆數
        /// </summary>
        public int totalRowCount { get; set; }


        public int totalPage { get; set; }


    }

    /// <summary>
    /// 回傳失敗
    /// </summary>
    public class ReturnFail
    {
        public bool success { get; set; }
        public string error_code { get; set; }
        public string message { get; set; }
    }
}
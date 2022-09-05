using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiCRUD.ViewModel
{
    /// <summary>
    /// 登入資訊
    /// </summary>
    public class LoginRequestModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
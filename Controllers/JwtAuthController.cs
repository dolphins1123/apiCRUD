using apiCRUD.Helper;
using apiCRUD.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace apiCRUD.Controllers
{
    /// <summary>
    /// JWT 驗證
    /// </summary>
    public class JwtAuthController : ApiController
    {

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/JwtAuth/Login")]
        public IHttpActionResult Login(LoginRequestModel model)
        {
            // TODO: 判斷帳密是否存在
            //if (model.Account == "jim" && model.Password == "12345")
            //{
                JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();

                Dictionary<string, Object> claim = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
                claim.Add("Account", "jim");
                claim.Add("Company", "appx");
                claim.Add("Department", "rd");

                string jwtToken = jwtAuthUtil.GenerateToken(claim);

                return this.Ok(new
                {
                    status = true,
                    token = jwtToken
                });
            //}
            //else
            //{

            //    return this.BadRequest();
            //}
        }

        /// <summary>
        /// 測試是否驗證成功
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/JwtAuth/IsAuthenticated")]
        public bool IsAuthenticated()
        {
            return true;
        }
    }
}

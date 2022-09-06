using Jose;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace apiCRUD.Helper
{
    public class JwtAuthUtil
    {
        /// <summary>
        /// 產生token
        /// </summary>
        /// <param name="claim">payload 需透過token傳遞的資料</param>
        /// <returns></returns>
        public string GenerateToken(Dictionary<string, Object> claim = null)
        {
            string tokeySecretKey = ConfigurationManager.AppSettings["tokeySecretKey"];//加解密的key,如果不一樣會無法成功解密

            if (claim == null)
            {
                claim = new Dictionary<string, Object>();
            }

            claim.Add("Exp", DateTime.Now.AddSeconds(Convert.ToInt32("100")).ToString());//Token 時效設定100秒

            //Dictionary<string, Object> claim = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
            //claim.Add("Account", "jim");
            //claim.Add("Company", "appx");
            //claim.Add("Department", "rd");
            //claim.Add("Exp", DateTime.Now.AddSeconds(Convert.ToInt32("100")).ToString());//Token 時效設定100秒

            var payload = claim;
            var token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(tokeySecretKey), JwsAlgorithm.HS512);//產生token

            return token;
        }
    }
}
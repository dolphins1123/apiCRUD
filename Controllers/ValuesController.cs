using NLog;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using LogManager = NLog.LogManager;//寫LOG

namespace apiCRUD.Controllers
{
    public class ValuesController : ApiController
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// CALL 外部API   範例
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get()
        {
            List<Class1> result = new List<Class1>();
            try
            {
                //_url   放到web.config參數
                string _url = "https://od.moi.gov.tw/od/data/api/EA28418E-8956-4790-BAF4-C2D3988266CC?$format=json";
                var client = new HttpClient();
                var _recData = await client.GetAsync(_url);
                _recData.EnsureSuccessStatusCode();
                string responseBody = await _recData.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<List<Class1>>(responseBody );   //Serialize    model >  string 
                logger.Info("  test...Info.");
                logger.Debug("  test Debug...");
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                return Json(result);
            }
            return Json(result);
        }


        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }


    public class Rootobject
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string unit { get; set; }
        public string telephon { get; set; }
        public string fax { get; set; }
        public string address { get; set; }
        public string website { get; set; }
        public string TgosTWD_X { get; set; }
        public string TgosTWD_Y { get; set; }
    }





}
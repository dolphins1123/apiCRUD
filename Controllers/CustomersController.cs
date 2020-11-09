using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
using apiCRUD.DOMAIN;
using apiCRUD.ViewModel;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

namespace apiCRUD.Controllers
{
    /// <summary>
    /// FOR 前端測試CRUD. 用Entities  直接存取就可
    /// </summary>
    public class CustomersController : ApiController
    {
        private NorthwindEntities db = new NorthwindEntities();

        /// <summary>
        /// FOR 前端測試CRUD.  北風資料庫 Customers    資料表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Customer/GetList")]
        [ResponseType(typeof(ResultModel))]
        [SwaggerResponse(HttpStatusCode.OK, "成功 <br>回傳欄位 請參考北風資料庫 Customers  資料表", typeof(ResultModel))]
        public async Task<IHttpActionResult> GetList([FromUri] SearchModel model)
        {
            return await Task.Run(() => this.queryData(model));
        }

        private IHttpActionResult queryData(SearchModel model)
        {
            if (model == null)
            {
                model = new SearchModel()
                {
                    limit = 10,
                    offset = 0,
                    orderby = "asc",
                    sortby = "CustomerID"
                };
            }
            //  query case
            if (!model.offset.HasValue) model.offset = 0;
            if (!model.limit.HasValue) model.limit = 10;
            if (string.IsNullOrEmpty(model.orderby)) model.orderby = "asc";
            if (string.IsNullOrEmpty(model.sortby)) model.sortby = "CustomerID";

            var data = db.Customers.AsQueryable().OrderBy(o => model.orderby).SortBy(model.sortby);

            //通用寫法
            if (!string.IsNullOrEmpty(model.name))
            {
                data = data.Where(x => x.CompanyName == model.name);
            }

            //查詢條件包在 JSON filters  中
            if (!string.IsNullOrEmpty(model.filters))
            {
                Customers qryCase = JsonConvert.DeserializeObject<Customers>(model.filters);

                // 多個欄位
                if (!string.IsNullOrEmpty(qryCase.CustomerID))
                    data = data.Where(x => x.CustomerID == qryCase.CustomerID);

                if (!string.IsNullOrEmpty(qryCase.CompanyName))
                    data = data.Where(x => x.CompanyName == qryCase.CompanyName);

                if (!string.IsNullOrEmpty(qryCase.City))
                    data = data.Where(x => x.City == qryCase.City);

                if (!string.IsNullOrEmpty(qryCase.Phone))
                    data = data.Where(x => x.Phone == qryCase.Phone);
            }

            var dataSource = data.Skip(model.offset.Value).Take(model.limit.Value).ToList();

            if (dataSource.Count > 0)
            {
                ReturnSuccess respSucc = new ReturnSuccess
                {
                    success = true,
                    totalRowCount = data.Count(),
                    totalPage = (int)Math.Ceiling((double)data.Count() / model.limit.Value),
                    result = dataSource
                };
                return this.Json(respSucc);
            }
            else
            {
                ReturnFail respFail = new ReturnFail
                {
                    success = false,
                    error_code = "404",
                    message = "查無資料"
                };

                return this.Json(respFail);
            }
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customers"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/Update")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Update([System.Web.Mvc.Bind(Include = "CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customers model)
        {
            return await Task.Run(() => this.doUpdate(model));
        }

        private IHttpActionResult doUpdate(Customers model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(model).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/Create")]
        public IHttpActionResult Create([FromUri] Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customers);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CustomersExists(customers.CustomerID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = customers.CustomerID }, customers);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/Customer/Delete")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(string id)
        {
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return NotFound();
            }

            //與order 連動.有可能刪除失敗
            db.Customers.Remove(customers);
            db.SaveChanges();

            return Ok(customers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomersExists(string id)
        {
            return db.Customers.Count(e => e.CustomerID == id) > 0;
        }
    }
}
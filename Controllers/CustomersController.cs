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
        /// FOR 前端測試CRUD.  北風資料庫 Customers  資料表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [Route("api/customer")]
        [ResponseType(typeof(ResultModel))]
        [SwaggerResponse(HttpStatusCode.OK, "成功 <br>回傳欄位 請參考北風資料庫 Customers  資料表", typeof(ResultModel))]
        public async Task<IHttpActionResult> GetList(SearchModel model)
        {
            return await Task.Run(() => this.queryData(model));
        }

        private IHttpActionResult queryData(SearchModel model)
        {
            //  query case
            if (!model.offset.HasValue) model.offset = 0;
            if (!model.limit.HasValue) model.limit = 10;
            if (string.IsNullOrEmpty(model.orderby)) model.orderby = "asc";
            if (string.IsNullOrEmpty(model.sortby)) model.sortby = "CustomerID";
            var data = db.Customers.AsQueryable().OrderBy(o => model.orderby).SortBy(model.sortby);
            if (!string.IsNullOrEmpty(model.id))
            {
                data = data.Where(x => x.CustomerID == model.id);
            }

            if (!string.IsNullOrEmpty(model.name))
            {
                data = data.Where(x => x.CompanyName == model.name);
            }

            var dataSource = data.Skip(model.offset.Value).Take(model.limit.Value).ToList();

            if (dataSource.Count > 0)
            {
                ReturnSuccess respSucc = new ReturnSuccess
                {
                    success = true,
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
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Update(string id, Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customers.CustomerID)
            {
                return BadRequest();
            }

            db.Entry(customers).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Create(Customers customers)
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
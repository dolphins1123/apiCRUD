using System;
using System.Collections.Generic;
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
        ///  update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customers"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/Update")]
        [ResponseType(typeof(ResultModel))]
        public async Task<IHttpActionResult> Update(Customers model)
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
            catch (DbUpdateConcurrencyException ex)
            {
                ReturnFail respFail = new ReturnFail
                {
                    success = false,
                    error_code = "500",
                    message = ex.Message
                };

                return this.Json(respFail);
            }

            ReturnSuccess respSucc = new ReturnSuccess
            {
                success = true
            };
            return this.Json(respSucc);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/Create")]
        [ResponseType(typeof(ResultModel))]
        public async Task<IHttpActionResult> Create(Customers model)
        {
            return await Task.Run(() => this.doCreate(model));
        }

        private IHttpActionResult doCreate(Customers model)
        {
            ReturnFail respFail = new ReturnFail
            {
                success = false,
                error_code = "500",
                message = "驗證失敗"
            };

            if (!ModelState.IsValid)
            {
                return this.Json(respFail);
            }

            db.Customers.Add(model);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (CustomersExists(model.CustomerID))
                {
                    return this.Json(respFail);
                }
                else
                {
                    return this.Json(respFail);
                }
            }

            //  CreatedAtRoute("DefaultApi", new { id = model.CustomerID }, model);

            ReturnSuccess respSucc = new ReturnSuccess
            {
                success = true
            };
            return this.Json(respSucc);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="idList">多筆請用, 隔開</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Customer/Delete")]
        [ResponseType(typeof(ResultModel))]
        public async Task<IHttpActionResult> Delete(string idList)
        {
            return await Task.Run(() => this.doDelete(idList));
        }

        private IHttpActionResult doDelete(string idList)
        {
            ReturnFail respFail = new ReturnFail
            {
                success = false,
                error_code = "500",
                message = "驗證失敗"
            };

            var dataSource = new List<Customers>();
            if (idList.Contains(","))
            {
                dataSource = db.Customers.Where(x => idList.Split(',').Contains(x.CustomerID)).ToList();
            }
            else
            {
                dataSource = db.Customers.Where(x => x.CustomerID == idList).ToList();
            }

            if (dataSource.Count < 1)
            {
                return this.Json(respFail);
            }

            //與order 連動.有可能刪除失敗
            try
            {
                db.Customers.RemoveRange(dataSource);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                respFail.message = ex.Message;
                return this.Json(respFail);
            }
            ReturnSuccess respSucc = new ReturnSuccess
            {
                success = true
            };
            return this.Json(respSucc);
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
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Portal.Controllers
{
    [Route("api/[controller]")]
    public class AnalysisTypesController : RootController
    {
        private readonly ILogger<AnalysisTypesController> logger;

    public AnalysisTypesController(NCDCContext context, ILogger<AnalysisTypesController> logger) : base(context)
    {
        this.logger = logger;
    }

            [HttpGet]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination, [FromQuery] AnalysisTypesFilterVM filter)
        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var query = from c in db.AnalysisTypes.Where(c => c.Status != Status.Deleted)
                                              .WhereIf(filter.AnalysisTypeName is not null, c => c.AnalysisTypeName.Contains(filter.AnalysisTypeName))
                            orderby c.AnalysisTypeId descending
                            select new
                            {
                                c.AnalysisTypeId,
                                c.AnalysisTypeName,
                                c.AnalysisTypeCode,
                                CreatedBy = c.CreatedByNavigation.FullName,
                                CreatedOn = c.CreatedOn.ToString("yyyy-MM-dd"),
                                c.Status
                            };

                var totalItems = await query.CountAsync();
                var AnalysisTypes = await query.Skip(pagination.PageSize * (pagination.Page - 1)).Take(pagination.PageSize).ToListAsync();

                var result = new { statusCode = 1, AnalysisTypes, totalItems };

                return Ok(result);
         
        }

        [HttpGet("{id}/details")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> GetDetails(int id)
        {
            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });
          
                var AnalysisType = await (from c in db.AnalysisTypes
                                           where c.AnalysisTypeId == id
                                           select new
                                           {
                                               c.AnalysisTypeId,
                                               c.AnalysisTypeName,
                                               c.AnalysisTypeCode,
                                               c.Description,
                                               CreatedBy = c.CreatedByNavigation.FullName,
                                               CreatedOn = c.CreatedOn.ToString("yyyy-MM-dd"),
                                               UpdatedBy = c.ModifiedByNavigation.FullName,
                                               UpdatedOn = c.ModifiedOn.Value.ToString("yyyy-MM-dd"),
                                               c.Status,
                                           }).SingleOrDefaultAsync();

                if (AnalysisType is null) return NotFound(new { statusCode = "RE0601", message = "لم يتم العثور على  التحليل" });

                var result = new { statusCode = 1, AnalysisType };

                return Ok(result);
           
        }

        [HttpGet("{id}")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Get(int id)
        {
          
            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

                var AnalysisType = await (from m in db.AnalysisTypes
                                           where m.AnalysisTypeId == id && m.Status != Status.Deleted
                                           select m).SingleOrDefaultAsync();


                if (AnalysisType is null) return NotFound(new { statusCode = "RE0602", message = "لم يتم العثور على بيانات التحليل" });

                var result = new { statusCode = 1, AnalysisType };
                return Ok(result);
          
        }

        [HttpDelete("{id}")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Delete(int id)
        {
            
            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

                var AnalysisType = await (from c in db.AnalysisTypes
                                           where c.AnalysisTypeId == id &&
                                            c.Status != Status.Deleted
                                           select c).SingleOrDefaultAsync();

                if (AnalysisType is null) return NotFound(new { statusCode = "RE0603", message = "لم يتم العثور على التحليل" });

                AnalysisType.Status = Status.Deleted;
                AnalysisType.ModifiedBy = UserId();
                AnalysisType.ModifiedOn = DateTime.Now;

                await db.SaveChangesAsync();

                var result = new { statusCode = 1, message = "تم حذف سجل التحليل بنجاح" };

                return Ok(result);          
        }

        [HttpPost]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Add([FromBody] AnalysisTypeVM vm)

        {
           
            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

                var AnalysisTypeNameExist = await (from c in db.AnalysisTypes.Where(c => c.Status != Status.Deleted
                                            && c.AnalysisTypeName.Equals(vm.AnalysisTypeName))
                                                    select c).AnyAsync();

                if (AnalysisTypeNameExist) return BadRequest(new { statusCode = "RE0605", message = "اسم التحليل  موجود مسبقا" });

                var AnalysisTypeCodeExist = await (from c in db.AnalysisTypes.Where(c => c.Status != Status.Deleted
                                         && c.AnalysisTypeCode.Equals(vm.AnalysisTypeCode))
                                                    select c).AnyAsync();

                if (AnalysisTypeCodeExist) return BadRequest(new { statusCode = "RE0605", message = "رمز التحليل  موجود مسبقا" });

                var newAnalysisType = new AnalysisTypes
                {
                    AnalysisTypeName = vm.AnalysisTypeName.Trim(),
                    AnalysisTypeCode = vm.AnalysisTypeCode,
                    Description = vm.Description,
                    CreatedBy = UserId(),
                    CreatedOn = DateTime.Now,
                    Status = Status.Active
                };

                db.Add(newAnalysisType);

                await db.SaveChangesAsync();

                var AnalysisType = new
                {
                    newAnalysisType.AnalysisTypeId,
                    newAnalysisType.AnalysisTypeName,
                    newAnalysisType.AnalysisTypeCode,
                    CreatedBy = UserName(),
                    CreatedOn = newAnalysisType.CreatedOn.ToString("yyyy-MM-dd"),
                    Status = Status.Active,
                };

                var result = new { statusCode = 1, AnalysisType, message = "تم إضافة التحليل  بنجاح" };

                return Ok(result);
           
        }


        [HttpPut("{id}/lock")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Lock(int id)
        {
            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var AnalysisType = await (from c in db.AnalysisTypes
                                           where c.AnalysisTypeId == id &&
                                                 c.Status == Status.Active
                                           select c).SingleOrDefaultAsync();

                if (AnalysisType is null) return NotFound(new { statusCode = "RE0606", message = "لم يتم العثور على التحليل" });

                AnalysisType.Status = Status.Locked;
                AnalysisType.ModifiedBy = UserId();
                AnalysisType.ModifiedOn = DateTime.Now;

                await db.SaveChangesAsync();

                var result = new { statusCode = 1, message = "تم تجميد  التحليل بنجاح" };

                return Ok(result);
         
        }

        [HttpPut("{id}/unlock")]
        [ExceptionCode("EX01001")]

        public async Task<IActionResult> Unlock(int id)
        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var AnalysisType = await (from c in db.AnalysisTypes
                                           where c.AnalysisTypeId == id &&
                                                 c.Status == Status.Locked
                                           select c).SingleOrDefaultAsync();

                if (AnalysisType is null) return NotFound(new { statusCode = "RE0607", message = "لم يتم العثور على التحليل" });

                AnalysisType.Status = Status.Active;
                AnalysisType.ModifiedBy = UserId();
                AnalysisType.ModifiedOn = DateTime.Now;

                await db.SaveChangesAsync();

                var result = new { statusCode = 1, message = "تم تفعيل   التحليل بنجاح" };

                return Ok(result);
         
        }

        [HttpPut("{id}")]
        [ExceptionCode("EX01001")]

        public async Task<IActionResult> Edit(int id, [FromBody] AnalysisTypeVM AnalysisTypeVM)
        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });


            var AnalysisTypeNameExist = await (from c in db.AnalysisTypes.Where(c => c.Status != Status.Deleted && c.AnalysisTypeId != id
                                            && c.AnalysisTypeName.Equals(AnalysisTypeVM.AnalysisTypeName))
                                                    select c).AnyAsync();

                if (AnalysisTypeNameExist) return BadRequest(new { statusCode = "RE0608", message = "اسم التحليل  موجود مسبقا" });


                var AnalysisTypeCodeExist = await (from c in db.AnalysisTypes.Where(c => c.Status != Status.Deleted && c.AnalysisTypeId != id
                                       && c.AnalysisTypeCode.Equals(AnalysisTypeVM.AnalysisTypeCode))
                                                    select c).AnyAsync();

                if (AnalysisTypeCodeExist) return BadRequest(new { statusCode = "RE0605", message = "رمز التحليل  موجود مسبقا" });

                var AnalysisTypesToEdit = await (from c in db.AnalysisTypes
                                                  where c.AnalysisTypeId == id
                                                  select new
                                                  {
                                                      c,
                                                      CreatedBy = c.CreatedByNavigation.FullName,
                                                  }).SingleOrDefaultAsync();

                if (AnalysisTypesToEdit is null) return NotFound(new { statusCode = "RE0609", message = "لم يتم العثور على التحليل " });

                AnalysisTypesToEdit.c.AnalysisTypeName = AnalysisTypeVM.AnalysisTypeName.Trim();
                AnalysisTypesToEdit.c.AnalysisTypeCode = AnalysisTypeVM.AnalysisTypeCode;
                AnalysisTypesToEdit.c.Description = AnalysisTypeVM.Description;
                AnalysisTypesToEdit.c.ModifiedBy = UserId();
                AnalysisTypesToEdit.c.ModifiedOn = DateTime.Now;

                await db.SaveChangesAsync();

                var AnalysisType = new
                {
                    AnalysisTypeId = id,
                    AnalysisTypeVM.AnalysisTypeName,
                    AnalysisTypeVM.AnalysisTypeCode,
                    AnalysisTypesToEdit.CreatedBy,
                    CreatedOn = AnalysisTypesToEdit.c.CreatedOn.ToString("yyyy-MM-dd"),
                    Status = Status.Active
                };

                var result = new { statusCode = 1, AnalysisType, message = "تم تعديل   التحليل بنجاح" };

                return Ok(result);
        
        } 
        [HttpPut("{id}/result")]
        [ExceptionCode("EX01001")]

        public async Task<IActionResult> Result(int id, [FromBody] AnalysisTypeResultVM vm)
        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });


                var AnalysisTypesToEdit = await (from c in db.AnalysisTypes
                                                  where c.AnalysisTypeId == id
                                                  select new
                                                  {
                                                      c,
                                                      AnalysisResults =  c.AnalysisResults.Where(x => x.Status == Status.Active).Select(x => x.ResultTypeId).ToList(),
                                                  }).SingleOrDefaultAsync();

                if (AnalysisTypesToEdit is null) return NotFound(new { statusCode = "RE0609", message = "لم يتم العثور على التحليل " });

                foreach(var itemId in vm.ResultIds)
            {
                if(!AnalysisTypesToEdit.AnalysisResults.Contains(itemId))
                {

                var newResult = new AnalysisResults()
                {
                    AnalysisTypeId = id,
                    ResultTypeId = itemId,
                    Status = Status.Active,
                };
                db.AnalysisResults.Add(newResult);
                }
            }  
            
            
            foreach(var itemId in AnalysisTypesToEdit.AnalysisResults)
            {
                if(!vm.ResultIds.Contains(itemId))
                {
                    var AnalysisResults = (from s in db.AnalysisResults
                                               where s.ResultTypeId == itemId
                                                && s.AnalysisTypeId == id
                                               select s).SingleOrDefault();

                    db.AnalysisResults.Remove(AnalysisResults);
               
                }
            }


                await db.SaveChangesAsync();


                var result = new { statusCode = 1, message = "تمت العملية  بنجاح" };

                return Ok(result);
        
        }
    }
}

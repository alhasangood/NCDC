using Microsoft.AspNetCore.Mvc;

namespace Portal.Controllers
{
    public class ResultTypesController : RootController
    {
        private readonly ILogger<ResultTypesController> logger;

        public ResultTypesController(NCDCContext context, ILogger<ResultTypesController> logger) : base(context)
        {
            this.logger = logger;
        }

        [HttpGet]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination, [FromQuery] ResultTypesFilterVM filter)
        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var query = from c in db.ResultTypes.Where(c => c.Status != Status.Deleted)
                                              .WhereIf(filter.ResultTypeName is not null, c => c.ResultTypeName.Contains(filter.ResultTypeName))
                        orderby c.ResultTypeId descending
                        select new
                        {
                            c.ResultTypeId,
                            c.ResultTypeName,
                            CreatedBy = c.CreatedByNavigation.FullName,
                            CreatedOn = c.CreatedOn.ToString("yyyy-MM-dd"),
                            c.Status
                        };

            var totalItems = await query.CountAsync();
            var ResultTypes = await query.Skip(pagination.PageSize * (pagination.Page - 1)).Take(pagination.PageSize).ToListAsync();

            var result = new { statusCode = 1, ResultTypes, totalItems };

            return Ok(result);

        }

        [HttpGet("{id}/details")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> GetDetails(int id)
        {
            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var AnalysisType = await (from c in db.ResultTypes
                                      where c.ResultTypeId == id
                                      select new
                                      {
                                          c.ResultTypeId,
                                          c.ResultTypeName,                                 
                                          CreatedBy = c.CreatedByNavigation.FullName,
                                          CreatedOn = c.CreatedOn.ToString("yyyy-MM-dd"),
                                          UpdatedBy = c.ModifiedByNavigation.FullName,
                                          UpdatedOn = c.ModifiedOn.Value.ToString("yyyy-MM-dd"),
                                          c.Status,
                                      }).SingleOrDefaultAsync();

            if (AnalysisType is null) return NotFound(new { statusCode = "RE0601", message = "لم يتم العثور على  النتيجة" });

            var result = new { statusCode = 1, AnalysisType };

            return Ok(result);

        }

        [HttpGet("{id}")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Get(int id)
        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var AnalysisType = await (from m in db.ResultTypes
                                      where m.ResultTypeId == id && m.Status != Status.Deleted
                                      select m).SingleOrDefaultAsync();


            if (AnalysisType is null) return NotFound(new { statusCode = "RE0602", message = "لم يتم العثور على بيانات النتيجة" });

            var result = new { statusCode = 1, AnalysisType };
            return Ok(result);

        }

        [HttpDelete("{id}")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Delete(int id)
        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var AnalysisType = await (from c in db.ResultTypes
                                      where c.ResultTypeId == id &&
                                       c.Status != Status.Deleted
                                      select c).SingleOrDefaultAsync();

            if (AnalysisType is null) return NotFound(new { statusCode = "RE0603", message = "لم يتم العثور على النتيجة" });

            AnalysisType.Status = Status.Deleted;
            AnalysisType.ModifiedBy = UserId();
            AnalysisType.ModifiedOn = DateTime.Now;

            await db.SaveChangesAsync();

            var result = new { statusCode = 1, message = "تم حذف سجل النتيجة بنجاح" };

            return Ok(result);
        }

        [HttpPost]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Add([FromBody] ResultTypeVM vm)

        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var ResultTypeNameExist = await (from c in db.ResultTypes.Where(c => c.Status != Status.Deleted
                                        && c.ResultTypeName.Equals(vm.ResultTypeName))
                                               select c).AnyAsync();

            if (ResultTypeNameExist) return BadRequest(new { statusCode = "RE0605", message = "اسم النتيجة  موجود مسبقا" });

          

            var newAnalysisType = new ResultTypes
            {
                ResultTypeName = vm.ResultTypeName.Trim(),
                CreatedBy = UserId(),
                CreatedOn = DateTime.Now,
                Status = Status.Active
            };

            db.Add(newAnalysisType);

            await db.SaveChangesAsync();

            var AnalysisType = new
            {
                newAnalysisType.ResultTypeId,
                newAnalysisType.ResultTypeName,
                CreatedBy = UserName(),
                CreatedOn = newAnalysisType.CreatedOn.ToString("yyyy-MM-dd"),
                Status = Status.Active,
            };

            var result = new { statusCode = 1, AnalysisType, message = "تم إضافة النتيجة  بنجاح" };

            return Ok(result);

        }


        [HttpPut("{id}/lock")]
        [ExceptionCode("EX01001")]
        public async Task<IActionResult> Lock(int id)
        {
            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var AnalysisType = await (from c in db.ResultTypes
                                      where c.ResultTypeId == id &&
                                            c.Status == Status.Active
                                      select c).SingleOrDefaultAsync();

            if (AnalysisType is null) return NotFound(new { statusCode = "RE0606", message = "لم يتم العثور على النتيجة" });

            AnalysisType.Status = Status.Locked;
            AnalysisType.ModifiedBy = UserId();
            AnalysisType.ModifiedOn = DateTime.Now;

            await db.SaveChangesAsync();

            var result = new { statusCode = 1, message = "تم تجميد  النتيجة بنجاح" };

            return Ok(result);

        }

        [HttpPut("{id}/unlock")]
        [ExceptionCode("EX01001")]

        public async Task<IActionResult> Unlock(int id)
        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });

            var AnalysisType = await (from c in db.ResultTypes
                                      where c.ResultTypeId == id &&
                                            c.Status == Status.Locked
                                      select c).SingleOrDefaultAsync();

            if (AnalysisType is null) return NotFound(new { statusCode = "RE0607", message = "لم يتم العثور على النتيجة" });

            AnalysisType.Status = Status.Active;
            AnalysisType.ModifiedBy = UserId();
            AnalysisType.ModifiedOn = DateTime.Now;

            await db.SaveChangesAsync();

            var result = new { statusCode = 1, message = "تم تفعيل   النتيجة بنجاح" };

            return Ok(result);

        }

        [HttpPut("{id}")]
        [ExceptionCode("EX01001")]

        public async Task<IActionResult> Edit(int id, [FromBody] ResultTypeVM AnalysisTypeVM)
        {

            // if (!await HasPermission(DashboardPermissions.view)) return BadRequest(new { statusCode = "AE33001", message = AppMessages.noPermission });


            var ResultTypeNameExist = await (from c in db.ResultTypes.Where(c => c.Status != Status.Deleted && c.ResultTypeId != id
                                            && c.ResultTypeName.Equals(AnalysisTypeVM.ResultTypeName))
                                               select c).AnyAsync();

            if (ResultTypeNameExist) return BadRequest(new { statusCode = "RE0608", message = "اسم النتيجة  موجود مسبقا" });



            var ResultTypesToEdit = await (from c in db.ResultTypes
                                             where c.ResultTypeId == id
                                             select new
                                             {
                                                 c,
                                                 CreatedBy = c.CreatedByNavigation.FullName,
                                             }).SingleOrDefaultAsync();

            if (ResultTypesToEdit is null) return NotFound(new { statusCode = "RE0609", message = "لم يتم العثور على النتيجة " });

            ResultTypesToEdit.c.ResultTypeName = AnalysisTypeVM.ResultTypeName.Trim();
            ResultTypesToEdit.c.ModifiedBy = UserId();
            ResultTypesToEdit.c.ModifiedOn = DateTime.Now;

            await db.SaveChangesAsync();

            var AnalysisType = new
            {
                ResultTypeId = id,
                AnalysisTypeVM.ResultTypeName,
                ResultTypesToEdit.CreatedBy,
                CreatedOn = ResultTypesToEdit.c.CreatedOn.ToString("yyyy-MM-dd"),
                Status = Status.Active
            };

            var result = new { statusCode = 1, AnalysisType, message = "تم تعديل   النتيجة بنجاح" };

            return Ok(result);

        }
    }
}

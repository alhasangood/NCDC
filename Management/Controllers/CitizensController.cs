using Common;
using Management.Services;
using Management.ViewModels;
using Managment.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Controllers
{
    
        [Route("api/[controller]")]
        public class CitizensController : RootController
        {
        private readonly ILogger<CitizensController> logger;

        public CitizensController(NCDCContext context, ILogger<CitizensController> logger) : base(context)
            {
            this.logger = logger;
        }


        [HttpGet("GetCitizens")]
        public async Task<IActionResult> GetCitizens(int pageNo, int pageSize, string searchByName, long NationalityNum)
        {
            try
            {
                //if (!GetUserPermission("09000"))
                //{
                //    return BadRequest(new { StatusCode = 919001, result = noPermission });
                //}

                if (pageNo <= 0)
                {
                    return BadRequest(new { StatusCode = 919002, result = "يرجي التأكد من البيانات" });
                }
                if (pageSize <= 0)
                {
                    return BadRequest(new { StatusCode = 919003, result = "يرجي التأكد من البيانات" });
                }

                var CitizensQuery = from c in db.Citizens
                                 where c.Status != Status.Deleted
                                 select c;

                if (!string.IsNullOrWhiteSpace(searchByName))
                {
                    CitizensQuery = from c in CitizensQuery
                                 where c.FullName.Contains(searchByName)
                                 select c;
                }

                if (NationalityNum >0 )
                {
                    CitizensQuery = from c in CitizensQuery
                                 where c.NationalityId == NationalityNum
                                    select c;
                }
                var Citizens = await (from c in CitizensQuery
                                   orderby c.CitizenId descending
                                   select new
                                   {
                                       c.FullName,
                                       c.Employee,
                                       c.Gender,
                                       c.NationalityId,
                                       c.CreatedOn,
                                       c.Status
                                   }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

                var totalItems = await CitizensQuery.CountAsync();

                return Ok(new { StatusCode = 0, result = new { Citizens, totalItems } });
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0003", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }


        [HttpPost("AddCitizen")]
        public async Task<IActionResult> AddCitizen([FromBody] CitizenData CitizenInfo)
    {
        try
        {       

            if (string.IsNullOrWhiteSpace(CitizenInfo.FullName))
            {
                return BadRequest(new { statusCode = 919006, result = "الرجاء التأكد من ادخال اسم المستخدم" });

            }
            if (CitizenInfo.FullName.Length > 50)
            {
                return BadRequest(new { statusCode = 919007, result = "الرجاء ادخال اسم مستخدم لا يتجاوز 50 حرف" });
            }
          
          
            if (!string.IsNullOrWhiteSpace(CitizenInfo.PhoneNo))
            {
                if (CitizenInfo.PhoneNo.Length < 9 || CitizenInfo.PhoneNo.Length > 10)
                {
                    return BadRequest(new { statusCode = 919010, result = "الرجاء ادخال رقم الهاتف بصيغة 9 أو 10 أرقام" });
                }
            }

            var checkCitizensName = await (from c in db.Citizens
                                        where c.FullName == CitizenInfo.FullName
                                        && c.Status != Status.Deleted
                                        select c).CountAsync();

            if (checkCitizensName > 0)
            {
                return BadRequest(new { statusCode = 919012, result = "اسم الدخول مستخدم مسبقا الرجاء ادخال اسم دخول اخر" });
            }
            Citizens addCitizen = new Citizens()
            {
                FullName = CitizenInfo.FullName,
                RegistrationNo = CitizenInfo.RegistrationNo,
                LaboratoryId = CitizenInfo.LaboratoryId,
                NationalNo = CitizenInfo.NationalNo,
                BirthDate = CitizenInfo.BirthDate,
                Gender = CitizenInfo.Gender,
                NationalityId = CitizenInfo.NationalityId,
                MotherName = CitizenInfo.MotherName,
                IdentityType = CitizenInfo.IdentityType,
                PassportNo = CitizenInfo.PassportNo,
                PhoneNo = CitizenInfo.PassportNo,
                Address = CitizenInfo.Address,
                EmployeePlace = CitizenInfo.EmployeePlace,
                Employee = CitizenInfo.Employee,
                OccupationId = CitizenInfo.OccupationId,
                PersonPhoto = CitizenInfo.PersonPhoto,
                FamilyStatus = CitizenInfo.FamilyStatus,
                RegistryNo = CitizenInfo.RegistryNo,
                CreatedBy = UserId(),
                CreatedOn = DateTime.Now,
                Status = Status.Active
            };
            await db.Citizens.AddAsync(addCitizen);

            await db.SaveChangesAsync();

            var addedCitizen = await (from c in db.Citizens
                                   where c.CitizenId == addCitizen.CitizenId
                                       select c).SingleOrDefaultAsync();

            return Ok(new { StatusCode = 0, result = new { message = "تم إضافة تسجيل المواطن بنجاح", addedCitizen } });
        }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0503", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }
        [HttpGet("GetCitizenForEdit")]
        public async Task<IActionResult> GetCitizenForEdit(int citizenId)
        {
            try
            {


                if (citizenId < 0)
                {
                    return BadRequest(new { StatusCode = 919047, result = "يرجي التأكد من بيانات المستخدم." });
                }
                var citizenInfo = await (from c in db.Citizens
                                      where c.CitizenId == citizenId
                                         && c.Status != Status.Deleted
                                    select c).SingleOrDefaultAsync();

                return Ok(new { statusCode = 0, result = new { citizenInfo } });

            }
        catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0503", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpPut("EditCitizen")]
        public async Task<IActionResult> EditCitizen([FromBody] CitizenData CitizenInfo)
        {
            try
            {

                //if (!GetUserPermission("09003"))
                //{
                //    return BadRequest(new { StatusCode = 919018, result =noPermission });
                //}
                if (CitizenInfo.CitizenId <= 0)
                {
                    return BadRequest(new { StatusCode = 919019, result = "يرجي اختيار المستخدم." });
                }

                var checkFullName = await (from c in db.Citizens
                                            where c.FullName == CitizenInfo.FullName
                                            && c.Status != Status.Deleted
                                            select c).CountAsync();

                if (checkFullName > 0)
                {
                    return BadRequest(new { statusCode = 919012, result = "اسم المواطن مستخدم مسبقا الرجاء ادخال اسم مواطن اخر" });
                }

                var citizenData = await (from c in db.Citizens
                                      where c.CitizenId == CitizenInfo.CitizenId
                                      select c).SingleOrDefaultAsync();

                if (citizenData == null)
                {
                    return BadRequest(new { statusCode = 919030, result = "لم يتم العثور على المواطن" });
                }

                citizenData.FullName = CitizenInfo.FullName;
                citizenData.RegistrationNo = CitizenInfo.RegistrationNo;
                citizenData.LaboratoryId = CitizenInfo.LaboratoryId;
                citizenData.NationalNo = CitizenInfo.NationalNo;
                citizenData.BirthDate = CitizenInfo.BirthDate;
                citizenData.Gender = CitizenInfo.Gender;
                citizenData.NationalityId = CitizenInfo.NationalityId;
                citizenData.MotherName = CitizenInfo.MotherName;
                citizenData.IdentityType = CitizenInfo.IdentityType;
                citizenData.PassportNo = CitizenInfo.PassportNo;
                citizenData.PhoneNo = CitizenInfo.PassportNo;
                citizenData.Address = CitizenInfo.Address;
                citizenData.EmployeePlace = CitizenInfo.EmployeePlace;
                citizenData.Employee = CitizenInfo.Employee;
                citizenData.OccupationId = CitizenInfo.OccupationId;
                citizenData.PersonPhoto = CitizenInfo.PersonPhoto;
                citizenData.FamilyStatus = CitizenInfo.FamilyStatus;
                citizenData.RegistryNo = CitizenInfo.RegistryNo;
                citizenData.ModifiedBy = UserId();
                citizenData.ModifiedOn = DateTime.Now;


                await db.SaveChangesAsync();

                return Ok(new { StatusCode = 0, result = new { result = "تم تعديل المواطن بنجاح" } });
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0503", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpPut("LockCitizen")]
        public async Task<IActionResult> LockCitizen(int citizenId)
        {
            try
            {
                if (citizenId <= 0)
                {
                    return BadRequest(new { statusCode = 919028, result = "الرجاء التأكد من البيانات " });
                }

                //if (!GetUserPermission("09004"))
                //{
                //    return BadRequest(new { statusCode = 919029, result = "ليس لديك الصلاحيات اللازمة لتنفيذ الامر" });
                //}

                var Citizen = await (from c in db.Citizens
                                  where c.CitizenId == citizenId
                                  && c.Status != Status.Deleted
                                  select c).SingleOrDefaultAsync();

                if (Citizen == null)
                {
                    return BadRequest(new { statusCode = 919030, result = "لم يتم العثور على المستخدم" });
                }

                if (Citizen.Status == Status.Locked)
                {
                    return BadRequest(new { statusCode = 919031, result = "تم تجميد المواطن مسبقا" });
                }

                Citizen.Status = Status.Locked;
                Citizen.ModifiedBy = UserId();
                Citizen.ModifiedOn = DateTime.Now;
                await db.SaveChangesAsync();

                return Ok(new { statusCode = 0, result = new { result = "تم تجميد المواطن بنجاح" } });
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0503", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpPut("UnlockCitizen")]
        public async Task<IActionResult> UnlockCitizen(int citizenId)
        {
            try
            {
                if (citizenId <= 0)
                {
                    return BadRequest(new { statusCode = 919033, result = "الرجاء التأكد من البيانات " });
                }

                //if (!GetUserPermission("09005"))
                //{
                //    return BadRequest(new { statusCode = 919034, result = "ليس لديك الصلاحيات اللازمة لتنفيذ الامر" });
                //}

                var Citizen = await (from c in db.Citizens
                                     where c.CitizenId == citizenId
                                     && c.Status != Status.Deleted
                                     select c).SingleOrDefaultAsync();


                if (Citizen == null)
                {
                    return BadRequest(new { statusCode = 919035, result = "لم يتم العثور على المواطن" });
                }

                if (Citizen.Status == Status.Active)
                {
                    return BadRequest(new { statusCode = 919036, result = "هذا المواطن مفعل مسبقا" });
                }

                Citizen.Status = Status.Active;
                Citizen.ModifiedBy = UserId();
                Citizen.ModifiedOn = DateTime.Now;
                await db.SaveChangesAsync();

                return Ok(new { statusCode = 0, result = new { result = "تم فك تجميد المواطن بنجاح" } });
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0503", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }

        [HttpDelete("DeleteCitizen")]
        public async Task<IActionResult> DeleteCitizen(int citizenId)
        {
            try
            {
                if (citizenId <= 0)
                {
                    return BadRequest(new { statusCode = 919038, result = "الرجاء التأكد من البيانات " });
                }

                //if (!GetUserPermission("09006"))
                //{
                //    return BadRequest(new { statusCode = 919039, result = "ليس لديك الصلاحيات اللازمة لتنفيذ الامر" });
                //}

                var citizen = await (from c in db.Citizens
                                  where c.CitizenId == citizenId
                                  select c).SingleOrDefaultAsync();

                if (citizen == null)
                {
                    return BadRequest(new { statusCode = 919040, result = "لم يتم العثور على المواطن" });
                }

                if (citizen.Status == Status.Deleted)
                {
                    return BadRequest(new { statusCode = 919041, result = "هذا المواطن محذوف مسبقا" });
                }

                citizen.Status = Status.Deleted;
                citizen.ModifiedBy = UserId();
                citizen.ModifiedOn = DateTime.Now;
                await db.SaveChangesAsync();

                return Ok(new { statusCode = 0, result = new { result = "تم حذف المواطن بنجاح" } });
            }
            catch (Exception ex)
            {
                var log = Logging.CreateLog("EX0503", HttpContext);

                logger.LogCritical(ex, "{@errorCode} {@method} {@path} {@queryString} {@userName}",
                                        log.ErrorCode, log.Method, log.Path, log.QueryString, log.UserName);
                return StatusCode(500, new { statusCode = log.ErrorCode, message = AppMessages.ServerError });
            }
        }


    }
}

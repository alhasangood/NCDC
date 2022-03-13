using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Services
{
    public class Logging
    {
        public static Log CreateLog(string errorCode, HttpContext ctx)
        {
            return new Log(ErrorCode: errorCode,
                            Method: ctx.Request.Method,
                            Path: ctx.Request.Path.Value,
                            QueryString: ctx.Request.QueryString.Value,
                            UserName: ctx.User.Claims.ToList().Where(p => p.Type == "UserName").Select(p => p.Value).SingleOrDefault()
                );
        }
    }

    public record Log(string ErrorCode, string Method, string Path, string QueryString, string UserName);


}
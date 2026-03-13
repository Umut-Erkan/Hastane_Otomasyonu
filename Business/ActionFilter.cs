using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hastane_Otomasyonu.Business
{
    public class ActionFilter : IAuthorizationFilter
    {   
        private readonly IConfiguration _config;
        
        public ActionFilter(IConfiguration config)
    {
        _config = config;
    }
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string secret = context.HttpContext.Request.Headers["Secret"]; // kullanıcının headerindeki Secret karşılığı

            if(secret != _config["AdminSecurity:SecretKey"]) // Gelen Secret'i benim istediğimle karşılaştırma
            {
                 context.Result = new ForbidResult();
            }

        }
        
    }
}

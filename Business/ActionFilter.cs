using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hastane_Otomasyonu.Business
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ActionFilter : Attribute
    {   
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ActionFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
        // Kapamak için kullanacağımız secret
        private const string Secret = "Super_Secret_Value_123";

        // Secret'i requestin headerinden al

        var response = _httpContextAccessor.HttpContext.Response;


    }
}
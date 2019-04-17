using EAD.CRM.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAD.CRM.Web.Infrastructure
{
    public class DashboardExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IEmailService _emailService;
        public DashboardExceptionFilter(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await _emailService.SendEmail("developer@example.com", context.Exception.Message, context.Exception.ToString());
            await base.OnExceptionAsync(context);
        }
    }
}

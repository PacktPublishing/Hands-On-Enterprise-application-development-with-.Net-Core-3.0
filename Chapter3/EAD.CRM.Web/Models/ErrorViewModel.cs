using System;

namespace EAD.CRM.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int ErrorCode { get; set; }
    }
}
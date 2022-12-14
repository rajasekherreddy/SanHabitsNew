using System;
using System.Collections.Generic;
using System.Text;

namespace HappinessIndex.Models
{
    public class ApiResult
    {
        public bool status { get; set; }
        public String message { get; set; }
        public object data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Business.Validation
{
    public class ValidationError
    {
        public string ErrorMessage { get; set; }
        public List<string> PropertyNames { get; set; }
    }
}

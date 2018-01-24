using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Business.Validation
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
            : base("Forbidden")
        {
        }
    }
}

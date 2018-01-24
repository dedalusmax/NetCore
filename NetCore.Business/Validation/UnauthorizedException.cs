using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Business.Validation
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base("Unauthorized")
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Business.Validation
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("Object not found")
        {
        }
    }
}

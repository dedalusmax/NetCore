using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCore.Business.Validation
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException(IEnumerable<ValidationError> errors = null)
            : base("Invalid data")
        {
            Errors = errors ?? Enumerable.Empty<ValidationError>();
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}

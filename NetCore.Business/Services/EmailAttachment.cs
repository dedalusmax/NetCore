using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Business.Services
{
    public class EmailAttachment
    {
        public string type { get; set; }

        public string name { get; set; }

        public byte[] content { get; set; }
    }
}

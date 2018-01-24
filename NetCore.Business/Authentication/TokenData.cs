using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Business.Authentication
{
    public class TokenData
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public DateTime? IssuedAt { get; set; }
    }
}

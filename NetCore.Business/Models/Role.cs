using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Business.Models
{
    public class Role
    {
        public long Id { get; set; }
    }

    public class RoleBase
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}

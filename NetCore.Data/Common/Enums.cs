using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Data.Common
{
    public enum RoleRelationCount
    {
        None = 0,
        Single = 1,
        Multiple = 2
    }

    public enum EmailTemplateType
    {
        RequestPasswordReset = 1,
        Save = 2,
        MarginsApproval = 3,
        ProCareApproval = 4,
        CreditRequest = 5,
        Contract = 6,
        Offer = 7
    }
}

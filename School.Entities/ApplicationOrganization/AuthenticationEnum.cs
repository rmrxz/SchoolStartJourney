using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.ApplicationOrganization
{
    /// <summary>
    /// 用户学生身份认证状态
    /// </summary>
    public enum AuthenticationEnum
    {
        //等待认证
        Wait,
        //认证失败
        Fail,
        //认证通过
        Success
    }
}

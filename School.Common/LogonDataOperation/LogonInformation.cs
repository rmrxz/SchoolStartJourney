using System;
using System.Collections.Generic;
using System.Text;

namespace School.Common.LogonDataOperation
{
    /// <summary>
    /// 基本登录信息
    /// </summary>
    public class LogonInformation
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } 

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}

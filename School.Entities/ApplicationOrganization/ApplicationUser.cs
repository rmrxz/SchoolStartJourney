using Microsoft.AspNetCore.Identity;
using School.Entities.Attachments;
using School.Entities.BusinessOrganization;
using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.Entities.ApplicationOrganization
{
    /// <summary>
    /// 用户
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        //public Guid ID { get; set; }
        /// <summary>
        /// 用户名(显示名称)
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 用户介绍
        /// </summary>
        [StringLength(400)]
        public string Description { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string SortCode { get; set; }
        ///// <summary>
        ///// 学号
        ///// </summary>
        //public string StudentID { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string UserAddress { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public BusinessImage Avatar { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday { get; set; }
        ///// <summary>
        ///// 电子邮箱
        ///// </summary>
        //public string Email { get; set; }
        /// <summary>
        ///注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 所在学校
        /// </summary>
        public string School{ get; set; }
        /// <summary>
        /// 所在学校地址
        /// </summary>
        public string SchoolAddress { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public AnJurisdiction Power { get; set; }

        /// <summary>
        /// 关联的实际个人
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// 学生身份认证
        /// </summary>
        public virtual AuthenticationEnum Authentication { get; set; }

        /// <summary>
        /// 数据更改时间
        /// </summary>
        public virtual DateTime ChangeDateTime { get; set; }


        public ApplicationUser() : base()
        {
            TwoFactorEnabled = true;
            Birthday = DateTime.Now;
            ChangeDateTime = DateTime.Now;
            this.Id = Guid.NewGuid().ToString();
        }
        public ApplicationUser(string userName) : base(userName)
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserName = userName;
        }

    }
}

using School.Entities.Attachments;
using System;
using System.Collections.Generic;
using System.Text;
using School.Entities.ApplicationOrganization;

namespace School.ViewModels.ApplicationOrganization.ApplicationUsers
{
    public class ApplicationUserInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户介绍
        /// </summary>
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
        /// 学号
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string UserAddress { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 所在学校
        /// </summary>
        public string School { get; set; }
        /// <summary>
        /// 所在学校地址
        /// </summary>
        public string SchoolAddress { get; set; }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="user"></param>
        public ApplicationUserInput(ApplicationUser user)
        {
            if (user == null)
            {
                user = new ApplicationUser();
            }
            this.ID = Guid.Parse(user.Id);
            this.Name = user.Name;
            this.Description = user.Description;
            this.SortCode = user.SortCode;
            this.QQ = user.QQ;
            this.MobileNumber = user.MobileNumber;
            this.Sex = user.Sex.ToString();
            this.UserAddress = user.UserAddress;
            this.School = user.School;
            this.SchoolAddress = user.SchoolAddress;
            this.Birthday =Convert.ToDateTime(user.Birthday.ToString("yyyy-MM-dd"));
            this.Email = user.Email;
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="user"></param>
        public void MapTo(Entities.ApplicationOrganization.ApplicationUser user)
        {
            user.Id = this.ID.ToString();
            user.Name = this.Name;
            user.Description = this.Description;
            user.SortCode = this.SortCode;
            user.QQ = this.QQ;
            user.MobileNumber = this.MobileNumber;
            user.Sex =Convert.ToBoolean(this.Sex);
            user.UserAddress = this.UserAddress;
            user.School = this.School;
            user.SchoolAddress = this.SchoolAddress;
            user.Birthday = this.Birthday;
            user.Email = this.Email;
        }
    }
}

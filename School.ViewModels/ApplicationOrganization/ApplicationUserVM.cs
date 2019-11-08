using School.Common.JsonModels;
using School.Common.ViewModelComponents;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.BusinessOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.ViewModels.ApplicationOrganization
{
    public class ApplicationUserVM
    {
        public Guid ID { get; set; }
        public string OrderNumber { get; set; } // 列表时候需要的序号
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Required(ErrorMessage = "显示名不能为空。")]
        [Display(Name = "名称")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string Name { get; set; }

        [Required(ErrorMessage = "显示名不能为空。")]
        [Display(Name = "名称")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string UserName { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Display(Name = "用户内部编码")]
        [StringLength(24, ErrorMessage = "用户内部编码输入长度超出限制，只限24个字符以内。")]
        public string SortCode { get; set; }

        //[Display(Name = "学号")]
        //[StringLength(16, ErrorMessage = "学号输入长度超出限制，只能输入16个字符以内。", MinimumLength = 16)]
        //public string StudentID { get; set; }

        [Required(ErrorMessage = "必须给出密码")]
        [Display(Name = "密码")]
        [StringLength(11, ErrorMessage = "密码输入长度超出限制，只能输入11个字符以内。", MinimumLength = 11)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "必须给出重复密码。")]
        [Display(Name = "密码")]
        [Compare("Password", ErrorMessage = "密码和重复密码不匹配")]
        [StringLength(16, ErrorMessage = "密码输入长度超出限制，只能输入16个字符以内。", MinimumLength = 16)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "归属用户组列表")]
        public List<string> RoleItemIDCollection { get; set; }
        [Display(Name = "归属用户组")]
        public string RoleItemNameString { get; set; }
        [PlainFacadeItemSpecification("RoleItemIDCollection")]
        public List<PlainFacadeItem> RoleItemColection { get; set; }

        [Display(Name = "归属部门")]
        public string DepartmentName { get; set; }

        [Display(Name = "手机")]
        [RegularExpression(@"^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\d{8}$", ErrorMessage = "非法的移动电话格式。")]
        public string MobileNumber { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex { get; set; }

        [Display(Name = "用户地址")]
        [StringLength(60, ErrorMessage = "输入地址超出长度，只能输入60个字符以内", MinimumLength = 60)]
        public string UserAddress { get; set; }

        [Display(Name = "用户注册时间")]
        public DateTime RegisterTime { get; set; }

        [Display(Name = "学校")]
        [StringLength(30, ErrorMessage = "输入学校超出长度，只能输入30个字符以内", MinimumLength = 30)]
        public string School { get; set; }

        [Display(Name = "学校地址")]
        [StringLength(60, ErrorMessage = "输入学校地址超出长度，只能输入60个字符以内", MinimumLength = 60)]
        public string SchoolAddress { get; set; }

        [Display(Name = "电子邮件")]
        [StringLength(24, ErrorMessage = "输入邮箱超出长度，只能输入24个字符以内")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "非法的电子邮件格式。")]
        public string EMail { get; set; }

        [Display(Name = "关联人员")]
        public string PersonID { get; set; }

        [Display(Name = "关联人员")]
        public string PersonName { get; set; }

        [PlainFacadeItemSpecification("PersonID")]
        public List<PlainFacadeItem> PersonItemCollection { get; set; }

        /// <summary>
        /// 用户被禁用状态
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleID { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public BusinessImage AvatarPath { get; set; }
        

        public Person Persons { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 爱好列表
        /// </summary>
        public List<Hobby> Hobbys { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        public ApplicationUserVM()
        { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="user"></param>
        public ApplicationUserVM(School.Entities.ApplicationOrganization.ApplicationUser user)
        {
            this.ID = Guid.Parse(user.Id);
            this.Name = user.Name;
            this.UserName = user.UserName;
            this.Description = user.Description;
            this.SortCode = user.SortCode;
            this.MobileNumber = user.MobileNumber;
            this.Sex = user.Sex;
            this.QQ = user.QQ;
            this.UserAddress = user.UserAddress;
            this.AvatarPath = user.Avatar;
            this.RegisterTime = user.RegisterTime;
            this.School = user.School;
            this.SchoolAddress = user.SchoolAddress;
            this.Persons = user.Person;
            this.Birthday = Convert.ToDateTime(user.Birthday.ToString("yyyy-MM-dd"));
            this.TwoFactorEnabled = user.TwoFactorEnabled;
        }
        public void MapToOm(School.Entities.ApplicationOrganization.ApplicationUser user)
        {
            user.Id = this.ID.ToString();
            user.Name = this.Name;
            user.UserName = this.UserName;
            user.Description = this.Description;
            user.SortCode = this.SortCode;
            user.MobileNumber = this.MobileNumber;
            user.Sex = this.Sex;
            user.UserAddress = this.UserAddress;
            user.Avatar = this.AvatarPath;
            user.QQ = this.QQ;
            user.School = this.School;
            user.SchoolAddress = this.SchoolAddress;
            user.Person = this.Persons;
        }
    }
}

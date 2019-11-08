using School.Common.JsonModels;
using School.Common.ViewModelComponents;
using School.DataAccess.SqlServer.Utilities;
using School.Entities.BusinessOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.ViewModels.BusinessOrganization
{
    //视图模型使于实体和方法之间的关联
   public class PersonVM
    {
        [Key]
        public Guid ID { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        public string SortCode { get; set; }

        //[Required(ErrorMessage = "必须选择人员归属部门。")]
        [Display(Name = "归属部门")]
        public string ParentItemID { get; set; }

        [Display(Name = "归属部门")]
        public SelfReferentialItem ParentItem { get; set; }

        [SelfReferentialItemSpecification("ParentItemID")]
        public List<SelfReferentialItem> ParentItemCollection { get; set; }

        [ListItemSpecification("<i class='icon-pictures'></i>", "01", 40, false)]
        [StringLength(50)]
        public string PersonPhotoPath { get; set; }

        //[Required(ErrorMessage = "工号不能为空值。")]
        [Display(Name = "工号")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string EmployeeCode { get; set; }

        //[Required(ErrorMessage = "姓氏不能为空值。")]
        [Display(Name = "姓氏")]
        [StringLength(6, ErrorMessage = "你输入的数据超出限制6个字符的长度。")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "名字不能为空值。")]
        [Display(Name = "名字")]
        [StringLength(6, ErrorMessage = "你输入的数据超出限制6个字符的长度。")]
        public string LastName { get; set; }

        [Display(Name = "性别")]
        public bool Sex { get; set; }

        [Display(Name = "性别")]
        [ListItemSpecification("性别", "04", 50, false)]
        public string SexString { get; set; }

        [PlainFacadeItemSpecification("Sex")]
        public List<PlainFacadeItem> SexSelector { get; set; }

        [Display(Name = "出生日期")]
        public DateTime Birthday { get; set; }

        [ListItemSpecification("出生日期", "08", 100, false)]
        [Display(Name = "出生日期")]
        public string BirthdayString { get; set; }

        [Display(Name = "固定电话")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制20个字符的长度。")]
        public string FixedTelephone { get; set; }

        [Required(ErrorMessage = "移动电话不能为空值。")]
        [Display(Name = "移动电话")]
        [StringLength(20, ErrorMessage = "你输入的数据超出限制6个字符的长度。")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "电子邮件不能为空值。")]
        [Display(Name = "电子邮件")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制6个字符的长度。")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "非法的电子邮件格式。")]
        public string Email { get; set; }

        // [Required(ErrorMessage = "身份证件编号不能为空值。")]
        [Display(Name = "身份证件编号")]
        [StringLength(50, ErrorMessage = "你输入的数据超出限制50个字符的长度。")]
        public string CredentialsCode { get; set; }

        [Display(Name = "工作岗位")]
        public string JobTitleID { get; set; }

        [ListItemSpecification("工作岗位", "08", 110, false)]
        [DetailItemSpecification(EditorItemType.PlainFacadeItem, Width = 300)]
        [Display(Name = "工作岗位")]
        public PlainFacadeItem JobTitleItem { get; set; }

        [Required(ErrorMessage = "密码不能为空值。")]
        [Display(Name = "密码")]
        public string InquiryPassword { get; set; }

        public List<Department> DepartmentList { get; set; }

        public PersonVM()
        {
            SexSelector = PlainFacadeItemFactory<Person>.GetBySex();
        }

        public PersonVM(Person bo)
        {
            ID = bo.ID;
            Name              = bo.Name;
            Description       = bo.Description;
            SortCode          = bo.SortCode;
            EmployeeCode      = bo.EmployeeCode;
            LastName          = bo.LastName;
            FixedTelephone    = bo.FixedTelephone;
            CredentialsCode   = bo.CredentialsCode;
            Birthday          = bo.Birthday;
            BirthdayString    = bo.Birthday.ToString("yyyy-MM-dd");
            InquiryPassword   = bo.InquiryPassword;

            if (Birthday.Year == 1)
            {
                Birthday = DateTime.Now;
                BirthdayString = Birthday.ToString("yyyy-MM-dd");
            }
        }

        public void MapToBo(Person bo)
        {
            bo.Name = Name;
            bo.Description = Description;
            bo.SortCode = SortCode;
            bo.EmployeeCode = EmployeeCode;
            //bo.FirstName       = FirstName;
            bo.LastName = LastName;
            bo.FixedTelephone = FixedTelephone;
            bo.CredentialsCode = CredentialsCode;
            bo.Birthday = DateTime.Now;                //DateTime.Parse(BirthdayString);
        }
    }
}

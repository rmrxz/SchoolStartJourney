using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.ViewModels.GroupOrganization
{
    public class AnAssociationVM
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 社团名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 社团简介
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SortCode { get; set; }

        /// <summary>
        /// 学校地址
        /// </summary>
        public string SchoolAddress { get; set; }

        /// <summary>
        /// 创建的时间
        /// </summary>
        public DateTime CreateDataTime { get; set; }


        /// <summary>
        /// 上限人数
        /// </summary>
        public string MaxNumber { get; set; }

        /// <summary>
        /// 社团人数
        /// </summary>
        public int AnAssociationNum { get; set; }


        public Guid UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public BusinessImage Avatar { get; set; }

        /// <summary>
        /// 活动数量
        /// </summary>
        public int acNum { get; set; }

        /// <summary>
        /// 人员数量
        /// </summary>
        public int userNum { get; set; }

        /// <summary>
        /// 社团禁用情况
        /// </summary>
        public bool IsDisable { get; set; }

        /// <summary>
        /// 社团图片
        /// </summary>
        public List<BusinessImage> Images { get; set; }

        /// <summary>
        /// 社团成员
        /// </summary>
        public List<AnAssociationAndUser> Members { get; set; }

        public List<ActivityTerm> Activitys { get; set; }

        public AnAssociationVM()
        { }

        public AnAssociationVM(AnAssociation an)
        {
            ID = an.ID;
            Name = an.Name;
            Description = an.Description;
            SortCode = an.SortCode;
            SchoolAddress = an.SchoolAddress;
            CreateDataTime = an.CreateDataTime;
            MaxNumber = an.MaxNumber==0?"无限":an.MaxNumber.ToString();
            User = an.User;
            Avatar = an.Avatar;
            IsDisable = an.IsDisable;
        }

        public void MapToAn(AnAssociation an)
        {
            an.Name = Name;
            an.Description = Description;
            an.SortCode = SortCode;
            an.SchoolAddress = SchoolAddress;
            an.CreateDataTime = CreateDataTime;
            an.MaxNumber =Convert.ToInt32(MaxNumber);
            an.Avatar = Avatar;
        }
    }
}

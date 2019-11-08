using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.ViewModels.GroupOrganization.ActivityTerms
{
    public class ActivityTermVM
    {

        public Guid ID { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Name { get; set; }

        public bool IsNew { get; set; }

        /// <summary>
        /// 简要说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SortCode { get; set; }

        /// <summary>
        /// 活动创建人
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 活动对应的社团Id
        /// </summary>
        public Guid AnAssociationId { get; set; }

        /// <summary>
        /// 活动对应的社团
        /// </summary>
        public AnAssociation AnAssociation { get; set; }

        /// <summary>
        /// 活动地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 活动最大人数
        /// </summary>
        public string MaxNumber { get; set; }

        /// <summary>
        /// 报名截止时间
        /// </summary>
        public DateTime SignDataTime { get; set; }

        /// <summary>
        /// 活动报名结束时间
        /// </summary>
        public DateTime EndDataTime { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime StartDataTime { get; set; }

        /// <summary>
        /// 活动创建的时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public double Expenses { get; set; }

        /// <summary>
        /// 社团活动名
        /// </summary>
        public string AnAssociationName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ActivityStatus Status { get; set; }

        /// <summary>
        /// 活动显示图片
        /// </summary>
        public BusinessImage Avatar { get; set; }

        /// <summary>
        /// 活动图片
        /// </summary>
        public List<BusinessImage> Images { get; set; }

        /// <summary>
        /// 活动成员
        /// </summary>
        public List<ActivityUser> Members { get; set; }

        /// <summary>
        /// 活动评论
        /// </summary>
        public List<CommentVM> Comments { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentNumber { get; set; }

        /// <summary>
        /// 活动禁用情况
        /// </summary>
        public bool IsDisable { get; set; }

        /// <summary>
        ///参与人 
        /// </summary>
        public ApplicationUser ActivityUser { get; set; }

        public int EnteredNumber { get; set; }

        public ActivityTermVM()
        { }

        public ActivityTermVM(ActivityTerm at)
        {
            ID = at.ID;
            Name = at.Name;
            Description = at.Description;
            SortCode = at.SortCode;
            User = at.User;
            AnAssociation = at.AnAssociation == null ? null : at.AnAssociation;
            Address = at.Address;
            MaxNumber = at.MaxNumber==0?"无限": at.MaxNumber.ToString();
            SignDataTime = at.SignDataTime;
            EndDataTime = at.EndDataTime;
            StartDataTime = at.StartDataTime;
            CreateDateTime = at.CreateDateTime;
            Expenses = at.Expenses;
            User = at.User;
            Status = at.Status;
            Avatar = at.Avatar;
            IsDisable = at.IsDisable;
        }

        public void MapToAT(ActivityTerm at)
        {
            at.Name = Name;
            at.Description = Description;
            at.SortCode = SortCode;
            at.AnAssociation = AnAssociation;
            at.Address = Address;
            at.MaxNumber = MaxNumber=="无限"?0:Convert.ToInt32(MaxNumber);
            at.SignDataTime = SignDataTime;
            at.EndDataTime = EndDataTime;
            at.StartDataTime = StartDataTime;
            at.Expenses = Expenses;
            at.Status = Status;
        }
    }
}

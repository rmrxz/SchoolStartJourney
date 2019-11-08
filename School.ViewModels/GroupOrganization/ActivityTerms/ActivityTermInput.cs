using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.GroupOrganization.ActivityTerms
{
    /// <summary>
    /// 用于编辑活动信息
    /// </summary>
    public class ActivityTermInput
    {
        public string ID { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 社团活动名
        /// </summary>
        public string AnAssociationId { get; set; }

        /// <summary>
        /// 活动地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 报名参与时间
        /// </summary>
        public DateTime SignDataTime { get; set; }


        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime StartDataTime { get; set; }

        /// <summary>
        /// 活动报名结束时间
        /// </summary>
        public DateTime EndDataTime { get; set; }

        /// <summary>
        /// 活动最大人数
        /// </summary>
        public int MaxNumber { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public double Expenses { get; set; }

        /// <summary>
        /// 简要说明
        /// </summary>
        public string Description { get; set; }

        public ActivityTermInput(ActivityTerm at)
        {
            if (at == null)
            {
                at = new ActivityTerm();
            }
            ID =at.ID.ToString();
            Name = at.Name;
            Description = at.Description;
            Address = at.Address;
            MaxNumber = at.MaxNumber;
            SignDataTime = at.SignDataTime;
            EndDataTime = at.EndDataTime;
            StartDataTime = at.StartDataTime;
            Expenses = at.Expenses;
            if (at.User != null)
            {
                UserName = at.User.UserName;
            }
            if (at.AnAssociation != null)
            {
                AnAssociationId = at.AnAssociation.ID.ToString();
            }
        }

        public void MapTo(ActivityTerm at)
        {
            at.ID = Guid.Parse(ID);
            at.Name = Name;
            at.Description = Description;
            at.Address = Address;
            at.MaxNumber = MaxNumber;
            at.SignDataTime = SignDataTime;
            at.EndDataTime = EndDataTime;
            at.StartDataTime = StartDataTime;
            at.Expenses = Expenses;
        }

    }
}

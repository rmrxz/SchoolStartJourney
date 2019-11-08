using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.GroupOrganization
{
    /// <summary>
    /// 活动
    /// </summary>
    public class ActivityTerm : IEntity
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 活动介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SortCode { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 活动负责人
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 活动举办方
        /// </summary>
        public AnAssociation AnAssociation { get; set; }

        /// <summary>
        /// 活动地点
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 上限人数
        /// </summary>
        public int MaxNumber { get; set; }

        /// <summary>
        /// 报名截止时间
        /// </summary>
        public DateTime SignDataTime { get; set; } 

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime StartDataTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime EndDataTime { get; set; }

        /// <summary>
        /// 活动创建的时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public double Expenses { get; set; }

        /// <summary>
        /// 显示图片
        /// </summary>
        public BusinessImage Avatar { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ActivityStatus Status { get; set; }

        /// <summary>
        /// 活动禁用情况
        /// </summary>
        public bool IsDisable { get; set; }

        public ActivityTerm()
        {
            ID = Guid.NewGuid();
            SignDataTime=CreateDateTime = StartDataTime= EndDataTime = DateTime.Now;
            IsDelete = false;
            IsDisable = true;
        }
    }
}

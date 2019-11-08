using School.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace School.Entities.GroupOrganization
{
    /// <summary>
    /// 关联用户和活动
    /// </summary>
    public class ActivityUser : IEntityBase
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid ActivityTermId { get; set; }

        /// <summary>
        /// 活动
        /// </summary>
        [ForeignKey("ActivityTermId")]
        public ActivityTerm ActivityTerm { get; set; }

        /// <summary>
        /// 数据创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 构造函数，初始化Id
        /// </summary>
        public ActivityUser()
        {
            ID = Guid.NewGuid();
            CreateDateTime = DateTime.Now;
        }
    }
}

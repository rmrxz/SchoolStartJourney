using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.GroupOrganization
{
    /// <summary>
    /// 活动和用户中心实体
    /// </summary>
    public class ActivityUsersVM
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 活动
        /// </summary>
        public ActivityTerm ActivityTermData { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public List<BusinessImage> BusinessImages { get; set; }

        public List<ActivityComment> Comments { get; set; }
    }
}

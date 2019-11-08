using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace School.Entities.ApplicationOrganization
{
    /// <summary>
    /// 关注的活动
    /// </summary>
    public class FollowEvent : IEntityBase
    {
        public Guid ID { get; set; }

        public DateTime FollowDateTime { get; set; }//关注时间

        public ActivityTerm ActivityTerm { get;set;}        //关注的活动

        public ApplicationUser User { get; set; }   //关联的用户

        public FollowEvent()
        {
            ID = Guid.NewGuid();
            FollowDateTime = DateTime.Now;

        }


    }
}

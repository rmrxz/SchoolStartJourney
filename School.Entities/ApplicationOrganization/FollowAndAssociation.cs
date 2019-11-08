using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.ApplicationOrganization
{

    /// <summary>
    /// 关注社团
    /// </summary>
    public class FollowAndAssociation:IEntityBase
    {
        public Guid ID { get; set; }
        public DateTime FollowDateTime { get; set; }    //关注的时间
        public AnAssociation AnAssociation { get; set; }//关注的社团
        public ApplicationUser user { get; set; }       //关联的用户


        public FollowAndAssociation()
        {
            ID = Guid.NewGuid();
            FollowDateTime = DateTime.Now;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.ApplicationOrganization
{
    /// <summary>
    /// 用户好友
    /// </summary>
    public class UserFriend : IEntityBase
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 好友
        /// </summary>
        public ApplicationUser Friend {get;set;}
        /// <summary>
        /// 对应的分组
        /// </summary>
        public FriendGroup FriednGroupName { get; set; }

        public UserFriend()
        {
            ID = Guid.NewGuid();
        }
    }
}

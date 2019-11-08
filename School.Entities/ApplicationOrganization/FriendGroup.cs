using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.ApplicationOrganization
{
    /// <summary>
    /// 用户好友分组
    /// </summary>
    public class FriendGroup : IEntity
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分组描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string SortCode { get; set; }
        /// <summary>
        /// 分组排序
        /// </summary>
        public int ItemTypeNumber { get; set; }
        /// <summary>
        /// 对应的用户
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 组内用户集合
        /// </summary>
        public ICollection<ApplicationUser> Members { get; set; }

        public FriendGroup()
        {
            ID = Guid.NewGuid();
            ItemTypeNumber = 0;
        }
    }
}

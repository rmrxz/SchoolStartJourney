using School.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.ApplicationOrganization
{
    /// <summary>
    /// 通知实体
    /// </summary>
    public class MessageNotification : IEntity
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 通知名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 通知说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SortCode { get; set; }

        /// <summary>
        /// 通知的用户
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public ApplicationUser CreatedUser { get; set; }

        /// <summary>
        /// 关联的业务Id
        /// </summary>

        public Guid ObjectId { get; set; }

        /// <summary>
        /// 通知创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public string NotificationType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public BusinessEmergencyEnum Status { get; set; }

        /// <summary>
        /// 标记是否查看过
        /// </summary>
        public bool isSee { get; set; }

        public MessageNotification()
        {
            ID = Guid.NewGuid();
            CreateDateTime = DateTime.Now;
            SortCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<MessageNotification>();
        }

    }
}

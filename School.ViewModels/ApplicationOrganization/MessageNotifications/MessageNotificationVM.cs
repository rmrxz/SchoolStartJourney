using School.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.ApplicationOrganization.MessageNotifications
{
    /// <summary>
    /// 用户通知视图模型
    /// </summary>
    public class MessageNotificationVM
    {
        /// <summary>
        /// 通知ID
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        /// 通知名称
        /// </summary>
        public virtual string Name { get; set; }

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
        /// 通知的用户
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

        public MessageNotificationVM(MessageNotification mn)
        {
            ID = mn.ID;
            Name = mn.Name;
            Description = mn.Description;
            SortCode = mn.SortCode;
            User = mn.User;
            ObjectId = mn.ObjectId;
            CreateDateTime = mn.CreateDateTime;
            NotificationType = mn.NotificationType;
            Status = mn.Status;
            CreatedUser = mn.CreatedUser;
            isSee = mn.isSee;
        }
    }
}

using School.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.GroupOrganization
{
    /// <summary>
    /// 活动的评论
    /// </summary>
    public class ActivityComment:IEntityBase
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 评论的活动
        /// </summary>
        public ActivityTerm Activity { get; set; }

        /// <summary>
        /// 评论人
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 被评论人
        /// </summary>
        public ApplicationUser AcceptUser { get; set; }

        /// <summary>
        /// 评论的时间
        /// </summary>
        public DateTime CommentDataTime { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public virtual Guid? ParentGrade { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public virtual int Hierarchy { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public virtual string Comment { get; set; }



        public ActivityComment()
        {
            ID = Guid.NewGuid();
            CommentDataTime = DateTime.Now;
        }
    }
}

using School.Entities.ApplicationOrganization;
using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.ViewModels.GroupOrganization
{
    /// <summary>
    /// 活动评论的视图模型
    /// </summary>
   public class CommentVM
    {
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 评论的活动
        /// </summary>
        public ActivityTerm Activity { get; set; }

        public Guid ActivityID { get; set; }

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
        /// 子评论
        /// </summary>
        public virtual List<ActivityComment> CommentChildrens { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public virtual Guid? ParentGrade { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public virtual ActivityComment ParentComment { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public virtual int Hierarchy { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public virtual string Comment { get; set; }

        public CommentVM()
        { }

        public CommentVM(ActivityComment cm)
        {
            ID = cm.ID;
            Name = cm.Name;
            Activity = cm.Activity;
            User = cm.User;
            CommentDataTime = cm.CommentDataTime;
            ParentGrade = cm.ParentGrade;
            Hierarchy = cm.Hierarchy;
            Comment = cm.Comment;
            AcceptUser = cm.AcceptUser;
        }

        public void MapToCm(ActivityComment cm)
        {
            cm.Name = Name;
            cm.Activity = Activity;
            cm.User = User;
            cm.CommentDataTime = CommentDataTime;
            cm.ParentGrade = ParentGrade;
            cm.Comment = Comment;
            cm.Hierarchy = Hierarchy;
            cm.AcceptUser = AcceptUser;
        }

    }
}

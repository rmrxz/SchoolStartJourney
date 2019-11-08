using School.Entities.Attachments;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.GroupOrganization
{
    /// <summary>
    /// 轮播图插件实体
    /// </summary>
    public class HomeExhibition : IEntity
    {

         public Guid ID { get; set; }

        public virtual string Name{ get; set; }

        public virtual string SortCode { get; set; }

        //关联业务事项
        public virtual ActivityTerm Activity { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 创建的时间
        /// </summary>
        public virtual DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime StartDateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime EndDateTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsUse { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public virtual BusinessImage Avatar { get; set; }

        /// <summary>
        /// 显示级别
        /// </summary>
        public virtual ExhibitionLevelEnum ExhibitionLeve { get; set; }

        public HomeExhibition()
        {
            IsUse = false;
        }

    }
}

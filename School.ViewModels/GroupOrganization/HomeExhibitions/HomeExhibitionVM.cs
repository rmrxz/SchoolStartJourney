using School.Entities.Attachments;
using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.GroupOrganization.HomeExhibitions
{
    public class HomeExhibitionVM
    {
        public Guid ID { get; set; }

        public virtual string Name { get; set; }

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

        public HomeExhibitionVM(HomeExhibition he)
        {
            ID = he.ID;
            Name = he.Name;
            SortCode = he.SortCode;
            Activity = he.Activity;
            Description = he.Description;
            CreateDateTime = he.CreateDateTime;
            StartDateTime = he.StartDateTime;
            EndDateTime = he.EndDateTime;
            IsUse = he.IsUse;
            Avatar = he.Avatar;
            ExhibitionLeve = he.ExhibitionLeve;
        }

        public void MapToAn(HomeExhibition he)
        {
            he.ID = ID;
            he.Name = Name;
            he.SortCode = SortCode;
            he.Activity = Activity;
            he.Description = Description;
            he.CreateDateTime = CreateDateTime;
            he.StartDateTime = StartDateTime;
            he.EndDateTime = EndDateTime;
            he.IsUse = IsUse;
            he.Avatar = Avatar;
            he.ExhibitionLeve = ExhibitionLeve;
        }
    }
}

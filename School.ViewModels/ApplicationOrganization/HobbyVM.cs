using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.ViewModels.ApplicationOrganization
{
    /// <summary>
    /// 爱好的视图模型
    /// </summary>
    public class HobbyVM
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 爱好说明
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 爱好介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public BusinessImage Avatar { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SortCode { get; set; }

        public HobbyVM()
        {}

        public HobbyVM(Hobby ho)
        {
            ID = ho.ID;
            Name = ho.Name;
            Description = ho.Description;
            SortCode = ho.SortCode;
            Avatar = ho.Avatar;
        }

        public void MapToHb(Hobby ho)
        {
            ho.Name = Name;
            ho.Description = Description;
            ho.SortCode = SortCode;;

        }
    }
}

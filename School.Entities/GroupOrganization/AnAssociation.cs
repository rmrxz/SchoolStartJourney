using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.GroupOrganization
{
    /// <summary>
    /// 社团
    /// </summary>
    public class AnAssociation : IEntity
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 社团名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SortCode { get; set; }

        /// <summary>
        /// 学校
        /// </summary>
        public string SchoolAddress { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDataTime { get; set; }

        /// <summary>
        /// 上限人数
        /// </summary>
        public int MaxNumber { get; set; } 

        /// <summary>
        /// 创建人
        /// </summary>
        public ApplicationUser User{ get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public BusinessImage Avatar { get; set; }

        /// <summary>
        /// 社团禁用情况
        /// </summary>
        public bool IsDisable { get; set; }

        public AnAssociation()
        {
            ID = Guid.NewGuid();
            CreateDataTime = DateTime.Now;
            MaxNumber = 350;
            IsDisable = true;
        }
    }
}

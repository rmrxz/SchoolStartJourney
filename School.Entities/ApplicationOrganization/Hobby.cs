using School.Entities.Attachments;
using School.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.ApplicationOrganization
{
    //爱好
    public class Hobby:IEntity
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 爱好名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 爱好说明
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

        public Hobby()
        {
            ID = Guid.NewGuid();
            SortCode = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
    }
}

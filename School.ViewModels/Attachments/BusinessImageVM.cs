using School.Entities.Attachments;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModels.Attachments
{
   public  class BusinessImageVM
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 图片显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片说明
        /// </summary>
        public string Description { get; set; } 

        /// <summary>
        /// 内部业务编码
        /// </summary>
        public string SortCode { get; set; } 

        /// <summary>
        /// 图片显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 图片原始文件
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 图片上传时间
        /// </summary>
        public DateTime UploadedTime { get; set; } 


        /// <summary>
        /// 图片上传保存路径
        /// </summary>
        public string UploadPath { get; set; } 

        /// <summary>
        /// 上传文件的后缀名
        /// </summary>
        public string UploadFileSuffix { get; set; }
        public long FileSize { get; set; }

        /// <summary>
        /// 文件物理格式图标
        /// </summary>
        public string IconString { get; set; }

        /// <summary>
        /// 使用该图片的业务对象的 id
        /// </summary>
        public Guid RelevanceObjectID { get; set; }

        public BusinessImageVM(BusinessImage bi) {
            ID = bi.ID;
            Name = bi.Name;
            Description = bi.Description;
            SortCode = bi.SortCode;
            DisplayName = bi.DisplayName;
            OriginalFileName = bi.OriginalFileName;
            UploadedTime = bi.UploadedTime;
            UploadPath = bi.UploadPath;
            UploadFileSuffix = bi.UploadFileSuffix;
            IconString = bi.IconString;
            RelevanceObjectID = bi.RelevanceObjectID;
        }
    }
}

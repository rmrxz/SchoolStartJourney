using School.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.Entities.Attachments
{
   public class BusinessVideo
    {
        [Key]
        public Guid ID { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(250)]
        public string SortCode { get; set; }
        public DateTime AttachmentTimeUploaded { get; set; }  //上传的时间        
        [StringLength(500)]
        public string OriginalFileName { get; set; }          //原文件名
        [StringLength(500)]
        public string UploadPath { get; set; }                //上传路径      
        
        public DateTime UploadedTime { get; set; }            //上传时间
        public bool IsForTitle { get; set; }                  //是否能用        
        [StringLength(10)]
        public string UploadFileSuffix { get; set; }          //文件类型        
        public byte[] BinaryContent { get; set; }             //内容        
        public long FileSize { get; set; }                    //文件大小        
        [StringLength(120)]
        public string IconString { get; set; }                // 文件物理格式图标

        public Guid RelevanceObjectID { get; set; }           //管理对象
        public Guid UploaderID { get; set; }                  // 关联上传人ID


        public BusinessVideo()
        {
            this.ID = Guid.NewGuid();
            this.SortCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<BusinessVideo>();
            this.AttachmentTimeUploaded = DateTime.Now;
        }
    }
}

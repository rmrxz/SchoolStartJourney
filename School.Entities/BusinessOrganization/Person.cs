using School.Entities.Attachments;
using School.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.Entities.BusinessOrganization
{
    public class Person:IEntity
    {
        [Key]
        public Guid ID { get; set; }
        /// <summary>
        /// 全名                                                                    
        /// </summary>
        public string Name { get; set; }                                  
        /// <summary>
        /// 人员简要说明
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }                           
        /// <summary>
        /// 系统内部编码
        /// </summary>
        [StringLength(150)]
        public string SortCode { get; set; }                              
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }                         
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ExpiredDateTime { get; set; }                       
        /// <summary>
        /// 持续时间
        /// </summary>
        public int Duration { get; set; }                                   
        /// <summary>
        /// 业务工号
        /// </summary>
        [StringLength(50)]
        public string EmployeeCode { get; set; }                             
        /// <summary>
        /// 名字
        /// </summary>
        [StringLength(50)]
        public string LastName { get; set; }                                 
        /// <summary>
        /// 固定电话
        /// </summary>
        [StringLength(20)]
        public string FixedTelephone { get; set; }                         
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday { get; set; }                               
        /// <summary>
        /// 身份证编号
        /// </summary>
        [StringLength(26)]
        public string CredentialsCode { get; set; }                          
        /// <summary>
        /// 信息更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }                             
        /// <summary>
        /// 查询密码，仅仅用于查询是否已经已经建立数
        /// </summary>
        [StringLength(50)]
        public string InquiryPassword { get; set; }                          
        /// <summary>
        /// 所属部门
        /// </summary>
        public virtual Department Department { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public virtual BusinessImage Avatar { get; set; }

        public Person()
        {
            ID = Guid.NewGuid();
            UpdateTime = CreateDateTime = Birthday = ExpiredDateTime = DateTime.Now;
        }

    }
}

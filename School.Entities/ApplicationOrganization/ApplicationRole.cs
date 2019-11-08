using Microsoft.AspNetCore.Identity;
using School.Entities.BusinessOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.Entities.ApplicationOrganization
{
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        [StringLength(250)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(550)]
        public string Description { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string SortCode { get; set; }

        public virtual Department Department { get; set; }

        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}

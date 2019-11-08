using School.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace School.Entities.BusinessOrganization
{
    public class Department : IEntity
    {
        [Key]
        public Guid ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(150)]
        public string SortCode { get; set; }
        public bool IsActiveDepartment { get; set; }              //是否拥有权限

        [ForeignKey("ParentDepartmentID")]
        public virtual Department ParentDepartment { get; set; }  // 上级部门，一般情况下，类似这样的

        public Department()
        {
            this.ID = Guid.NewGuid();
            this.IsActiveDepartment = true;
            this.SortCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<Department>();
        }
    }
}

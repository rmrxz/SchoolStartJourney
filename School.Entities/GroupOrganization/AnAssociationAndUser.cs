using School.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace School.Entities.GroupOrganization
{
    /// <summary>
    /// 关联社团和用户
    /// </summary>
    public class AnAssociationAndUser : IEntityBase
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 关联用户Id
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 关联社团的Id
        /// </summary>
        public Guid AnAssociationId { get; set; }

        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 关联的社团
        /// </summary>
        [ForeignKey("AnAssociationId")]
        public AnAssociation AnAssociation { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 用户在社团里面的权限
        /// </summary>
        public AnJurisdiction AnJurisdictionManager { get; set; }
        public AnAssociationAndUser()
        {
            ID = Guid.NewGuid();
            CreateDateTime = DateTime.Now;
        }
    }
}

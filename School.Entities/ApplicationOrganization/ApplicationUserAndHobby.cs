using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities.ApplicationOrganization
{
    /// <summary>
    /// 用户对应的兴趣
    /// </summary>
    public class ApplicationUserAndHobby : IEntityBase
    {
        public Guid ID { get; set; }
        public ApplicationUser User { get; set; }//关联的用户
        public Hobby Hobby { get; set; }         //关联的兴趣
        public ApplicationUserAndHobby()
        {
            ID = Guid.NewGuid();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities
{
    public interface IEntity:IEntityBase
    {
        string Name { get; set; }       //名称
        string Description { get; set; }//描述
        string SortCode { get; set; }   //编号
    }
}

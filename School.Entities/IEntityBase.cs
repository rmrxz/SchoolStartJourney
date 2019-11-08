using System;
using System.Collections.Generic;
using System.Text;

namespace School.Entities
{
    public interface IEntityBase
    {
        Guid ID { get; set; }
    }
}

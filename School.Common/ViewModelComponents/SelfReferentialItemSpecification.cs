using System;
using System.Collections.Generic;
using System.Text;

namespace School.Common.ViewModelComponents
{
    public class SelfReferentialItemSpecification : Attribute
    {
        public string RelevanceID { get; set; }
        public SelfReferentialItemSpecification(string id)
        {
            RelevanceID = id;
        }
    }
}

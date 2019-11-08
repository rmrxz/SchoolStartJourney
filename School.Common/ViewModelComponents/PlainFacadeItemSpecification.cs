using System;
using System.Collections.Generic;
using System.Text;

namespace School.Common.ViewModelComponents
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PlainFacadeItemSpecification : Attribute
    {
        public string RelevanceID { get; set; }
        public PlainFacadeItemSpecification(string id)
        {
            RelevanceID = id;
        }

    }
}

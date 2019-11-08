using System;
using System.Collections.Generic;
using System.Text;

namespace School.Common.ViewModelComponents
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DetailItemSpecification : Attribute
    {
        public EditorItemType ItemType { get; set; }
        public int Width { get; set; }

        public DetailItemSpecification(EditorItemType itemType)
        {
            this.ItemType = itemType;
        }

    }
}

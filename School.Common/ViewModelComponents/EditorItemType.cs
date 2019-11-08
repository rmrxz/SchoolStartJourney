using System;
using System.Collections.Generic;
using System.Text;

namespace School.Common.ViewModelComponents
{
    /// <summary>
    /// 定义最基本的数据（对象属性数据）编辑UI类型
    /// </summary>
    public enum EditorItemType
    {
        TextBox,
        TextArea,
        DorpdownOptionWithSelfReferentialItem,
        DorpdownOptionWithPlainFacadeItem,
        DorpdownOptionWithEnum,
        ComboBox,
        CheckBox,
        Date,
        DateTime,
        Time,
        Hidden,
        HtmlTextEditor,
        SelfReferentialItem,
        PlainFacadeItem,
        SinglePerson,
        MultiPerson,
        MultiPersonWithTableList
    }
}

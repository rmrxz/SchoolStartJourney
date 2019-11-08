using System;
using System.Collections.Generic;
using System.Text;

namespace School.Common.ViewModelComponents
{
    public class ValidateMessage
    {
        public bool IsOK { get; set; }
        public List<ValidateMessageItem> ValidateMessageItems { get; set; }

        public ValidateMessage()
        {
            this.IsOK = true;
            this.ValidateMessageItems = new List<ValidateMessageItem>();
        }
    }
}

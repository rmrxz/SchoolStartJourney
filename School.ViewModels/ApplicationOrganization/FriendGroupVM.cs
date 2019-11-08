using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.ViewModels.ApplicationOrganization
{
    /// <summary>
    ///用户好友分组的视图模型
    /// </summary>
    public class FriendGroupVM
    {
        [Key]
        public Guid ID { get; set; }
       
        [Display(Name = "分组名称")]
        public string Name { get; set; }
       
        [Display(Name = "分组描述")]
        public string Description { get; set; }

        [Display(Name = "编号")]
        public string SortCode { get; set; }

        [Display(Name = "分组排序")]
        public int ItemTypeNumber { get; set; }

        [Display(Name = "对应的用户")]
        public string UserID { get; set; }
        public FriendGroupVM()
        {}

        public FriendGroupVM(FriendGroupVM fg)
        {
            ID = fg.ID;
            Name = fg.Name;
            Description = fg.Description;
            SortCode = fg.SortCode;
            ItemTypeNumber = fg.ItemTypeNumber;
            UserID = fg.UserID;
        }

        public void MapToFG(FriendGroupVM fg)
        {
            fg.Name = Name;
            fg.Description = Description;
            fg.SortCode = SortCode;
            fg.ItemTypeNumber = ItemTypeNumber;
            fg.UserID = UserID;
        }
    }
}

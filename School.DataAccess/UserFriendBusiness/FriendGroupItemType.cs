using School.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataAccess.UserFriendBusiness
{
    public class FriendGroupItemType
    {
        /// <summary>
        /// 添加或从新排序分组时获取当前分组排序之后的所有分组
        /// </summary>
        /// <param name="ItemTypeNumbers"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public List<FriendGroup> GroupItemType(List<FriendGroup> ItemTypeNumbers, int serialNumber)
        {
            if (ItemTypeNumbers.Count<=0) throw new Exception("没有任何分组内容");
            int groupAmount = ItemTypeNumbers.Count;
            List<FriendGroup> friendGroups = new List<FriendGroup>();
            foreach (var itemTypeNumber in ItemTypeNumbers)
            {
                if (itemTypeNumber.ItemTypeNumber >= serialNumber)
                {
                    friendGroups.Add(itemTypeNumber);
                }
            }
            return friendGroups;
        }
    }
}

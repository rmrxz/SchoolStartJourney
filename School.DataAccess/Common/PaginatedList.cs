using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace School.DataAccess.Common
{
   public class PaginatedList<T>:List<T>
    {
        public int PageIndex { get; private set; }//当前页面

        public int PageSize { get; private set; }//一页显示的数量

        public int TotalCount { get; private set; }//总数目

        public int TotalPageCount { get; private set; }//页数

        public PaginatedList(int pageIndex, int pageSize, int totalCount, IQueryable<T> source)
        {
            AddRange(source);
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = (int)Math.Ceiling(TotalCount / (double)PageSize);
        }

        public bool HasPreviousPage//是否有前一页
        {
            get { return (PageIndex > 1); }//判断当前页数是否大于1，返回布尔值
        }

        public bool HasNextPage//是否有后一页
        {
            get { return (PageIndex < TotalPageCount); }//判断当前页数是否小于总页数，返回布尔值
        }
    }
}

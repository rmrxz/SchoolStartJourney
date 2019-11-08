using Microsoft.EntityFrameworkCore;
using School.DataAccess.SqlServer;
using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace School.Web.Common
{
    /// <summary>
    /// 定时任务
    /// </summary>
    public class TimerServiceExtensions
    {
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;
        Timer timer = new Timer(1000*60);
        public TimerServiceExtensions(IDataExtension<ActivityTerm> activityTermExtension)
        {
            _activityTermExtension = activityTermExtension;
        }

        public void TimerEditActivitySaatus()
        {
            timer.Elapsed += new ElapsedEventHandler(EditActivityStatus);
            timer.AutoReset = true;
            timer.Enabled = true;
           
        }

        /// <summary>
        /// 更改所有活动的状态
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void EditActivityStatus(object source, ElapsedEventArgs e)
        {
            
            var activityList = _activityTermExtension.GetAll().Where(x=>(x.Status== ActivityStatus.未开始||x.Status==ActivityStatus.进行中)&&x.StartDataTime>DateTime.Now).ToList();
            foreach (var activity in activityList)
            {
                if (activity.StartDataTime > DateTime.Now && activity.EndDataTime > DateTime.Now)
                {
                    activity.Status = ActivityStatus.进行中;
                    _activityTermExtension.Edit(activity);
                }
                else if (activity.EndDataTime < DateTime.Now)
                {
                    activity.Status = ActivityStatus.已结束;
                    _activityTermExtension.Edit(activity);
                }
                _activityTermExtension.Save();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Common
{
    public static class BusinessMatter
    {
        //static EntityDbContext _Context;
        //public static void BusinessInitialize(EntityDbContext context)
        //{
        //    _Context = context;
        //}
        //public static ApplicationUser UserDetail(string id)
        //{
        //    var user = _Context.Set<ApplicationUser>().Where(x => x.Id == id).FirstOrDefault();
        //    if (user == null)
        //    {
        //        return user = new ApplicationUser();
        //    }
        //    return user;
        //}
    }
}

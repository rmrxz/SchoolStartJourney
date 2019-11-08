using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using School.Common.JsonModels;
using School.DataAccess.Common;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.GroupOrganization;
using School.ViewModels.ApplicationOrganization;
using System.Collections.Generic;

namespace School.Web.Controllers.ApplicationOrganization
{
    public class ApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataExtension<ApplicationUser> _userExtension;
        private readonly IDataExtension<UserFriend> _userFriendExtension;
        private readonly IDataExtension<ActivityUser> _activityUserExtension;
        private readonly IDataExtension<AnAssociation> _anAssociationExtension;
        private readonly IDataExtension<AnAssociationAndUser> _userAnAssociationExtension;
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, IDataExtension<ApplicationUser> userExtension,
            IDataExtension<UserFriend> userFriendExtension, IDataExtension<ActivityUser> activityUserExtension, IDataExtension<AnAssociation> anAssociationExtension,
            IDataExtension<AnAssociationAndUser> userAnAssociationExtension, IDataExtension<ActivityTerm> activityTermExtension)
        {
            _userManager = userManager;
            _userExtension = userExtension;
            _userFriendExtension = userFriendExtension;
            _activityUserExtension = activityUserExtension;
            _anAssociationExtension = anAssociationExtension;
            _userAnAssociationExtension = userAnAssociationExtension;
            _activityTermExtension = activityTermExtension;
        }
        // <summary>
        /// 数据管理的入口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var user = User.Claims.FirstOrDefault();
            //if (user == null)
            //{
            //    return RedirectToAction("NotLogin", "Home");
            //}
            //var UserInfo = await _userExtension.GetSingleAsyn(x => x.Id == user.Value);
            //if (UserInfo.Power== AnJurisdiction.Ordinary)
            //{
            //    throw new Exception("您不是管理员");
            //}
            //var boCollection = await _userExtension.GetAllIncludingAsyn(x => x.Avatar);
            //var boVMCollection = new List<ApplicationUserVM>();
            //foreach (var bo in boCollection)
            //{
            //    var boVM = new ApplicationUserVM(bo);
            //    boVMCollection.Add(boVM);
            //}
            //var pageSize = 10;
            //var pageIndex = 1;
            //var personList = boVMCollection.OrderByDescending(x => x.RegisterTime).FirstOrDefault();
            //// 处理分页
            //var userCollectionPageList = IQueryableExtensions.ToPaginatedList(boVMCollection.AsQueryable<ApplicationUserVM>(), pageIndex, pageSize);
            //var userCollections = new List<ApplicationUserVM>();
            //foreach (var userCollection in userCollectionPageList)
            //{
            //    userCollections.Add(userCollection);
            //}

            ////提取当前分页关联的分页器实例
            //var pageGroup = PagenateGroupRepository.GetItem<ApplicationUserVM>(userCollectionPageList, 5, pageIndex);
            //ViewBag.PageGroup = pageGroup;

            //var listPageParameter = new ListPageParameter()
            //{
            //    PageIndex = userCollectionPageList.PageIndex,
            //    Keyword = "",
            //    PageSize = userCollectionPageList.PageSize,
            //    ObjectTypeID = "",
            //    ObjectAmount = userCollectionPageList.TotalCount,
            //    SortDesc = "Default",
            //    SortProperty = "UserName",
            //    PageAmount = 0,
            //    SelectedObjectID = ""
            //};
            //ViewBag.PageParameter = listPageParameter;userCollections
            return View("~/Views/ApplicationOrganization/ApplicationUser/Index.cshtml"); 
        }

        /// <summary>
        /// 根据关键词检索人员数据集合，返回给前端页面
        /// </summary>
        /// <param name="keywork"></param>
        /// <returns></returns>
        public async Task<IActionResult> List(string keywork, string listPageParaJson,string pageIndex)
        {
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null && listPageParaJson != "")
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }
            listPagePara.PageIndex = Convert.ToInt32(pageIndex);
            var boVMCollection = new List<ApplicationUserVM>();
            if (!String.IsNullOrEmpty(keywork)&& keywork!= "undefined")
            {
                Expression<Func<ApplicationUser, bool>> condtion = x =>//Contains(参数字符串是否包含于string对象中)
                   x.Name.Contains(keywork) ||
                   x.SchoolAddress.Contains(keywork) ||
                   x.School.Contains(keywork);
                var userCollection = await _userExtension.GetAll().Include(x => x.Avatar).Where(condtion).OrderByDescending(x=>x.RegisterTime).ToListAsync();
                foreach (var bo in userCollection)
                {
                    boVMCollection.Add(new ApplicationUserVM(bo));
                }
            }
            else
            {
                var userCollection = await _userExtension.GetAll().Include(x=>x.Avatar).OrderByDescending(x => x.RegisterTime).ToListAsync();
                foreach (var bo in userCollection)
                {
                    boVMCollection.Add(new ApplicationUserVM(bo));
                }
            }

            var userCollectionPageList = IQueryableExtensions.ToPaginatedList(boVMCollection.AsQueryable<ApplicationUserVM>(), listPagePara.PageIndex, listPagePara.PageSize);

            var userCollections = new List<ApplicationUserVM>();
            foreach (var userCollection in userCollectionPageList)
            {
                userCollections.Add(userCollection);
            }
            var pageGroup = PagenateGroupRepository.GetItem<ApplicationUserVM>(userCollectionPageList, 3, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/ApplicationOrganization/ApplicationUser/_List.cshtml", userCollections);

        }

        /// <summary>
        /// 保存人员数据
        /// </summary>
        /// <param name="perVM"></param>
        /// <returns></returns>
        public async Task<IActionResult> Save([Bind("ID,IsNew,UserName,Name,Description,MobileNumber,QQ,UserAddress,School")]ApplicationUserVM userVM)
        {
            var hasDuplicateNamePerson = await _userExtension.HasInstanceAsyn(x => x.Name == userVM.Name);
            if (hasDuplicateNamePerson && userVM.IsNew)//判断是否已存在该人员
            {
                ModelState.AddModelError("", "人员名重复，无法添加");//添加指定的错误信息
                return View("~/Views/BusinessOrganization/Person/CreateOrEdit.cshtml", userVM);
            }
            var user = new ApplicationUser();
            if (!userVM.IsNew)
            {
                user = await _userExtension.GetSingleAsyn(x => x.Id == userVM.ID.ToString());
            }
            userVM.MapToOm(user);
            if (userVM.IsNew)
            {
                _userExtension.AddAndSave(user);
            }
            else
            {
                _userExtension.EditAndSave(user);
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// 增或者编辑人员数据的处理
        /// </summary>
        /// <param name="id">人员对象的ID属性值，如果这个值在系统中找不到具体的对象，则看成是新建对象。</param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var isNew = false;
            var user = await _userExtension.GetSingleAsyn(x=>x.Id== id.ToString());
            if (user == null)
            {
                user = new ApplicationUser();
                user.Name = "";
                user.Description = "";
                user.SortCode = "";
                isNew = true;
            }
            var UserVM = new ApplicationUserVM(user);
            UserVM.IsNew = isNew;
            return View("~/Views/ApplicationOrganization/ApplicationUser/CreateOrEdit.cshtml", UserVM);

        }

        /// <summary>
        /// 以局部页的方式的方式，构建明细数据的处理
        /// </summary>
        /// <param name="id">人员对象的ID属性值</param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(Guid id)
        {
            var user = await _userExtension.GetAll().Include(x=>x.Person).Where(x=>x.Id== id.ToString()).FirstOrDefaultAsync();
            var userVM = new ApplicationUserVM(user);

            //获取用户好友数量
            var userFriens = await _userFriendExtension.GetAll().Where(x => x.UserID == id.ToString()).ToListAsync();
            ViewBag.Friends = userFriens.Count;

            //获取社团数量
            var anAssociations = await _anAssociationExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == id.ToString()).ToListAsync();
            var userAnAssociations = await _userAnAssociationExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == id.ToString()).ToListAsync();
            ViewBag.AnAssociations = anAssociations.Count + userAnAssociations.Count;

            //获取活动数量
            var activitys = await _activityTermExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == id.ToString() && x.Status == ActivityStatus.已结束).ToListAsync();
            var userActivitys = await _activityUserExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == id.ToString() && x.ActivityTerm.Status == ActivityStatus.已结束).ToListAsync();
            ViewBag.Activitys = activitys.Count + userActivitys.Count;
            return PartialView("~/Views/ApplicationOrganization/ApplicationUser/_Detail.cshtml", userVM);

        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userExtension.GetSingleAsyn(x => x.Id == id.ToString());
            _userExtension.DeleteAndSave(user);
            return Json(new { isOK=true,message=user.Name+ "删除成功"});
        }

        public async Task<IActionResult> PasswordReset(Guid id)
        {
            var userInfo = await _userManager.FindByIdAsync(id.ToString());
            userInfo.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(userInfo, "123@abc");
            return Json(new {isOK=true,message=userInfo.Name+ "用户密码重置成功" });
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> EnabledUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            //if (user.TwoFactorEnabled)
            //{
            //    return Json(new { isOK = false, message = "该用户已经启用过" });
            //}
            user.TwoFactorEnabled = true;
            _userExtension.EditAndSave(user);
            return Json(new { isOK = true, message = "成功启用用户："+user.Name });
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DisableUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            //if (user.TwoFactorEnabled)
            //{
            //    return Json(new { isOK = false, message = "该用户已经启用过" });
            //}
            user.TwoFactorEnabled = false;
            _userExtension.EditAndSave(user);
            return Json(new { isOK = true, message = "成功禁用用户："+user.Name });
        }
    }
}
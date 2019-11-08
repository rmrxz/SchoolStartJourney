using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.DataAccess;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.GroupOrganization;
using School.ViewModels.ApplicationOrganization;
using School.ViewModels.ApplicationOrganization.ApplicationUsers;
using School.ViewModels.GroupOrganization;
using School.ViewModels.GroupOrganization.ActivityTerms;
using School.Web.Common;
using Microsoft.EntityFrameworkCore;
using School.DataAccess.Common;
using System.Linq.Expressions;
using School.Common.JsonModels;
using System.Linq;
using School.ViewModels.Attachments;
using School.ViewModels.GroupOrganization.AnAssociations;
using Microsoft.Extensions.Options;
using School.ViewModels.ApplicationOrganization.MessageNotifications;

namespace School.Web.Controllers.BusinessView
{
    /// <summary>
    /// 管理中心控制器
    /// </summary>
    public class ManageCenterController : Controller
    {
        private readonly IDataExtension<AnAssociation> _anAssociationExtension;
        private readonly IEntityRepository<AnAssociation> _anAssociationRepository;
        private readonly IDataExtension<AnAssociationAndUser> _anAnAssociationAndUserExtension;
        private readonly IDataExtension<ActivityUser> _activityUserExtension;
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;
        private readonly IEntityRepository<ActivityTerm> _activityTermRepository;
        private readonly IDataExtension<Hobby> _hobbyExtension;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataExtension<ApplicationUser> _userExtension;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IDataExtension<BusinessImage> _businessImageExtension;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataExtension<UserFriend> _userFriendExtension;
        private readonly IDataExtension<ApplicationUserAndHobby> _userHoppyExtension;
        private readonly IDataExtension<MessageNotification> _notificationExtension;
        /// <summary>
        /// 
        /// </summary>
        public ManageCenterController(IDataExtension<ActivityUser> activityUserExtension,
            IDataExtension<ActivityTerm> activityTermExtension,
            IEntityRepository<ActivityTerm> activityTermRepository,
             IDataExtension<Hobby> hobbyExtension,
             UserManager<ApplicationUser> userManager,
            IDataExtension<AnAssociation> anAssociationExtension,
            IEntityRepository<AnAssociation> anAssociationRepository,
        IDataExtension<AnAssociationAndUser> anAnAssociationAndUserExtension,
            IDataExtension<ApplicationUser> userExtension,
            IHostingEnvironment hostingEnv,
            IDataExtension<BusinessImage> businessImageExtension,
            SignInManager<ApplicationUser> signInManager,
            IDataExtension<UserFriend> userFriendExtension,
             IDataExtension<ApplicationUserAndHobby> userHoppyExtension,
             IDataExtension<Entities.ApplicationOrganization.MessageNotification> notificationExtension)
        {
            _activityUserExtension = activityUserExtension;
            _activityTermExtension = activityTermExtension;
            _activityTermRepository = activityTermRepository;
            _hobbyExtension = hobbyExtension;
            _userManager = userManager;
            _anAssociationExtension = anAssociationExtension;
            _anAssociationRepository = anAssociationRepository;
            _anAnAssociationAndUserExtension = anAnAssociationAndUserExtension;
            _userExtension = userExtension;
            _hostingEnv = hostingEnv;
            _businessImageExtension = businessImageExtension;
            _signInManager = signInManager;
            _userFriendExtension = userFriendExtension;
            _userHoppyExtension = userHoppyExtension;
            _notificationExtension = notificationExtension;
        }

        /// <summary>
        /// 管理中心初始界面(个人信息)
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var user = User.Claims.FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("NotLogin", "Home");
            }
            return View("~/Views/BusinessView/ManageCenter/Index.cshtml");
        }

        public async Task<IActionResult> UserInfo()
        {
            var user = User.Claims.FirstOrDefault();
            var entity = await _userManager.FindByIdAsync(user.Value);
            var userVm = new ApplicationUserInput(entity);
            return View("~/Views/BusinessView/ManageCenter/UserInfo.cshtml", userVm);
        }

        /// <summary>
        /// 管理中心初始界面(个人信息保存)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SaveInfo(string userInfoData)
        {

            var userInput = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationUserInput>(userInfoData);
            var user = await _userManager.FindByIdAsync(userInput.ID.ToString());
            userInput.MapTo(user);
            if (user.Name == "")
            {
                return Json(new { result = false, message = "用户名不能为空" });
            }
            _userExtension.EditAndSave(user);
            return Json(new { result = true, message = "保存成功" });
        }

        /// <summary>
        /// 获取好友
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UserFriends()
        {
            var user = User.Claims.FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("NotLogin", "Home");
            }
            var userFriends = await _userFriendExtension.GetAll().Include(x => x.Friend.Avatar).Include(x => x.Friend).Where(x => x.UserID == user.Value).ToListAsync();
            var users = new List<ApplicationUserVM>();
            foreach (var item in userFriends)
            {
                users.Add(new ApplicationUserVM(item.Friend));
            }
            return View("~/Views/BusinessView/ManageCenter/UserFriends.cshtml", users);

        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DeleteFriend(Guid id)
        {
            var user = User.Claims.FirstOrDefault();
            var userFriends = await _userFriendExtension.GetAll().Include(x => x.Friend).Where(x => x.UserID == user.Value && x.Friend.Id == id.ToString()).FirstOrDefaultAsync();
            if (userFriends == null)
            {
                return Json(new { isOK = false, message = "删除失败，改好友不存在" });
            }
            _userFriendExtension.DeleteAndSave(userFriends);
            return Json(new { isOK = true });
        }

        /// <summary>
        /// 获取爱好
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UserHobbys()
        {
            //获取所有爱好
            var hobbys = await _hobbyExtension.GetAll().OrderBy(x=>x.Name).Include(x => x.Avatar).ToListAsync();
            //获取用户的爱好
            var userHobbys = await _userHoppyExtension.GetAll().Include(x => x.User).OrderBy(x => x.Hobby.Name).Include(x => x.Hobby).Include(x => x.Hobby.Avatar)
               .Where(x => x.User.Id == User.Claims.FirstOrDefault().Value).Select(x => x.Hobby).ToListAsync();
            //获取用户爱好ID
            var userHobbysId = userHobbys.Select(x => x.ID);
            var hobbyVM = new List<HobbyVM>();
            //赛选
            var hobbyList = await _hobbyExtension.GetAll().Include(x => x.Avatar).Where(x=> !userHobbysId.Contains(x.ID)).ToListAsync();
            foreach (var hobby in hobbyList)
            {
                hobbyVM.Add(new HobbyVM(hobby));
            }
            //foreach (var hobby in hobbys)
            //{
            //    foreach (var item in userHobbys)
            //    {
            //        if (hobby.ID != item.Hobby.ID)
            //        {
            //            hobbyVM.Add(new HobbyVM(hobby));
            //            break;
            //        }
            //    }

            //}
            ViewBag.UserHobbyList = userHobbys;
            return View("~/Views/BusinessView/ManageCenter/Hobby.cshtml", hobbyVM);

        }

        /// <summary>
        /// 更换爱好
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateUserHobbys(string hobbys)
        {
            string[] strArray = hobbys.Split(',');
            var user = await _userManager.FindByIdAsync(User.Claims.FirstOrDefault().Value);
            var userHobbyList = await _userHoppyExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == user.Id).ToListAsync();
            foreach (var item in userHobbyList)
            {
                _userHoppyExtension.Delete(item);
            }
            var userHobby = new ApplicationUserAndHobby();
            foreach (var item in strArray)
            {
                userHobby = new ApplicationUserAndHobby()
                {
                    User = user,
                    Hobby = await _hobbyExtension.GetAll().Where(x => x.ID == Guid.Parse(item)).FirstOrDefaultAsync()
                };
                _userHoppyExtension.Add(userHobby);
            }
            _userHoppyExtension.Save();
            return Json(true);

        }


        #region 管理中心活动业务

        public IActionResult Activity()
        {
            return View("~/Views/BusinessView/ManageCenter/Activity.cshtml");
        }

        /// <summary>
        /// 活动列表
        /// </summary>
        /// <param name="keywork"></param>
        /// <param name="listPageParaJson"></param>
        /// <returns></returns>
        public async Task<IActionResult> ActivityList(string keywork, string listPageParaJson)
        {
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null)
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }
            listPagePara.PageSize = 7;
            var atCollection = new List<ActivityTerm>();
            var user = User.Claims.FirstOrDefault();
            if (!String.IsNullOrEmpty(keywork))
            {
                Expression<Func<ActivityTerm, bool>> condtion = x =>
                   x.Name.Contains(keywork) ||
                    x.User.Name.Contains(keywork) ||
                   x.Address.Contains(keywork);

                Expression<Func<ActivityUser, bool>> condtionRelation = x =>
                 x.ActivityTerm.Name.Contains(keywork) ||
                  x.ActivityTerm.User.Name.Contains(keywork) ||
                 x.ActivityTerm.Address.Contains(keywork);

                //获取自己创建的
                var activityTermCollection = await _activityTermExtension.GetAll().Include(x => x.User).Where(condtion).Where(x => x.User.Id == user.Value).ToListAsync();

                //获取自己参与的
                var activityUserList = await _activityUserExtension.GetAll().Include(x => x.ActivityTerm)
                    .Include(x => x.User).Include(x => x.ActivityTerm.User).Where(condtionRelation)
                    .Where(x => x.User.Id == user.Value).ToListAsync();
                var activitieList = activityUserList.AsQueryable().Select(x => x.ActivityTerm);
                //合并
                atCollection = activityTermCollection.Union(activitieList).OrderByDescending(x => x.StartDataTime).ToList();
            }
            else
            {
                //获取自己创建的
                var activityTermCollection = await _activityTermExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == user.Value).ToListAsync();
                //获取自己参与的
                var activityUserList = await _activityUserExtension.GetAll().Include(x => x.User)
                    .Include(x => x.ActivityTerm).Include(x => x.ActivityTerm.User)
                    .Where(x => x.User.Id == user.Value).ToListAsync();
                var activitieList = activityUserList.AsQueryable().Select(x => x.ActivityTerm);
                atCollection = activityTermCollection.Union(activitieList).OrderByDescending(x => x.StartDataTime).ToList();

            }
            var activityTermPageList = IQueryableExtensions.ToPaginatedList(atCollection.AsQueryable(), listPagePara.PageIndex, listPagePara.PageSize);

            var activityCollections = new List<ActivityTermVM>();

            foreach (var activityTermPage in activityTermPageList)
            {
                activityCollections.Add(new ActivityTermVM(activityTermPage));
            }

            var pageGroup = PagenateGroupRepository.GetItem(activityTermPageList, 3, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/BusinessView/ManageCenter/ActivityList.cshtml", activityCollections);
        }

        /// <summary>
        /// 获取活动信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ActivityDetailed(Guid id)
        {
            var entity = await _activityTermExtension.GetAll().Include(x => x.User).Include(x => x.AnAssociation).Where(x => x.ID == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return View("~/Views/BusinessView/ManageCenter/ActivityDetailed.cshtml", new ActivityTermVM());
            }
            var activity = new ActivityTermVM(entity);
            return View("~/Views/BusinessView/ManageCenter/ActivityDetailed.cshtml", activity);
        }

        /// <summary>
        /// 获取活动图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ActivityDetailedImages(Guid id)
        {
            var activity = await _activityTermExtension.GetAll().Include(x => x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
            var activityVM = new ActivityTermVM(activity);
            activityVM.Images = await _businessImageExtension.GetAll().Where(x => x.RelevanceObjectID == id).ToListAsync();
            return View("~/Views/BusinessView/ManageCenter/ActivityDetailedImages.cshtml", activityVM);
        }

        /// <summary>
        /// 获取活动成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ActivityDetailedMenber(Guid id)
        {
            var activity = await _activityTermExtension.GetAll().Include(x => x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
            var activityVM = new ActivityTermVM(activity);
            activityVM.Members = new List<ActivityUser>();
            activityVM.Members = await _activityUserExtension.GetAll().OrderBy(x => x.CreateDateTime).Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.ActivityTermId == id).ToListAsync();
            return View("~/Views/BusinessView/ManageCenter/ActivityDetailedMembers.cshtml", activityVM);
        }

        /// <summary>
        /// 添加活动图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveActivityDetailedImages(Guid id)
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return Json(new { isOK = false, message = "请选择上传的图片" });
            }
            var user = User.Claims.FirstOrDefault();
            UploadPhone updatePhone = new UploadPhone(_hostingEnv);
            BusinessImage image;
            foreach (var file in files)
            {
                image = new BusinessImage();
                var activity = await _activityTermExtension.GetAll().Include(x => x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
                if (activity.User.Id != user.Value)
                {
                    return Json(new { isOK = false, message = "您没有上传图片的权限" });
                }
                //保存文件
                image.UploadPath = updatePhone.PhoneNewUpload(file, "ActivityTerm");

                image.UploaderID = Guid.Parse(user.Value);
                image.RelevanceObjectID = activity.ID;
                _businessImageExtension.AddAndSave(image);
                activity.Avatar = image;
                _activityTermExtension.EditAndSave(activity);
            }
            return Json(new { isOK = true });
        }

        /// <summary>
        /// 删除活动图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteActivityDetailedImages(Guid id, Guid activityId)
        {
            var entity = await _businessImageExtension.GetAll().Where(x => x.RelevanceObjectID == activityId && x.ID == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return Json(new { isOK = false, message = "请选择删除的图片" });
            }
            var activity = await _activityTermExtension.GetAll().Include(x => x.User).Include(x => x.Avatar).Where(x => x.User.Id == User.Claims.FirstOrDefault().Value && x.ID == activityId).FirstOrDefaultAsync();
            if (activity == null)
            {
                return Json(new { isOK = false, message = "您没有删除图片的权限" });
            }
            if (activity.Avatar.ID == id)
            {
                activity.Avatar = activity.Avatar = await _businessImageExtension.GetAll().Where(x => x.Name == "默认图片" && x.IsSystem).FirstOrDefaultAsync();
                _activityTermExtension.EditAndSave(activity);
            }
            _businessImageExtension.DeleteAndSave(entity);
            return Json(new { isOK = true });
        }

        /// <summary>
        /// 删除活动成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteActivityMember(Guid id)
        {
            var entity = await _activityUserExtension.GetAll().Include(x => x.User).Include(x => x.ActivityTerm).Where(x => x.ID == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return Json(new { isOK = true, message = "删除失败，此人早已退出该活动" });
            }
            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(entity.User);
            await AddMessageNotification(users, id, entity.ActivityTerm.Name, "活动成员移除通知", BusinessEmergencyEnum.一般);
            _activityUserExtension.DeleteAndSave(entity);
            return Json(new { isOK = true });
        }

        /// <summary>
        /// 取消活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CancelActivity(Guid id)
        {
            var entity = await _activityTermExtension.GetAll().Include(x => x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
            var user = User.Claims.FirstOrDefault();
            if (entity.User.Id != user.Value)
            {
                return Json(new { isOK = false, message = "你不是创建人，不可取消" });
            }
            else if (entity.Status != ActivityStatus.未开始)
            {
                return Json(new { isOK = false, message = "活动已开始、已结束或者已取消" });
            }
            entity.Status = ActivityStatus.已取消;
            _activityTermExtension.EditAndSave(entity);
            return Json(new { isOK = true, message = "活动取消成功" });
        }

        /// <summary>
        /// 退出活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OutActivity(Guid id)
        {
            var user = User.Claims.FirstOrDefault();
            var entity = await _activityUserExtension.GetAll().Include(x => x.ActivityTerm).Include(x => x.User).Where(x => x.User.Id == user.Value && x.ActivityTerm.ID == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return Json(new { isOK = false, message = "操作失败，你已经退出了活动" });
            }
            else if (entity.ActivityTerm.Status != ActivityStatus.未开始)
            {
                return Json(new { isOK = false, message = "活动已开始、已结束或者已取消,不可退出" });
            }
            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(entity.User);
            await AddMessageNotification(users, id, entity.ActivityTerm.Name, "活动成员退出通知", BusinessEmergencyEnum.一般);
            _activityUserExtension.DeleteAndSave(entity);
            return Json(new { isOK = true, message = "退出活动成功" });
        }

        /// <summary>
        /// 添加或编辑活动
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddActivity(Guid? Id)
        {
            var user = User.Claims.FirstOrDefault();
            var entity = new ActivityTerm();
            if (Id != null)
            {
                entity = await _activityTermExtension.GetAll().Include(x => x.User).Where(x => x.ID == Guid.Parse(Id.ToString())).FirstAsync();
            }
            ViewBag.AnAssociation = await _anAssociationExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == user.Value).ToListAsync();
            var activityInput = new ActivityTermInput(entity);
            return View("~/Views/BusinessView/ManageCenter/AddActivity.cshtml", activityInput);
        }


        /// <summary>
        /// 保存活动
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveActivity(string activityData)
        {
            var activity = new ActivityTerm();
            var activityInput = Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityTermInput>(activityData);
            activityInput.MapTo(activity);
            if (activity.Name == "")
            {
                return Json(new { result = false, message = "活动名称不能为空" });
            }
            if (activityInput.AnAssociationId != "")
            {
                activity.AnAssociation = await _anAssociationExtension.GetAll().Include(x => x.User).Where(x => x.ID == Guid.Parse(activityInput.AnAssociationId)).FirstOrDefaultAsync();
                if (activity.AnAssociation == null)
                {
                    return Json(new { result = false, message = "没有该社团，请从新输入" });
                }
            }

            //判断是否有该活动
            var isExists = this._activityTermExtension.GetAll().Any(x => x.ID == activity.ID);
            if (!isExists)
            {
                activity.IsDisable = true;
                activity.User = await _userManager.FindByIdAsync(User.Claims.FirstOrDefault().Value);
                var userAn = new ActivityUser()
                {
                    User = await _userManager.FindByIdAsync(User.Claims.FirstOrDefault().Value),
                    ActivityTerm = activity
                };
                _activityUserExtension.Add(userAn);
                activity.Avatar = await _businessImageExtension.GetAll().Where(x => x.Name == "默认图片" && x.IsSystem).FirstOrDefaultAsync();

            }
            else
            {
                var activityUser = await _activityUserExtension.GetAll().Include(x => x.User).Where(x => x.ActivityTermId == activity.ID).Select(x => x.User).ToListAsync();
                await AddMessageNotification(activityUser, activity.ID, activity.Name, "社团活动信息有更动，请及时查看", BusinessEmergencyEnum.一般);
            }
            //获取社团成员,给所有社团成员添加该活动
            if (activity.AnAssociation != null)
            {
                if (activity.AnAssociation.User.Id == User.Claims.FirstOrDefault().Value)
                {
                    var userAnAssociations = await _anAnAssociationAndUserExtension.GetAll().Include(x => x.User).Include(x => x.AnAssociation).Where(x => x.AnAssociation.ID == activity.AnAssociation.ID).Select(x => x.User).ToListAsync();

                    foreach (var user in userAnAssociations)
                    {
                        var userAn = new ActivityUser()
                        {
                            User = user,
                            ActivityTerm = activity
                        };
                    }
                    await AddMessageNotification(userAnAssociations, activity.ID, activity.Name, "社团活动通知，请及时查看", BusinessEmergencyEnum.一般);
                    _activityUserExtension.Save();
                }

            }

            await _activityTermRepository.AddOrEditAndSaveAsyn(activity);
            return Json(new { result = true, id = activity.ID, message = "添加活动成功" });
        }

        #endregion

        #region 管理中心社团管理
        public IActionResult AnAssociation()
        {
            return View("~/Views/BusinessView/ManageCenter/AnAssociation.cshtml");
        }

        /// <summary>
        /// 社团列表
        /// </summary>
        /// <param name="keywork"></param>
        /// <param name="listPageParaJson"></param>
        /// <returns></returns>
        public async Task<IActionResult> AnAssociationList(string keywork, string listPageParaJson)
        {
            //处理脚本传送的分页数据
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null)
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }
            listPagePara.PageSize = 7;
            var anCollection = new List<AnAssociation>();
            var user = User.Claims.FirstOrDefault();

            //判断是否有筛选条件
            if (!String.IsNullOrEmpty(keywork))
            {
                //linq表达式处理筛选条件，

                Expression<Func<AnAssociationAndUser, bool>> condtionRelation = x =>
                 x.AnAssociation.Name.Contains(keywork) ||
                  x.AnAssociation.User.Name.Contains(keywork) ||
                 x.AnAssociation.SchoolAddress.Contains(keywork);

                //var 

                //获取自己产参加的社团
                var anUserCollection = await _anAnAssociationAndUserExtension.GetAll().Include(x => x.User).Include(x => x.AnAssociation).Include(x => x.AnAssociation.User)
                    .Where(condtionRelation).Where(x => x.User.Id == user.Value).ToListAsync();
                anCollection = anUserCollection.AsQueryable().Select(x => x.AnAssociation).ToList();
            }
            else
            {
                //获取自己产参加的社团
                var anUserCollection = await _anAnAssociationAndUserExtension.GetAll().Include(x => x.User).Include(x => x.AnAssociation).Include(x => x.AnAssociation.User)
                   .Where(x => x.User.Id == user.Value).ToListAsync();
                anCollection = anUserCollection.AsQueryable().Select(x => x.AnAssociation).ToList();
            }
            //处理分页数据
            var AnAssociationPageList = IQueryableExtensions.ToPaginatedList(anCollection.AsQueryable(), listPagePara.PageIndex, listPagePara.PageSize);

            var AnAssociationCollections = new List<AnAssociationVM>();

            foreach (var AnAssociationPage in AnAssociationPageList)
            {
                AnAssociationCollections.Add(new AnAssociationVM(AnAssociationPage));
            }

            var pageGroup = PagenateGroupRepository.GetItem(AnAssociationPageList, 3, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/BusinessView/ManageCenter/AnAssociationList.cshtml", AnAssociationCollections);
        }

        /// <summary>
        /// 获取社团信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AnAssociationDetailed(Guid id)
        {
            /*根据id获取社团信息和对应的业务*/

            var entity = await _anAssociationExtension.GetAll().Include(x => x.User).Include(x => x.Avatar).Where(x => x.ID == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return View("~/Views/BusinessView/ManageCenter/ActivityDetailed.cshtml", new AnAssociationVM());
            }
            var anAssociation = new AnAssociationVM(entity);
            //获取社团人数
            anAssociation.AnAssociationNum = (await _anAnAssociationAndUserExtension.GetAll().Where(x => x.AnAssociationId == anAssociation.ID).ToListAsync()).Count();
            //获取社团活动
            anAssociation.acNum = (await _activityTermExtension.GetAll().Include(x => x.AnAssociation).Where(x => x.AnAssociation.ID == anAssociation.ID).ToListAsync()).Count();
            return View("~/Views/BusinessView/ManageCenter/AnAssociationDetailed.cshtml", anAssociation);
        }

        /// <summary>
        /// 解散社团
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AnAssociationDissolution(Guid id)
        {
            /*删除对应的社团和社团成全，*/

            var anAssociation = await _anAssociationExtension.GetAll().Where(x => x.ID == id).FirstOrDefaultAsync();
            if (anAssociation == null)
            {
                return Json(new { isOK = false, message = "没有这个社团" });
            }
            //批量删除
            var userList = await _anAnAssociationAndUserExtension.GetAll().Where(x => x.AnAssociationId == id).Select(x => x.User).ToListAsync();
            _anAnAssociationAndUserExtension.DeleteAndSaveBy(x => x.AnAssociationId == id);
            _anAssociationExtension.DeleteAndSave(anAssociation);
            await AddMessageNotification(userList, id, anAssociation.Name, "社团解散通知", BusinessEmergencyEnum.一般);
            return Json(new { isOK = true, message = "成功解散社团：" + anAssociation.Name + "" });

        }


        /// <summary>
        /// 退出社团
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OutAnAssociation(Guid id)
        {
            var anUser = await _anAnAssociationAndUserExtension.GetAll().Include(x => x.AnAssociation).Include(x => x.User).Where(x => x.AnAssociationId == id && x.User.Id.ToString() == User.Claims.FirstOrDefault().Value).FirstOrDefaultAsync();
            if (anUser == null)
            {
                return Json(new { isOK = false, message = "你已经不是这个社团的成员" });
            }
            _anAnAssociationAndUserExtension.DeleteAndSave(anUser);
            List<ApplicationUser> user = new List<ApplicationUser>();
            user.Add(anUser.User);
            await AddMessageNotification(user, anUser.AnAssociationId, anUser.AnAssociation.Name, "社团成员退出，请及时查看", BusinessEmergencyEnum.一般);
            return Json(new { isOK = true, message = "成功退出社团：" + anUser.AnAssociation.Name + "" });
        }


        /// <summary>
        /// 获取社团图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AnAssociationDetailedImages(Guid id)
        {
            var anAssociation = await _anAssociationExtension.GetAll().Include(x => x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
            var anAssociationVM = new AnAssociationVM(anAssociation);
            anAssociationVM.Images = await _businessImageExtension.GetAll().Where(x => x.RelevanceObjectID == id).ToListAsync();
            return View("~/Views/BusinessView/ManageCenter/AnAssociationDetailedImages.cshtml", anAssociationVM);
        }


        /// <summary>
        /// 获取社团成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AnAssociationDetailedMenber(Guid id)
        {
            var anAssociation = await _anAssociationExtension.GetAll().Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.ID == id).FirstOrDefaultAsync();
            var anAssociationVM = new AnAssociationVM(anAssociation);
            anAssociationVM.Members = new List<AnAssociationAndUser>();
            anAssociationVM.Members = await _anAnAssociationAndUserExtension.GetAll().OrderByDescending(x => x.AnJurisdictionManager).ThenByDescending(x => x.CreateDateTime).Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.AnAssociationId == id).ToListAsync();
            return View("~/Views/BusinessView/ManageCenter/AnAssociationDetailedMenber.cshtml", anAssociationVM);
        }

        /// <summary>
        /// 添加社团图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveAnAssociationDetailedImages(Guid id)
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return Json(new { isOK = false, message = "请选择上传的图片" });
            }
            var user = User.Claims.FirstOrDefault();
            UploadPhone updatePhone = new UploadPhone(_hostingEnv);
            BusinessImage image;
            foreach (var file in files)
            {
                image = new BusinessImage();
                var anAssociation = await _anAssociationExtension.GetAll().Include(x => x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
                if (anAssociation.User.Id != user.Value)
                {
                    return Json(new { isOK = false, message = "您没有上传图片的权限" });
                }
                //保存文件
                image.UploadPath = updatePhone.PhoneNewUpload(file, "AnAssociation");

                image.UploaderID = Guid.Parse(user.Value);
                image.RelevanceObjectID = anAssociation.ID;
                _businessImageExtension.AddAndSave(image);
                anAssociation.Avatar = image;
                _anAssociationExtension.EditAndSave(anAssociation);
            }
            return Json(new { isOK = true });
        }

        /// <summary>
        /// 删除社团图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteAnAssociationDetailedImages(Guid id, Guid anAssociationId)
        {
            var entity = await _businessImageExtension.GetAll().Where(x => x.RelevanceObjectID == anAssociationId && x.ID == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return Json(new { isOK = false, message = "请选择删除的图片" });
            }
            var anAssociation = await _anAssociationExtension.GetAll().Include(x => x.User).Include(x => x.Avatar).Where(x => x.User.Id == User.Claims.FirstOrDefault().Value && x.ID == anAssociationId).FirstOrDefaultAsync();
            if (anAssociationId == null)
            {
                return Json(new { isOK = false, message = "您没有删除图片的权限" });
            }
            if (anAssociation.Avatar.ID == id)
            {
                anAssociation.Avatar = anAssociation.Avatar = await _businessImageExtension.GetAll().Where(x => x.Name == "默认图片" && x.IsSystem).FirstOrDefaultAsync();
                _anAssociationExtension.EditAndSave(anAssociation);
            }
            _businessImageExtension.DeleteAndSave(entity);
            return Json(new { isOK = true });
        }


        /// <summary>
        /// 删除社团成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteAnAssociationMember(Guid id)
        {
            var entity = await _anAnAssociationAndUserExtension.GetAll().Include(x => x.User).Include(x => x.AnAssociation).Where(x => x.ID == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return Json(new { isOK = true, message = "删除失败，此人早已不是社团成员" });
            }
            List<ApplicationUser> user = new List<ApplicationUser>();
            user.Add(entity.User);
            await AddMessageNotification(user, entity.AnAssociationId, entity.AnAssociation.Name, "社团成员退出，请及时查看", BusinessEmergencyEnum.一般);
            _anAnAssociationAndUserExtension.DeleteAndSave(entity);
            return Json(new { isOK = true });
        }

        /// <summary>
        /// 添加或编辑社团
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddOrEditAnAssociation(Guid? Id)
        {
            var user = User.Claims.FirstOrDefault();
            var anAssociation = new AnAssociation();
            if (Id != null)
            {
                anAssociation = await _anAssociationExtension.GetAll().Include(x => x.User).Where(x => x.ID == Guid.Parse(Id.ToString())).FirstAsync();
            }
            var anAssociationInput = new AnAssociationInput(anAssociation);
            return View("~/Views/BusinessView/ManageCenter/AddOrEditAnAssociation.cshtml", anAssociationInput);
        }

        /// <summary>
        /// 保存社团信息
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveAnAssociation(string anAssociationData)
        {
            var anAssociation = new AnAssociation();
            var activityInput = Newtonsoft.Json.JsonConvert.DeserializeObject<AnAssociationInput>(anAssociationData);
            activityInput.MapTo(anAssociation);
            var messageOf = "社团更新成功";
            //判断是否有该社团
            var isExists = this._anAssociationExtension.GetAll().Any(x => x.ID == activityInput.ID);
            if (!isExists)
            {
                messageOf = "社团创建成功";
                var user = User.Claims.FirstOrDefault();
                anAssociation.User = await _userManager.FindByIdAsync(user.Value);
                var anUser = new AnAssociationAndUser()
                {
                    User = await _userManager.FindByIdAsync(user.Value),
                    AnAssociationId = anAssociation.ID,
                    AnJurisdictionManager = AnJurisdiction.Founder
                };
                _anAnAssociationAndUserExtension.Add(anUser);
                anAssociation.Avatar = await _businessImageExtension.GetAll().Where(x => x.Name == "默认图片" && x.IsSystem).FirstOrDefaultAsync();
                anAssociation.IsDisable = true;
            }
            await _anAssociationRepository.AddOrEditAndSaveAsyn(anAssociation);
            _anAnAssociationAndUserExtension.Save();
            return Json(new { result = true, id = anAssociation.ID, message = messageOf });
        }
        #endregion


        /// <summary>
        /// 获取头像
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> HeadPortrait()
        {
            var user = await _userExtension.GetAll().Include(x => x.Avatar).Where(x => x.Id == User.Claims.FirstOrDefault().Value).FirstAsync();
            ViewBag.Head = user.Avatar.UploadPath;
            return View("~/Views/BusinessView/ManageCenter/HeadPortrait.cshtml");
        }

        /// <summary>
        /// 更改头像
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SaveHeadPortrait()
        {
            UploadPhone updatePhone = new UploadPhone(_hostingEnv);
            BusinessImage image = new BusinessImage();
            var file = Request.Form.Files.FirstOrDefault();
            var user = await _userManager.FindByIdAsync(User.Claims.FirstOrDefault().Value);

            //保存文件
            image.UploadPath = updatePhone.PhoneNewUpload(file, "User");

            image.UploaderID = Guid.Parse(user.Id);
            image.RelevanceObjectID = Guid.Parse(user.Id);
            _businessImageExtension.AddAndSave(image);
            user.Avatar = image;
            _userExtension.EditAndSave(user);
            return Json(true);
        }

        /// <summary>
        /// 密码设置
        /// </summary>
        /// <returns></returns>
        public IActionResult UserPassword()
        {
            return View("~/Views/BusinessView/ManageCenter/UserPassword.cshtml");
        }

        /// <summary>
        /// 保存密码
        /// </summary>
        /// <param name="password"></param
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveUserPassword(string password, string newPassword)
        {
            if (password == "")
            {
                return Json(new { isOK = false, message = "请输入密码" });
            }
            var user = User.Claims.FirstOrDefault();
            if (user == null)
            {
                return Json(new { isOK = false, message = "密码修改失败，请联系管理员" });
            }
            var userInfo = await _userManager.FindByIdAsync(user.Value);
            var result = await _signInManager.PasswordSignInAsync(userInfo.UserName, password, true, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return Json(new { isOK = false, message = "旧密码输入错误" });
            }
            userInfo.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(userInfo, newPassword);
            await _userExtension.SaveAsyn();
            return Json(new { isOK = true, message = "密码修改成功" });
        }

        #region 通知管理

        public IActionResult GetNotification()
        {
            return View("~/Views/BusinessView/ManageCenter/GetNotification.cshtml");
        }

        /// <summary>
        /// 获取当前用户的通知
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetMessageNotificationList(string keywork, string listPageParaJson)
        {
            //处理脚本传送的分页数据
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null)
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }
            listPagePara.PageSize = 7;
            var mnCollection = new List<MessageNotification>();
            var user = User.Claims.FirstOrDefault();

            //判断是否有筛选条件
            if (!String.IsNullOrEmpty(keywork))
            {
                //linq表达式处理筛选条件，

                Expression<Func<Entities.ApplicationOrganization.MessageNotification, bool>> condtionRelation = x =>
                 x.Name.Contains(keywork) ||
                  x.User.Name.Contains(keywork) ||
                 x.CreatedUser.Name.Contains(keywork);

                //var 

                //获取自己产参加的社团
                mnCollection = await _notificationExtension.GetAll().OrderByDescending(x => x.CreateDateTime).Include(x => x.User).Include(x => x.User).Include(x => x.CreatedUser)
                    .Where(condtionRelation).Where(x => x.User.Id == user.Value).ToListAsync();

            }
            else
            {
                //获取自己产参加的社团
                mnCollection = await _notificationExtension.GetAll().OrderByDescending(x => x.CreateDateTime).Include(x => x.User).Include(x => x.User).Include(x => x.CreatedUser)
                   .Where(x => x.User.Id == user.Value).ToListAsync();
            }
            //处理分页数据
            var meesageNotificationPageList = IQueryableExtensions.ToPaginatedList(mnCollection.AsQueryable(), listPagePara.PageIndex, listPagePara.PageSize);

            var meesageNotificationCollections = new List<MessageNotificationVM>();

            foreach (var meesageNotificationPage in meesageNotificationPageList)
            {
                meesageNotificationCollections.Add(new MessageNotificationVM(meesageNotificationPage));
            }

            var pageGroup = PagenateGroupRepository.GetItem(meesageNotificationPageList, 3, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/BusinessView/ManageCenter/GetNotificationList.cshtml", meesageNotificationCollections);
        }

        /// <summary>
        /// 获取单条通知
        /// </summary>
        /// <param name="notification"></param>
        public async Task<IActionResult> GetMessageNotificationDetailed(Guid id)
        {
            var notification = await _notificationExtension.GetAll().Include(x => x.User).Include(x => x.CreatedUser).Where(x => x.ID == id).FirstOrDefaultAsync();
            notification.isSee = true;
            _notificationExtension.EditAndSave(notification);
            var notificationVM = new MessageNotificationVM(notification);
            return View("~/Views/BusinessView/ManageCenter/GetMessageNotificationDetailed.cshtml", notificationVM);
        }

        /// <summary>
        /// 删除单条通知
        /// </summary>
        /// <param name="notification"></param>
        public async Task<IActionResult> DeleteMessageNotification(Guid id)
        {
            var notification = await _notificationExtension.GetAll().Where(x => x.ID == id).FirstOrDefaultAsync();
            _notificationExtension.DeleteAndSave(notification);
            return Json(true);
        }

        /// <summary>
        /// 删除全部通知
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DeleteMessageNotificationAll()
        {
            var user = User.Claims.FirstOrDefault();
            var notifications = await _notificationExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == user.Value).ToListAsync();
            foreach (var item in notifications)
            {
                _notificationExtension.Delete(item);
            }
            _notificationExtension.Save();
            return Json(true);
        }

        /// <summary>
        /// 全部标记为已读
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ReadMessageNotificationAll()
        {
            var user = User.Claims.FirstOrDefault();
            var notifications = await _notificationExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == user.Value).ToListAsync();
            foreach (var item in notifications)
            {
                item.isSee = true;
                _notificationExtension.Edit(item);
            }
            _notificationExtension.Save();
            return Json(true);
        }

        /// <summary>
        /// 标记单条数据为已读
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReadMessageNotification(Guid id)
        {
            var notification = await _notificationExtension.GetAll().Where(x => x.ID == id).FirstOrDefaultAsync();
            notification.isSee = true;
            _notificationExtension.EditAndSave(notification);
            return Json(true);
        }

        /// <summary>
        /// 获取未读通知数目
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> MessageNotificationNumber()
        {
            var user = User.Claims.FirstOrDefault();
            var notifications = await _notificationExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == user.Value && !x.isSee).ToListAsync();
            return Json(notifications.Count());
        }

        /// <summary>
        /// 通知的业务ID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task AddMessageNotification(List<ApplicationUser> users, Guid objectId, string name, string description, BusinessEmergencyEnum status)
        {
            var notification = new MessageNotification();
            foreach (var user in users)
            {
                notification = new Entities.ApplicationOrganization.MessageNotification()
                {
                    User = user,
                    CreatedUser = (await _userManager.FindByIdAsync(User.Claims.FirstOrDefault().Value)),
                    ObjectId = objectId,
                    Name = name,
                    Description = description,
                    Status = status
                };
                _notificationExtension.Add(notification);
            }
            _notificationExtension.Save();
        }

        #endregion

        /// <summary>
        /// 获取当前用户登录信息
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UserDetail()
        {
            var user = User.Claims.FirstOrDefault();
            //获取信息
            var entity = await _userExtension.GetAll().Include(x => x.Avatar).Where(x => x.Id == user.Value).FirstOrDefaultAsync();
            if (user == null || entity == null)
            {
                return RedirectToAction("NotLogin", "Home");
            }
            var userVM = new ApplicationUserVM(entity);
            return View("~/Views/BusinessView/ManageCenter/UserDetail.cshtml", userVM);
        }
    }
}
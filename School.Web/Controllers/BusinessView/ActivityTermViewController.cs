using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Common.JsonModels;
using School.DataAccess;
using School.DataAccess.Common;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.GroupOrganization;
using School.ViewModels.ApplicationOrganization;
using School.ViewModels.Attachments;
using School.ViewModels.GroupOrganization;
using School.ViewModels.GroupOrganization.ActivityTerms;

namespace School.Web.Controllers.BusinessView
{
    //[Area("BusinessView")]
    public class ActivityTermViewController : Controller
    {
        private readonly IEntityRepository<ActivityTerm> _activityTermRepository;
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;
        private readonly IEntityRepository<ActivityUser> _activityUserRepository;
        private readonly IDataExtension<ActivityUser> _activityUserExtension;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataExtension<ApplicationUser> _userExtension;
        private readonly IDataExtension<BusinessImage> _businessImageExtension;
        private readonly IDataExtension<MessageNotification> _notificationExtension;
        private readonly IDataExtension<ActivityComment> _commonExtension;
        public ActivityTermViewController(IEntityRepository<ActivityTerm> repository,
            IDataExtension<ActivityTerm> activityTermExtension,
            IEntityRepository<ActivityUser> activityUserRepository,
            IDataExtension<ActivityUser> activityUserExtension,
             UserManager<ApplicationUser> userManager,
              IDataExtension<BusinessImage> businessImageExtension,
              IDataExtension<ApplicationUser> userExtension, IDataExtension<MessageNotification> notificationExtension,
              IDataExtension<ActivityComment> commonExtension)
        {
            _activityTermRepository = repository;
            _activityTermExtension = activityTermExtension;
            _activityUserRepository = activityUserRepository;
            _activityUserExtension = activityUserExtension;
            _userManager = userManager;
            _businessImageExtension = businessImageExtension;
            _userExtension = userExtension;
            _notificationExtension = notificationExtension;
            _commonExtension = commonExtension;
        }

        public async Task<IActionResult> Index()
        {
            var userCount = 9;//默认获取9个人
            var users = await _userExtension.GetAll().Include(s => s.Avatar).ToListAsync();
            userCount = userCount > users.Count ? users.Count : userCount;  //判断是否够9个人
            var ran = new Random();
            var index = 0;
            var interests = new List<ApplicationUser>();//用于存储随机相同兴趣的人
            for (int i = 0; i < userCount; i++)
            {
                index = ran.Next(0, users.Count());
                interests.Add(users[index]);
                users.RemoveAt(index);
            }
            ViewBag.InterestUsers = interests;
            return View("~/Views/BusinessView/ActivityTermView/Index.cshtml");
        }

        public async Task<IActionResult> List(string keywork, string probably, string listPageParaJson)
        {
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null)
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }
            var atCollection = new List<ActivityTerm>();
            if (!String.IsNullOrEmpty(keywork))
            {
                Expression<Func<ActivityTerm, bool>> condtion = x =>
                   x.Name.Contains(keywork) ||
                   x.User.Name.Contains(keywork) ||
                   x.AnAssociation.Name.Contains(keywork) ||
                   x.Address.Contains(keywork);
                atCollection = await _activityTermExtension.GetAll().Include(x => x.User).Include(x => x.AnAssociation).Include(x => x.Avatar).Where(condtion).Where(x => x.Status == ActivityStatus.未开始&&x.IsDisable).ToListAsync();
                if (probably == "1")
                {
                    atCollection = atCollection.Where(x => x.AnAssociation != null).ToList();
                }
                else if (probably == "2")
                {
                    atCollection = atCollection.Where(x => x.User != null && x.AnAssociation == null).ToList();
                }
            }
            else
            {
                atCollection = await _activityTermExtension.GetAll().Include(x => x.User).Include(x => x.AnAssociation).Include(x => x.Avatar).Where(x => x.Status == ActivityStatus.未开始 && x.IsDisable).ToListAsync();
                if (probably == "1")
                {
                    atCollection = atCollection.Where(x => x.AnAssociation != null).ToList();
                }
                else if (probably == "2")
                {
                    atCollection = atCollection.Where(x => x.User != null&&x.AnAssociation==null).ToList();
                }
            }
            var activityTermPageList = IQueryableExtensions.ToPaginatedList(atCollection.AsQueryable(), listPagePara.PageIndex, 12);

            var activityCollections = new List<ActivityTermVM>();

            foreach (var activityTermPage in activityTermPageList)
            {
                activityCollections.Add(new ActivityTermVM(activityTermPage) { EnteredNumber = (await _activityUserExtension.GetAll().Where(x => x.ActivityTermId == activityTermPage.ID).ToListAsync()).Count });
            }

            var pageGroup = PagenateGroupRepository.GetItem(activityTermPageList, 5, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/BusinessView/ActivityTermView/_List.cshtml", activityCollections);
        }

        /// <summary>
        /// 获取活动明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Detailied(Guid id)
        {
            var i = 0;
            //获取活动，
            var entity = await _activityTermRepository.GetSingleAsyn(id, x => x.Avatar, x => x.User, x => x.AnAssociation, x => x.User.Avatar, x => x.AnAssociation.Avatar, x => x.AnAssociation.User);
            //获取参与活动的用户
            var activityUsers = await _activityUserExtension.GetAll().Include(x => x.User).Include(x => x.ActivityTerm).Where(x => x.ActivityTermId == id).ToListAsync();
            ViewBag.Number = activityUsers.Count();
            var Users = new List<ApplicationUser>();
            //获取社团用户
            foreach (var activityUser in activityUsers)
            {
                Users.Add(await _userManager.FindByIdAsync(activityUser.User.ToString()));
            }
            ViewBag.Users = Users;
            var entityVm = new ActivityTermVM(entity);
            return View("~/Views/BusinessView/ActivityTermView/Detailied.cshtml", entityVm);
        }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddActivity(Guid id)
        {

            if (id == Guid.Empty)
            {
                return Json(new { isOK = false, message = "请选择活动" });
            }
            var activity = await _activityTermRepository.GetAll().Include(x => x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
            if (activity == null)
            {
                return Json(new { isOK = false, message = "活动选择失败，不存在改活动" });
            }
            var user = User.Claims.FirstOrDefault();
            if (user == null)
            {
                return Json(new { isOK = false, message = "请登录!!!" });
            }
            var userInfo = await _userManager.FindByIdAsync(user.Value);
            var entity = _activityUserExtension.GetAll().Include(x => x.ActivityTerm).Include(x => x.ActivityTerm.User)
                .Where(x => x.User.Id == user.Value && x.ActivityTermId == id).FirstOrDefault();
            if (entity != null)
            {
                return Json(new { isOK = false, message = "你已经加入该活动，不可重复加入" });
            };
            var activityUser = new ActivityUser()
            {
                User = userInfo,
                ActivityTermId = id
            };
            var isOK = await _activityUserRepository.AddOrEditAndSaveAsyn(activityUser);
            await AddMessageNotification(activity.User, id, activity.Name, "活动新成员:" + userInfo.Name, BusinessEmergencyEnum.一般);
            if (isOK)
            {
                return Json(new { isOK = true, message = "添加成功" });
            }
            else
            {
                return Json(new { isOK = false, message = "添加失败" });
            }
        }

        /// <summary>
        /// 取消活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CancelActivity(Guid id)
        {
            var user = User.Claims.FirstOrDefault();
            if (user == null)
            {
                return Json(new { isOK = false, message = "请登录!!!" });
            }
            var entity = _activityUserExtension.GetAll().Include(x => x.ActivityTerm).Include(x => x.ActivityTerm.User)
                .Where(x => x.User.Id == user.Value && x.ActivityTermId == id).FirstOrDefault();
            if (entity == null)
            {
                return Json(new { isOK = false, message = "你已经加入该活动，不可重复加入" });
            };
            _activityUserExtension.DeleteAndSave(entity);
            var userInfo = await _userManager.FindByIdAsync(user.Value);
            await AddMessageNotification(entity.ActivityTerm.User, id, entity.ActivityTerm.Name, userInfo.Name + "取消活动:" + userInfo.Name, BusinessEmergencyEnum.一般);
            return Json(new { isOK = true, message = "取消成功" });
        }

        /// <summary>
        /// 获取社团对应的图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ImageList(Guid id)
        {
            var entitys = await _businessImageExtension.FindByAsyn(x => x.RelevanceObjectID == id);

            var entityList = new List<BusinessImageVM>();
            foreach (var entity in entitys)
            {
                entityList.Add(new BusinessImageVM(entity));
            }
            return View("~/Views/BusinessView/ActivityTermView/_ImageList.cshtml", entityList);
        }

        /// <summary>
        /// 获取社团成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> UserList(Guid id)
        {
            //获取社团用户
            var anAdminUsers = await _activityUserExtension.GetAll().Include(x => x.User).Where(x => x.ActivityTermId == id).ToListAsync();
            var AnAdmins = new List<ApplicationUserVM>();
            foreach (var item in anAdminUsers)
            {
                var user = await _userExtension.GetAllMultiCondition(x => x.Id == item.User.Id, x => x.Avatar).FirstOrDefaultAsync();
                AnAdmins.Add(new ApplicationUserVM(user));
            }
            return View("~/Views/BusinessView/ActivityTermView/_UserList.cshtml", AnAdmins);
        }

        /// <summary>
        /// 通知的业务ID
        /// </summary>
        /// <param name="objectId">业务id</param>
        /// <param name="name">活动名称</param>
        /// <param name="description">活动说明</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public async Task AddMessageNotification(ApplicationUser user, Guid objectId, string name, string description, BusinessEmergencyEnum status)
        {
            var notification = new Entities.ApplicationOrganization.MessageNotification()
            {
                User = user,
                CreatedUser = (await _userManager.FindByIdAsync(User.Claims.FirstOrDefault().Value)),
                ObjectId = objectId,
                Name = name,
                Description = description,
                Status = status
            };
            _notificationExtension.AddAndSave(notification);
        }

        /// <summary>
        /// 获取活动评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetComments(Guid id, string listPageParaJson)
        {
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null)
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }

            ViewBag.activityId = id;
            //获取一级评论
            var commentList = await _commonExtension.GetAll().OrderByDescending(x => x.CommentDataTime).Include(x => x.Activity).Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.Activity.ID == id && x.ParentGrade == null).ToListAsync();
            var comments = new List<CommentVM>();
            foreach (var item in commentList)
            {
                var commentVM = new CommentVM(item);
                commentVM.CommentChildrens = await _commonExtension.GetAll().OrderBy(x => x.CommentDataTime).Include(x => x.Activity).Include(x => x.AcceptUser).Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.ParentGrade == item.ID).ToListAsync();
                comments.Add(commentVM);
            }
            var commentPageList = IQueryableExtensions.ToPaginatedList(comments.AsQueryable(), listPagePara.PageIndex, listPagePara.PageSize);
            var commentCollections = new List<CommentVM>();
            foreach (var commentTermPage in commentPageList)
            {
                commentCollections.Add(commentTermPage);
            }
            var pageGroup = PagenateGroupRepository.GetItem(commentPageList, 5, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/BusinessView/ActivityTermView/_GetComments.cshtml", commentCollections);
        }


        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="id">活动id</param>
        /// <param name="ParentGrade">父级id</param>
        /// <returns></returns>
        public async Task<IActionResult> AddComment(Guid id, Guid? parentGrade, Guid acceptUserId, string comment)
        {
            var user = await _userManager.FindByIdAsync(User.Claims.FirstOrDefault().Value);
            if (user == null)
            {
                return Json(new { isOK = false, message = "您还未登录，请登录后再评论" });
            }
            if (id == Guid.Empty)
            {
                return Json(new { isOK = false, message = "请选择评论的活动" });
            }
            var activity = await _activityTermExtension.GetAll().Where(x => x.ID == id).FirstOrDefaultAsync();
            //评论的父级
            var parentGradeEntity = new ActivityComment();
            if (parentGrade != null && parentGrade != Guid.Empty)
            {
                parentGradeEntity = await _commonExtension.GetAll().Include(x => x.User).Where(x => x.ID == parentGrade).FirstOrDefaultAsync();
                if (parentGradeEntity == null)
                {
                    return Json(new { isOK = false, message = "评论失败" });
                }
            }
            var acceptUser = parentGradeEntity.User;
            if (acceptUserId != null && acceptUserId != Guid.Empty)
            {
                acceptUser = await _userManager.FindByIdAsync(acceptUserId.ToString());
            }
            _commonExtension.AddAndSave(new ActivityComment()
            {
                Name = activity.Name,
                Activity = activity,
                User = user,
                AcceptUser = acceptUser,
                ParentGrade = parentGrade,
                Comment = comment,
                Hierarchy = parentGrade != null && parentGrade != Guid.Empty ? 1 : 0
            });
            return Json(new { isOK = true, message = "评论成功" });
        }

        /// <summary>
        /// 获取活动评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<List<CommentVM>> GetComments(Guid id)
        {
            //获取一级评论
            var commentList = await _commonExtension.GetAll().OrderBy(x => x.CommentDataTime).Include(x => x.Activity).Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.Activity.ID == id && x.Hierarchy == 0).ToListAsync();
            var comments = new List<CommentVM>();
            foreach (var item in commentList)
            {
                var commentVM = new CommentVM(item);
                commentVM.CommentChildrens = await _commonExtension.GetAll().OrderBy(x => x.CommentDataTime)
                    .Include(x => x.Activity).Include(x => x.AcceptUser).Include(x => x.User)
                    .Include(x => x.User.Avatar).Where(x => x.ParentGrade == item.ID && x.Hierarchy == 1).ToListAsync();
                comments.Add(commentVM);
            }
            return comments;
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var entity = await _commonExtension.GetAll().Where(x => x.ID == id).FirstOrDefaultAsync();
            _commonExtension.DeleteAndSave(entity);
            return Json(new { isOK = true , message = "删除评论成功" });
        }
    }
}
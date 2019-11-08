using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.DataAccess;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.GroupOrganization;
using School.ViewModels.ApplicationOrganization;
using School.ViewModels.GroupOrganization;
using School.ViewModels.GroupOrganization.ActivityTerms;

namespace School.Web.Controllers.BusinessView
{
    public class PersonalCenterController : Controller
    {
        private readonly IDataExtension<ActivityUser> _activityUserExtension;
        private readonly IDataExtension<AnAssociation> _anAssociationExtension;
        private readonly IDataExtension<AnAssociationAndUser> _userAnAssociationExtension;
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;
        private readonly IDataExtension<Hobby> _hobbyExtension;
        private readonly IDataExtension<BusinessImage> _businessImageExtension;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataExtension<ApplicationUser> _userExtension;
        private readonly IDataExtension<ApplicationUserAndHobby> _userHoppyExtension;
        private readonly IDataExtension<UserFriend> _userFriendExtension;
        private readonly IDataExtension<ActivityComment> _commonExtension;

        /// <summary>
        /// 
        /// </summary>
        public PersonalCenterController(IDataExtension<ActivityUser> activityUserExtension, IDataExtension<AnAssociation> anAssociationExtension,
            IDataExtension<AnAssociationAndUser> userAnAssociationExtension,
            IDataExtension<Hobby> hobbyExtension,
            IDataExtension<ActivityTerm> activityTermExtension,
             IDataExtension<BusinessImage> businessImageExtension,
             UserManager<ApplicationUser> userManager,
             IDataExtension<ApplicationUser> userExtension,
             IDataExtension<ApplicationUserAndHobby> userHoppyExtension,
             IDataExtension<UserFriend> userFriendExtension,
             IDataExtension<ActivityComment> commonExtension)
        {
            _activityUserExtension = activityUserExtension;
            _activityTermExtension = activityTermExtension;
            _hobbyExtension = hobbyExtension;
            _businessImageExtension = businessImageExtension;
            _userManager = userManager;
            _userExtension = userExtension;
            _userHoppyExtension = userHoppyExtension;
            _userFriendExtension = userFriendExtension;
            _anAssociationExtension = anAssociationExtension;
            _userAnAssociationExtension = userAnAssociationExtension;
            _commonExtension = commonExtension;
        }

        /// <summary>
        /// 管理中心初始界面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(string id)
        {
            var user = User.Claims.FirstOrDefault();
            //判断用户是否登录
            if (user == null)
            {
                return RedirectToAction("NotLogin", "Home");
            }
            if (id == null)
            {
                id = user.Value;
            }
            var userInfo = await _userExtension.GetAll().Include(x => x.Avatar).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (userInfo == null)
            {
                return RedirectToAction("NotLogin", "Home");
            }
            var userVM = new ApplicationUserVM(userInfo);
            var hobbys = await _userHoppyExtension.GetAll().Include(x => x.Hobby).Include(x => x.User).Where(x => x.User.Id == id).ToListAsync();
            userVM.Hobbys = new List<Hobby>();
            foreach (var item in hobbys)
            {
                userVM.Hobbys.Add(item.Hobby);
            }

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

            //获取用户好友数量
            var userFriens = await _userFriendExtension.GetAll().Where(x => x.UserID == id).ToListAsync();
            ViewBag.Friends = userFriens.Count;

            //获取社团数量
            var anAssociations = await _anAssociationExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == id).ToListAsync();
            var userAnAssociations = await _userAnAssociationExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == id).ToListAsync();
            ViewBag.AnAssociations = anAssociations.Count + userAnAssociations.Count;

            //获取活动数量
            //var activitys = await _activityTermExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == id && x.Status == ActivityStatus.已结束).ToListAsync();
            var userActivitys = await _activityUserExtension.GetAll().Include(x => x.User).Where(x => x.User.Id == id && x.ActivityTerm.Status == ActivityStatus.已结束).ToListAsync();
            ViewBag.Activitys =userActivitys.Count;

            return View("~/Views/BusinessView/PersonalCenter/Index.cshtml", userVM);
        }

        /// <summary>
        /// 获取用户活动列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> List(string id, string pageNumber)
        {
            if (pageNumber == "undefined")
            {
                pageNumber = "0";
            }
            ////获取自己创建的
            //var activityTermCollection = await _activityTermExtension.GetAll().Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.Status == ActivityStatus.已结束 && x.User.Id == id && x.IsDisable).ToListAsync();

            //获取自己参与的
            var activityUserCollection = await _activityUserExtension.GetAll().OrderByDescending(x=>x.ActivityTerm.StartDataTime).Include(x => x.ActivityTerm).Include(x => x.User).Include(x => x.User.Avatar)
                .Include(x => x.ActivityTerm.User).Include(x => x.ActivityTerm.User.Avatar).Where(x => x.ActivityTerm.Status == ActivityStatus.已结束 && x.User.Id == id && x.ActivityTerm.IsDisable).ToListAsync();
            var activitys = new List<ActivityTerm>();
            //activitys = activityUserCollection.Select(x => x.ActivityTerm).ToList();
            //合并、去重
            //var activitys = activityTermCollection.Union(activityUser).ToList();
            activityUserCollection = activityUserCollection.Skip(Convert.ToInt32(pageNumber) * 10).Take(10).ToList();
            if (activityUserCollection.Count == 0)
            {
                return Json(false);
            }
            var activityTermVM = new List<ActivityTermVM>();
            foreach (var activity in activityUserCollection)
            {
                var acVM = new ActivityTermVM(activity.ActivityTerm);
                acVM.Images = await _businessImageExtension.GetAll().Where(x => x.RelevanceObjectID == activity.ActivityTerm.ID).ToListAsync();
                acVM.Comments = await GetComments(activity.ActivityTerm.ID);
                //获取评论数量
                acVM.CommentNumber = (await _commonExtension.GetAll().Include(x => x.Activity).Where(x => x.Activity.ID == activity.ActivityTerm.ID).ToListAsync()).Count;
                acVM.ActivityUser = activity.User;
                activityTermVM.Add(acVM);
            }
            return View("~/Views/BusinessView/PersonalCenter/_List.cshtml", activityTermVM);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteActivityRecord(Guid id)
        {
            var entity = await _activityTermExtension.GetAll().Include(x=>x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
            if (entity.User.Id != User.Claims.FirstOrDefault().Value)
            {
                return Json(new { isOK = false, message = "删除失败，您没有权限操作" });
            }
            if (entity == null)
            {
                return Json(new { isOK = false, message = "删除失败，请联系管理员" });
            }
            _activityTermExtension.DeleteAndSave(entity);
            return Json(new { isOK = true, message = "删除历史活动成功" });
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="id">活动id</param>
        /// <param name="ParentGrade">父级id</param>
        /// <returns></returns>
        public async Task<IActionResult> AddComment(Guid id, Guid? parentGradeBusiness, Guid acceptUserId, string comment)
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
            if (parentGradeBusiness != null && parentGradeBusiness != Guid.Empty)
            {
                parentGradeEntity = await _commonExtension.GetAll().Include(x => x.User).Where(x => x.ID == parentGradeBusiness).FirstOrDefaultAsync();
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
                ParentGrade = parentGradeBusiness,
                Comment = comment,
                Hierarchy = parentGradeBusiness != null && parentGradeBusiness != Guid.Empty ? 1 : 0
            });
            return Json(new { isOK = true, message = "评论成功" });
        }

        /// <summary>
        /// 获取活动评论呈现到前端
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetActivityComments(Guid id)
        {
            var comments = await GetComments(id);
            return View("~/Views/BusinessView/PersonalCenter/_GetActivityComments.cshtml", comments);
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
            return Json(new { isOK = true ,message="评论删除成功"});
        }
    }
}
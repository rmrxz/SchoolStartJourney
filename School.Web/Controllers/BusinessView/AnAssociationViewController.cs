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

namespace School.Web.Controllers.BusinessView
{
    public class AnAssociationViewController : Controller
    {
        private readonly IEntityRepository<AnAssociation> _anAssociationRepository;
        private readonly IDataExtension<AnAssociation> _anAssociationExtension;
        private readonly IDataExtension<AnAssociationAndUser> _anAssociationAndUserExtension;
        private readonly IDataExtension<BusinessImage> _businessImageExtension;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataExtension<ApplicationUser> _userExtension;
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;
        private readonly IDataExtension<MessageNotification> _notificationExtension;
        public AnAssociationViewController(IEntityRepository<AnAssociation> anAssociationRepository,
           IDataExtension<AnAssociation> anAssociationExtension,
           IDataExtension<AnAssociationAndUser> anAssociationAndUserExtension,
            UserManager<ApplicationUser> userManager,
             IDataExtension<ApplicationUser> userExtension,
             IDataExtension<BusinessImage> businessImageExtension,
             IDataExtension<ActivityTerm> activityTermExtension,
            IDataExtension<MessageNotification> notificationExtension)
        {
            _anAssociationRepository = anAssociationRepository;
            _anAssociationExtension = anAssociationExtension;
            _anAssociationAndUserExtension = anAssociationAndUserExtension;
            _userManager = userManager;
            _userExtension = userExtension;
            _businessImageExtension = businessImageExtension;
            _activityTermExtension = activityTermExtension;
            _notificationExtension = notificationExtension;
        }

        public async Task<IActionResult> Index()
        {
            int getCount = 4;//获取条目数
            var datas = await this._anAssociationExtension.GetAll().Include(x => x.Avatar).ToListAsync();
            var anAssociationRecommend = new List<AnAssociationVM>();
            foreach (var item in datas)
            {
                var anVM = new AnAssociationVM(item);
                anVM.AnAssociationNum = (await _anAssociationAndUserExtension.GetAll().Where(x => x.AnAssociation.ID == item.ID).ToListAsync()).Count;
                anAssociationRecommend.Add(anVM);
            }
            var listAns = anAssociationRecommend.OrderByDescending(s => s.CreateDataTime).OrderByDescending(s => s.AnAssociationNum).Take(getCount).ToList();
            ViewBag.ListAns = listAns;

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
            return View("~/Views/BusinessView/AnAssociationView/Index.cshtml");
        }

        public async Task<IActionResult> List(string keywork, string listPageParaJson)
        {
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null)
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }
            var anCollection = new List<AnAssociation>();
            if (!String.IsNullOrEmpty(keywork))
            {
                Expression<Func<AnAssociation, bool>> condtion = x =>
                   x.Name.Contains(keywork) ||
                   x.User.Name.Contains(keywork) ||
                   x.SchoolAddress.Contains(keywork);
                anCollection =await _anAssociationExtension.GetAll().Include(x => x.User).Include(x => x.Avatar).Where(condtion).Where(x=>x.IsDisable).ToListAsync();
            }
            else
            {
                anCollection =await _anAssociationExtension.GetAll().Include(x => x.User).Include(x => x.Avatar).Where(x=>x.IsDisable).ToListAsync();
            }
            var activityTermPageList = IQueryableExtensions.ToPaginatedList(anCollection.AsQueryable(), listPagePara.PageIndex, 12);

            var anAssociationCollections = new List<AnAssociationVM>();
            foreach (var activityTermPage in activityTermPageList)
            {
                var anNum = await _anAssociationAndUserExtension.GetAll().Where(x => x.AnAssociationId == activityTermPage.ID).Select(x=>x.AnAssociation).ToListAsync();
                anAssociationCollections.Add(new AnAssociationVM(activityTermPage) { AnAssociationNum = anNum .Count()});
            }

            var pageGroup = PagenateGroupRepository.GetItem(activityTermPageList, 5, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/BusinessView/AnAssociationView/_List.cshtml", anAssociationCollections);
        }

        public async Task<IActionResult> Detailed(Guid id)
        {
            var entity = await _anAssociationRepository.GetSingleAsyn(id, x => x.User, x => x.User.Avatar, x => x.Avatar);
            //获取社团用户
            var anAdminUsers = await _anAssociationAndUserExtension.GetAll().Include(x => x.User).Where(x => x.AnAssociationId == entity.ID).ToListAsync();
            var AnAdmins = new List<ApplicationUser>();
            //获取社团管理者
            foreach (var item in anAdminUsers)
            {
                if (item.AnJurisdictionManager == AnJurisdiction.Admin)
                {
                    AnAdmins.Add(await _userExtension.GetAll().Include(x => x.Avatar).Where(x => x.Id == item.User.Id).FirstOrDefaultAsync());
                }
            }
            ViewBag.AnAdmins = AnAdmins;

            var entityVM = new AnAssociationVM(entity);
            var acNum = await _activityTermExtension.GetAll().Include(x => x.AnAssociation).Where(x => x.AnAssociation.ID == id).ToListAsync();
            entityVM.acNum = acNum.Count;
            entityVM.userNum = anAdminUsers.Count();
            return View("~/Views/BusinessView/AnAssociationView/_Detailed.cshtml", entityVM);

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
            return View("~/Views/BusinessView/AnAssociationView/_ImageList.cshtml", entityList);
        }

        /// <summary>
        /// 获取社团成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> UserList(Guid id)
        {
            //获取社团用户
            var anAdminUsers = await _anAssociationAndUserExtension.GetAll().Include(x => x.User).Where(x => x.AnAssociationId == id).ToListAsync();
            var AnAdmins = new List<ApplicationUserVM>();
            foreach (var item in anAdminUsers)
            {
                var user = await _userExtension.GetAllMultiCondition(x => x.Id == item.User.Id, x => x.Avatar).FirstOrDefaultAsync();
                AnAdmins.Add(new ApplicationUserVM(user));
            }
            return View("~/Views/BusinessView/AnAssociationView/_UserList.cshtml", AnAdmins);
        }

        /// <summary>
        /// 添加社团
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddAnAssociation(Guid id)
        {
            var entity = await _anAssociationRepository.GetAll().Include(x=>x.User).Where(x=>x.ID==id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return Json(new { isOK = false, message = "没有这条数据" });
            }
            var user = User.Claims.FirstOrDefault();
            if (user == null)
            {
                return Json(new { isOK = false, message = "请登录!!!" });
            }
            var entityUser = _anAssociationAndUserExtension.GetAll().Include(x=>x.AnAssociation).Include(x=>x.AnAssociation.User)
                .Include(x => x.User).Where(x => x.User.Id == user.Value && x.AnAssociationId == id).FirstOrDefault();
            if (entityUser != null)
            {
                return Json(new { isOK = false, message = "你已经是社团成员，不可重复加入" });
            };
            var userInfo = await _userManager.FindByIdAsync(user.Value);
            var anAssociationAndUser = new AnAssociationAndUser { User = userInfo, AnAssociationId = id };
            _anAssociationAndUserExtension.AddAndSave(anAssociationAndUser);
            await AddMessageNotification(entity.User, id, entity.Name, entity.Name+"社团新成员:" + userInfo.Name, BusinessEmergencyEnum.一般);
            return Json(new { isOK = true, message = "成功加入" });
        }

        /// <summary>
        /// 退出社团
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CancelAnAssociation(Guid id)
        {
            var user = User.Claims.FirstOrDefault();
            if (user == null)
            {
                return Json(new { isOK = false, message = "请登录!!!" });
            }
            
            var entityUser = _anAssociationAndUserExtension.GetAll().Include(x => x.User).Include(x => x.AnAssociation).Include(x => x.AnAssociation.User)
                .Where(x => x.User.Id == user.Value && x.AnAssociationId == id).FirstOrDefault();
            if (entityUser == null)
            {
                return Json(new { isOK = false, message = "你不是该社团成员" });
            }
            var userInfo = await _userManager.FindByIdAsync(user.Value);
            _anAssociationAndUserExtension.DeleteAndSave(entityUser);
            await AddMessageNotification(entityUser.AnAssociation.User, id, entityUser.AnAssociation.Name, userInfo.Name + "退出社团:"+ entityUser.AnAssociation.Name, BusinessEmergencyEnum.一般);
            return Json(new { isOK = true, message = "成功退出" });
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
    }
}
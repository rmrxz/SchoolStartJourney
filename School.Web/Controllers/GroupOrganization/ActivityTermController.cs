using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Common.JsonModels;
using School.DataAccess;
using School.DataAccess.Common;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.GroupOrganization;
using School.ViewModels.GroupOrganization.ActivityTerms;
using Microsoft.EntityFrameworkCore;
using School.Entities.Attachments;
using School.Web.Common;
using Microsoft.AspNetCore.Hosting;
using School.ViewModels.GroupOrganization;
using Microsoft.AspNetCore.Authorization;

namespace School.Web.Controllers.GroupOrganization
{
    public class ActivityTermController : Controller
    {
        private readonly IEntityRepository<ActivityTerm> _activityTermRepository;
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IDataExtension<BusinessImage> _businessImageRepository;
        private readonly IDataExtension<BusinessImage> _businessImageExtension;
        private readonly IDataExtension<ActivityUser> _activityUserExtension;
        private readonly IDataExtension<ActivityComment> _commonExtension;
        private readonly IDataExtension<HomeExhibition> _homeExhibitionExtension;
        public ActivityTermController(IEntityRepository<ActivityTerm> repository, 
            IDataExtension<ActivityTerm> activityTermExtension,
             UserManager<ApplicationUser> userManager,
             IDataExtension<BusinessImage> businessImageRepository,
             IHostingEnvironment hostingEnv,
            IDataExtension<BusinessImage> businessImageExtension,
            IDataExtension<ActivityUser> activityUserExtension,
            IDataExtension<ActivityComment> commonExtension,
            IDataExtension<HomeExhibition> homeExhibitionExtension)
        {
            _activityTermRepository = repository;
            _activityTermExtension = activityTermExtension;
            _userManager = userManager;
            _businessImageRepository = businessImageRepository;
            _hostingEnv = hostingEnv;
            _businessImageExtension = businessImageExtension;
            _activityUserExtension = activityUserExtension;
            _commonExtension = commonExtension;
            _homeExhibitionExtension = homeExhibitionExtension;
        }

        /// <summary>
        /// 活动管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var atCollection = await _activityTermExtension.GetAll().OrderBy(x => x.Status).ThenByDescending(x => x.CreateDateTime).Include(x => x.User).ToListAsync();
            var atVmCollection = new List<ActivityTermVM>();
            foreach (var at in atCollection)
            {
                var atVM = new ActivityTermVM(at);
                atVmCollection.Add(atVM);
            }

            var pageSize = 10;
            var pageIndex = 1;
            var activityTermList = atVmCollection.FirstOrDefault();
            var activityTermCollections = new List<ActivityTermVM>();
            //处理分页

            var activityTermCollectionPageList = IQueryableExtensions.ToPaginatedList(atVmCollection.AsQueryable<ActivityTermVM>(), pageIndex, pageSize);

            foreach (var activityTermCollection in activityTermCollectionPageList)
            {
                activityTermCollections.Add(activityTermCollection);
            }

            //提取当前分页关联的分页器实例
            var pageGroup = PagenateGroupRepository.GetItem(activityTermCollectionPageList, 5, pageIndex);
            ViewBag.PageGroup = pageGroup;

            var listPageParameter = new ListPageParameter()
            {
                PageIndex = activityTermCollectionPageList.PageIndex,
                Keyword = "",
                PageSize = activityTermCollectionPageList.PageSize,
                ObjectTypeID = "",
                ObjectAmount = activityTermCollectionPageList.TotalCount,
                SortDesc = "Default",
                SortProperty = "Name",
                PageAmount = 0,
                SelectedObjectID = ""
            };
            ViewBag.PageParameter = listPageParameter;
            return View("~/Views/GroupOrganization/ActivityTerm/Index.cshtml", activityTermCollections);

        }

        /// <summary>
        /// 根据关键词检索活动数据集合，返回给前端页面
        /// </summary>
        /// <param name="keywork"></param>
        public async Task<IActionResult> List(string keywork, string listPageParaJson)
        {
            var listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            var atVMCollection = new List<ActivityTermVM>();
            if (!String.IsNullOrEmpty(keywork) && keywork != "undefined")
            {
                Expression<Func<ActivityTerm, bool>> condtion = x =>
                   x.Name.Contains(keywork) ||
                   x.Address.Contains(keywork);
                var activityTermCollection = await _activityTermExtension.GetAll().OrderBy(x => x.Status).ThenByDescending(x => x.CreateDateTime).Include(x=>x.User).Where(condtion).ToListAsync();
                foreach (var at in activityTermCollection)
                {
                    atVMCollection.Add(new ActivityTermVM(at));
                }
            }
            else
            {
                var activityTermCollection = await _activityTermExtension.GetAll().OrderBy(x => x.Status).ThenByDescending(x => x.CreateDateTime).Include(x => x.User).ToListAsync();
                foreach (var at in activityTermCollection)
                {
                    atVMCollection.Add(new ActivityTermVM(at));
                }
            }
            var activityTermPageList = IQueryableExtensions.ToPaginatedList(atVMCollection.AsQueryable(), listPagePara.PageIndex, listPagePara.PageSize);

            var activityCollections = new List<ActivityTermVM>();

            foreach (var activityTermPage in activityTermPageList)
            {
                activityCollections.Add(activityTermPage);
            }

            var pageGroup = PagenateGroupRepository.GetItem(activityTermPageList, 5, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/GroupOrganization/ActivityTerm/_List.cshtml", activityCollections);
        }

        /// <summary>
        /// 用于存储或更新活动
        /// </summary>
        /// <param name="atVM"></param>
        /// <returns></returns>
        public async Task<IActionResult> Sava([Bind("ID,Name,Description,Address,MaxNumber,AnAssociationID,SignDataTime,MaxNumber,EndDataTime,StartDataTime,Expenses")]ActivityTermVM atVM)
        {
            var at = new ActivityTerm();
            at = await _activityTermRepository.GetSingleAsyn(atVM.ID);
            if (at == null)
            {
                at = new ActivityTerm();
                var userclaims = User.Claims.FirstOrDefault();
                var userData = await _userManager.FindByIdAsync(userclaims.Value.ToString());
                at.User = userData;
                at.IsDisable = true;
            }
            atVM.MapToAT(at);
            var savaStatus = await _activityTermRepository.AddOrEditAndSaveAsyn(at);
            if (savaStatus)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "数据保存出现异常，无法创建活动。");
                return View("~/Views/GroupOrganization/ActivityTerm/CreateOrEdit.cshtml", atVM);
            }
        }

        /// <summary>
        /// 增或者编辑人员数据的处理
        /// </summary>
        /// <param name="id">活动对象的ID属性值，如果这个值在系统中找不到具体的对象，则看成是新建对象。</param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var at = await _activityTermRepository.GetSingleAsyn(id,x=>x.User,x=>x.AnAssociation);
            if (at == null)
            {
                at = new ActivityTerm();
            }
            var atVM = new ActivityTermVM(at);
            return View("~/Views/GroupOrganization/ActivityTerm/CreateOrEdit.cshtml", atVM);
        }

        /// <summary>
        /// 根据ID查询对应的活动信息，
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(Guid id)
        {
            var activityTerm = await _activityTermRepository.GetSingleAsyn(id,x=>x.User,x=>x.AnAssociation);
            var atVM = new ActivityTermVM(activityTerm);
            atVM.Images = await _businessImageRepository.GetAll().Where(x => x.RelevanceObjectID == id).ToListAsync();
            //获取参与活动的用户
            atVM.Members = await _activityUserExtension.GetAll().Include(x => x.User).Include(x => x.User.Avatar).Include(x => x.ActivityTerm).Where(x => x.ActivityTermId == id).ToListAsync();
            ViewBag.MemberNumber = atVM.Members.Count();
            ViewBag.CommentNumber = (await _commonExtension.GetAll().Include(x => x.Activity).Where(x => x.Activity.ID == id).ToListAsync()).Count();
            return PartialView("~/Views/GroupOrganization/ActivityTerm/_Detail.cshtml", atVM);
        }

        public async Task<IActionResult> ActivityComment(Guid id, string listPageParaJson)
        {
            //获取一级评论
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null)
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }
            //获取一级评论
            var commentList = await _commonExtension.GetAll().OrderByDescending(x => x.CommentDataTime).Include(x => x.Activity).Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.Activity.ID == id && x.ParentGrade == null&&x.User!=null).ToListAsync();
            var comments = new List<CommentVM>();
            foreach (var item in commentList)
            {
                var commentVM = new CommentVM(item);
                commentVM.CommentChildrens = await _commonExtension.GetAll().OrderBy(x => x.CommentDataTime).Include(x => x.Activity).Include(x => x.AcceptUser).Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.ParentGrade == item.ID && x.User != null).ToListAsync();
                comments.Add(commentVM);
            }
            var commentPageList = IQueryableExtensions.ToPaginatedList(comments.AsQueryable(), listPagePara.PageIndex, 10);
            var commentCollections = new List<CommentVM>();
            foreach (var commentTermPage in commentPageList)
            {
                commentCollections.Add(commentTermPage);
            }
            var pageGroup = PagenateGroupRepository.GetItem(commentPageList, 5, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return PartialView("~/Views/GroupOrganization/ActivityTerm/ActivityComment.cshtml", commentCollections);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _activityTermRepository.DeleteAndSaveAsyn(id);
            return Json(status);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult UploadPicture(Guid id)
        {
            ViewBag.Id = id;
            return View("~/Views/GroupOrganization/ActivityTerm/UploadPicture.cshtml");
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> SavaUploaPhone(Guid id)
        {
            //TODO 获取对应的图片存储到服务器上和数据库上（业务ID ，当前用户ID ，图片路径）

            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
            {
                return Json(new { isOk = false, message = "请选择上传的图片" });
            }
            var user = User.Claims.FirstOrDefault();
            if (user == null)
            {

                //return Json(new { isOK = false, messsage = "请选择上传图片" });
                return null;
            }
            UploadPhone updatePhone = new UploadPhone(_hostingEnv);
            BusinessImage image;
            image = new BusinessImage();
            var activityTerm = await _activityTermExtension.GetAll().Include(x => x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
            if (file.Length > 10485760)
            {
                return null;
            }
            //保存文件
            image.UploadPath = updatePhone.PhoneNewUpload(file, "ActivityTerm");

            image.UploaderID = Guid.Parse(user.Value);
            image.RelevanceObjectID = activityTerm.ID;
            _businessImageRepository.AddAndSave(image);
            activityTerm.Avatar = image;
            _activityTermExtension.EditAndSave(activityTerm);
            return Json(true);

        }


        /// <summary>
        /// 删除活动图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteActivityDetailedImages(Guid id)
        {
            var entity = await _businessImageExtension.GetAll().Where(x => x.ID == id).FirstOrDefaultAsync();
            _businessImageExtension.DeleteAndSave(entity);
            return Json(new { isOk = true });
        }

        /// <summary>
        /// 禁止活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ProhibitActivityTrem(Guid id)
        {
            var entity = await _activityTermExtension.GetSingleAsyn(x => x.ID == id);
            if (entity == null)
            {
                return Json(new { isOK = false, message = "不存在该活动" });
            }
            if (!entity.IsDisable)
            {
                return Json(new { isOK = false, message = "该活动已被禁止" });
            }
            entity.IsDisable = false;
            _activityTermExtension.EditAndSave(entity);
            return Json(new { isOK = true, message = "成功禁用活动："+ entity.Name });
        }

        /// <summary>
        /// 解封活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> LiftedActivityTrem(Guid id)
        {
            var entity = await _activityTermExtension.GetSingleAsyn(x => x.ID == id);
            if (entity == null)
            {
                return Json(new { isOK = false, message = "不存在该活动" });
            }
            if (entity.IsDisable)
            {
                return Json(new { isOK = false, message = "该活动已被解封" });
            }
            entity.IsDisable = true;
            _activityTermExtension.EditAndSave(entity);
            return Json(new { isOK = true, message = "成功解封活动："+entity.Name });
        }

        /// <summary>
        /// 添加进首页呈现
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> HomeExhibitionPresent(Guid id)
        {
            var homeExhhibition = await _homeExhibitionExtension.GetAll().Include(x => x.Activity).Where(x => x.Activity.ID == id).FirstOrDefaultAsync();
            if (homeExhhibition != null)
            {
                return Json(new { isOK = false, message = homeExhhibition.Activity.Name + "已在首页显示设置里，请在首页显示设置模块设置允许显示" });
            }
            var activity = await _activityTermExtension.GetAll().Include(x=>x.Avatar).Where(x => x.ID == id).FirstOrDefaultAsync();
            homeExhhibition = new HomeExhibition()
            {
                Name = activity.Name,
                Activity = activity,
                Description = activity.Description,
                CreateDateTime=DateTime.Now,
                StartDateTime=DateTime.Now,
                EndDateTime= activity.EndDataTime,
                Avatar=activity.Avatar,
                ExhibitionLeve= ExhibitionLevelEnum.Highest
            };
            _homeExhibitionExtension.AddAndSave(homeExhhibition);
            return Json(new { isOK = true, message = activity.Name+"添加进首页显示成功，请在首页显示设置模块设置允许显示" });
        }
    }
}
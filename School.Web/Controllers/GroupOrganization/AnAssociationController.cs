using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Common.JsonModels;
using School.DataAccess;
using School.DataAccess.Common;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.GroupOrganization;
using School.ViewModels.GroupOrganization;
using School.Web.Common;
using Microsoft.EntityFrameworkCore;
using School.ViewModels.GroupOrganization.ActivityTerms;

namespace School.Web.Controllers.GroupOrganization
{
    public class AnAssociationController : Controller
    {
        private readonly IEntityRepository<AnAssociation> _anAssociationRepository;
        private readonly IDataExtension<AnAssociation> _anAssociationExtension;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IDataExtension<BusinessImage> _businessImageRepository;
        private readonly IDataExtension<AnAssociationAndUser> _anAssociationAndUserExtension;
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;
        public AnAssociationController(IEntityRepository<AnAssociation> repository,
            IDataExtension<AnAssociation> anAssociationExtension,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment hostingEnv,
            IDataExtension<BusinessImage> businessImageRepository,
            IDataExtension<AnAssociationAndUser> anAssociationAndUserExtension,
            IDataExtension<ActivityTerm> activityTermExtension)
        {
            _anAssociationRepository = repository;
            _anAssociationExtension = anAssociationExtension;
            _userManager = userManager;
            _hostingEnv = hostingEnv;
            _businessImageRepository = businessImageRepository;
            _anAssociationAndUserExtension = anAssociationAndUserExtension;
            _activityTermExtension = activityTermExtension;
        }

        /// <summary>
        /// 活动管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var anCollection = await _anAssociationExtension.GetAllIncludingAsyn(x => x.User);
            var anVmCollection = new List<AnAssociationVM>();
            foreach (var an in anCollection)
            {
                var anVM = new AnAssociationVM(an);
                anVmCollection.Add(anVM);
            }

            var pageSize = 10;
            var pageIndex = 1;
            var anAssociationList = anVmCollection.OrderBy(x => x.Name).FirstOrDefault();

            //处理分页

            var anAssociationCollectionPageList = IQueryableExtensions.ToPaginatedList(anVmCollection.AsQueryable<AnAssociationVM>(), pageIndex, pageSize);
            var anAssociationCollections = new List<AnAssociationVM>();
            foreach (var anAssociationCollection in anAssociationCollectionPageList)
            {
                anAssociationCollections.Add(anAssociationCollection);
            }

            //提取当前分页关联的分页器实例
            var pageGroup = PagenateGroupRepository.GetItem(anAssociationCollectionPageList, 5, pageIndex);
            ViewBag.PageGroup = pageGroup;

            var listPageParameter = new ListPageParameter()
            {
                PageIndex = anAssociationCollectionPageList.PageIndex,
                Keyword = "",
                PageSize = anAssociationCollectionPageList.PageSize,
                ObjectTypeID = "",
                ObjectAmount = anAssociationCollectionPageList.TotalCount,
                SortDesc = "Default",
                SortProperty = "UserName",
                PageAmount = 0,
                SelectedObjectID = ""
            };
            ViewBag.PageParameter = listPageParameter;
            return View("~/Views/GroupOrganization/AnAssociation/Index.cshtml", anAssociationCollections);

        }

        /// <summary>
        /// 根据关键词检索活动数据集合，返回给前端页面
        /// </summary>
        /// <param name="keywork"></param>
        public async Task<IActionResult> List(string keywork, string listPageParaJson)
        {
            var listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            var anVMCollection = new List<AnAssociationVM>();
            if (!String.IsNullOrEmpty(keywork) && keywork != "undefined")
            {
                Expression<Func<AnAssociation, bool>> condtion = x =>
                   x.Name.Contains(keywork) ||
                   x.SchoolAddress.Contains(keywork);
                var anAssociationCollection = await _anAssociationExtension.GetAll().Include(x => x.User).Where(condtion).ToListAsync();
                foreach (var an in anAssociationCollection)
                {
                    anVMCollection.Add(new AnAssociationVM(an));
                }
            }
            else
            {
                var anAssociationCollection = await _anAssociationExtension.GetAllIncludingAsyn(x => x.User);
                foreach (var an in anAssociationCollection)
                {
                    anVMCollection.Add(new AnAssociationVM(an));
                }
            }
            var anAssociationPageList = IQueryableExtensions.ToPaginatedList(anVMCollection.AsQueryable(), listPagePara.PageIndex, listPagePara.PageSize);

            var associationCollections = new List<AnAssociationVM>();

            foreach (var anAssociationPage in anAssociationPageList)
            {
                associationCollections.Add(anAssociationPage);
            }

            var pageGroup = PagenateGroupRepository.GetItem(anAssociationPageList, 5, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/GroupOrganization/AnAssociation/_List.cshtml", associationCollections);
        }

        /// <summary>
        /// 用于存储或更新活动
        /// </summary>
        /// <param name="anVM"></param>
        /// <returns></returns>
        public async Task<IActionResult> Sava([Bind("ID,UserId,Name,SchoolAddress,CreateDataTime,MaxNumber,Description")]AnAssociationVM anVM)
        {

            var an = await _anAssociationRepository.GetSingleAsyn(anVM.ID);
            if (an == null)
            {
                an = new AnAssociation();
                var userclaims = User.Claims.FirstOrDefault();
                var userData = await _userManager.FindByIdAsync(userclaims.Value.ToString());
                an.User = userData;
                var anFounder = new AnAssociationAndUser()
                {
                    User = userData,
                    AnAssociationId = anVM.ID,
                    AnJurisdictionManager = AnJurisdiction.Founder
                };
                an.IsDisable = true;
                _anAssociationAndUserExtension.Add(anFounder);
            }
            anVM.MapToAn(an);
            var savaStatus = await _anAssociationRepository.AddOrEditAndSaveAsyn(an);
            if (an == null)
            {
                _anAssociationAndUserExtension.Save();
            }
            if (savaStatus)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "数据保存出现异常，无法创建活动。");
                return View("~/Views/GroupOrganization/AnAssociation/CreateOrEdit.cshtml", anVM);
            }
        }

        /// <summary>
        /// 增或者编辑人员数据的处理
        /// </summary>
        /// <param name="id">活动对象的ID属性值，如果这个值在系统中找不到具体的对象，则看成是新建对象。</param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var an = await _anAssociationRepository.GetSingleAsyn(id, x => x.User);
            if (an == null)
            {
                an = new AnAssociation();
            }
            var anVM = new AnAssociationVM(an);
            return View("~/Views/GroupOrganization/AnAssociation/CreateOrEdit.cshtml", anVM);
        }

        /// <summary>
        /// 根据ID查询对应的活动信息，
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(Guid id)
        {
            var anAssociation = await _anAssociationRepository.GetSingleAsyn(id, x => x.User);
            var anVM = new AnAssociationVM(anAssociation);
            //获取图片
            anVM.Images = await _businessImageRepository.GetAll().Where(x => x.RelevanceObjectID == id).ToListAsync();
            //获取社团管理员的数量和用户的数量
            var anUser = _anAssociationAndUserExtension.GetAll().OrderByDescending(x => x.AnJurisdictionManager).Include(x => x.User).Include(x => x.User.Avatar).Where(x => x.AnAssociationId == id);
            ViewBag.AnManagerNumber = (await anUser.Where(x => x.AnJurisdictionManager == AnJurisdiction.Founder || x.AnJurisdictionManager == AnJurisdiction.Admin).ToListAsync()).Count();
            ViewBag.UserNumber = anUser.Count();
            anVM.Members = anUser.ToList();
            //获取社团活动
            var activity = await _activityTermExtension.GetAll().Include(x => x.AnAssociation).Where(x => x.AnAssociation.ID == id).ToListAsync();
            //anVM.Activitys = activity;
            ViewBag.ActivityNumber = activity.Count();

            return PartialView("~/Views/GroupOrganization/AnAssociation/_Detail.cshtml", anVM);
        }

        /// <summary>
        /// 社团活动
        /// </summary>
        /// <param name="keywork"></param>
        /// <param name="listPageParaJson"></param>
        /// <returns></returns>
        public async Task<IActionResult> ActivityList(Guid id, string listPageParaJson)
        {
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null)
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }
            var atVMCollection = new List<ActivityTermVM>();
            var activityTermCollection = await _activityTermExtension.GetAll().Include(x => x.User).Include(x => x.AnAssociation).Include(x => x.Avatar).Where(x => x.AnAssociation.ID == id).ToListAsync();
            var activityTermPageList = IQueryableExtensions.ToPaginatedList(activityTermCollection.AsQueryable(), listPagePara.PageIndex, 10);
            var activityCollections = new List<ActivityTermVM>();
            foreach (var activityTermPage in activityTermPageList)
            {
                activityCollections.Add(new ActivityTermVM(activityTermPage));
            }
            var pageGroup = PagenateGroupRepository.GetItem(activityTermPageList, 5, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/GroupOrganization/AnAssociation/ActivityList.cshtml", activityCollections);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _anAssociationRepository.DeleteAndSaveAsyn(id);
            if (!status.IsOK)
            {
                status.Message = "有其它关联数据，删除失败";
            }
            return Json(status);
        }

        public IActionResult UploadPicture(Guid id)
        {
            ViewBag.Id = id;
            return View("~/Views/GroupOrganization/AnAssociation/UploadPicture.cshtml");
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
            //if (file == null)
            //{
            //    return null;
            //    //return Json(new { isOK = false, messsage = "请选择上传图片" });
            //}
            UploadPhone updatePhone = new UploadPhone(_hostingEnv);
            BusinessImage image;
            image = new BusinessImage();
            var anAssociation = await _anAssociationExtension.GetAll().Include(x => x.User).Where(x => x.ID == id).FirstOrDefaultAsync();
            if (file.Length > 10485760)
            {
                return null;
                //return Json(new { isOK = false, messsage = "图片大小有误,每张图片限制于10M" });
            }
            //保存文件
            image.UploadPath = updatePhone.PhoneNewUpload(file, "AnAssociation");

            image.UploaderID = Guid.Parse(user.Value);
            image.RelevanceObjectID = anAssociation.ID;
            _businessImageRepository.AddAndSave(image);
            anAssociation.Avatar = image;
            _anAssociationExtension.EditAndSave(anAssociation);
            return Json(true);

        }

        /// <summary>
        /// 禁用社团
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ProhibitAnAssociation(Guid id)
        {
            var entity = await _anAssociationExtension.GetSingleAsyn(x => x.ID == id);
            if (entity == null)
            {
                return Json(new { isOK = false, meesage = "不存在该社团" });
            }
            if (!entity.IsDisable)
            {
                return Json(new { isOK = false, meesage = "该社团已被禁用" });
            }
            entity.IsDisable = false;
            _anAssociationExtension.EditAndSave(entity);
            return Json(new { isOK = true, meesage = "成功禁用社团：" + entity.Name });
        }

        /// <summary>
        /// 解封社团
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> LiftedAnAssociation(Guid id)
        {
            var entity = await _anAssociationExtension.GetSingleAsyn(x => x.ID == id);
            if (entity == null)
            {
                return Json(new { isOK = false, meesage = "不存在该社团" });
            }
            if (entity.IsDisable)
            {
                return Json(new { isOK = false, meesage = "该社团已被禁用" });
            }
            entity.IsDisable = true;
            _anAssociationExtension.EditAndSave(entity);
            return Json(new { isOK = true, meesage = "成功解封社团："+entity.Name });
        }
    }
}
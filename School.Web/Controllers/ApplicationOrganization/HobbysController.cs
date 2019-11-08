using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Common.JsonModels;
using School.DataAccess.Common;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.ViewModels.ApplicationOrganization;
using School.Web.Common;

namespace School.Web.Controllers.ApplicationOrganization
{
    public class HobbysController : Controller
    {

        private readonly IDataExtension<Hobby> _hobbyExtension;
        private readonly IDataExtension<BusinessImage> _businessImageExtension;
        private readonly IHostingEnvironment _hostingEnv;
        public HobbysController(IDataExtension<Hobby> hobbyExtension,
            IDataExtension<BusinessImage> businessImageExtension,
            IHostingEnvironment hostingEnv)
        {
            _hobbyExtension = hobbyExtension;
            _businessImageExtension = businessImageExtension;
            _hostingEnv = hostingEnv;
        }
        public async Task<IActionResult> Index()
        {
            var boCollection = await _hobbyExtension.GetAll().Include(x => x.Avatar).OrderBy(x=>x.SortCode).ToListAsync();
            var boVMCollection = new List<HobbyVM>();
            foreach (var bo in boCollection)
            {
                var boVM = new HobbyVM(bo);
                boVMCollection.Add(boVM);
            }
            var pageSize = 10;
            var pageIndex = 1;
            var personList = boVMCollection.OrderBy(x => x.Name).FirstOrDefault();
            // 处理分页
            var hobbyCollectionPageList = IQueryableExtensions.ToPaginatedList(boVMCollection.AsQueryable<HobbyVM>(), pageIndex, pageSize);
            var hobbyCollections = new List<HobbyVM>();
            foreach (var hobbyCollection in hobbyCollectionPageList)
            {
                hobbyCollections.Add(hobbyCollection);
            }

            //提取当前分页关联的分页器实例
            var pageGroup = PagenateGroupRepository.GetItem<HobbyVM>(hobbyCollectionPageList, 5, pageIndex);
            ViewBag.PageGroup = pageGroup;

            var listPageParameter = new ListPageParameter()
            {
                PageIndex = hobbyCollectionPageList.PageIndex,
                Keyword = "",
                PageSize = hobbyCollectionPageList.PageSize,
                ObjectTypeID = "",
                ObjectAmount = hobbyCollectionPageList.TotalCount,
                SortDesc = "Default",
                SortProperty = "Name",
                PageAmount = 0,
                SelectedObjectID = ""
            };
            ViewBag.PageParameter = listPageParameter;
            return View("~/Views/ApplicationOrganization/Hobbys/Index.cshtml", hobbyCollections);
        }

        /// <summary>
        /// 根据关键词检索爱好数据集合，返回给前端页面
        /// </summary>
        /// <param name="keywork"></param>
        /// <returns></returns>
        public async Task<IActionResult> List(string keywork, string listPageParaJson)
        {
            var listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            var boVMCollection = new List<HobbyVM>();
            if (!String.IsNullOrEmpty(keywork))
            {
                Expression<Func<Hobby, bool>> condtion = x =>//Contains(参数字符串是否包含于string对象中)
                   x.Name.Contains(keywork) ||
                   x.Description.Contains(keywork);
                var hobbyCollection = await _hobbyExtension.GetAll().Include(x=>x.Avatar).OrderBy(x=>x.SortCode).Where(condtion).ToListAsync();
                foreach (var bo in hobbyCollection)
                {
                    boVMCollection.Add(new HobbyVM(bo));
                }
            }
            else
            {
                var hobbyCollection = await _hobbyExtension.GetAll().Include(x => x.Avatar).OrderBy(x => x.SortCode).ToListAsync();
                foreach (var bo in hobbyCollection)
                {
                    boVMCollection.Add(new HobbyVM(bo));
                }
            }

            var hobbyCollectionPageList = IQueryableExtensions.ToPaginatedList(boVMCollection.AsQueryable<HobbyVM>(), listPagePara.PageIndex, listPagePara.PageSize);

            var hobbyCollections = new List<HobbyVM>();
            foreach (var hoCollection in hobbyCollectionPageList)
            {
                hobbyCollections.Add(hoCollection);
            }
            var pageGroup = PagenateGroupRepository.GetItem<HobbyVM>(hobbyCollectionPageList, 5, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/ApplicationOrganization/Hobbys/_List.cshtml", hobbyCollections);

        }

        /// <summary>
        /// 保存爱好数据
        /// </summary>
        /// <param name="perVM"></param>
        /// <returns></returns>
        public async Task<IActionResult> Save([Bind("ID,Name,Description,SortCode")]HobbyVM hobbyVM)
        {

            if (hobbyVM.Name == "")
            {
                ModelState.AddModelError("", "爱好名称为空，无法添加");//添加指定的错误信息
            }
            var hobby = await _hobbyExtension.GetSingleAsyn(x => x.ID == hobbyVM.ID);
            if (hobby == null)
            {
                hobby = new Hobby();
                hobbyVM.MapToHb(hobby);
                _hobbyExtension.AddAndSave(hobby);
            }
            else
            {
                hobbyVM.MapToHb(hobby);
                _hobbyExtension.EditAndSave(hobby);
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// 增或者编辑爱好数据的处理
        /// </summary>
        /// <param name="id">数据的ID属性值，如果这个值在系统中找不到具体的对象，则看成是新建对象。</param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var hobby = await _hobbyExtension.GetSingleAsyn(x => x.ID == id);
            if (hobby == null)
            {
                hobby = new Hobby();
                hobby.Name = "";
                hobby.Description = "";
                hobby.SortCode = "";
            }
            var hobbyVM = new HobbyVM(hobby);
            return View("~/Views/ApplicationOrganization/Hobbys/CreateOrEdit.cshtml", hobbyVM);

        }

        /// <summary>
        ///删除爱好
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _hobbyExtension.GetSingleAsyn(x => x.ID == id);
            _hobbyExtension.DeleteAndSave(user);
            return Json(true);
        }

        /// <summary>
        /// 更改头像
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> hobbyPhone(Guid id)
        {
            var hobby = await _hobbyExtension.GetAll().Include(x=>x.Avatar).Where(x => x.ID == id).FirstOrDefaultAsync();
            var hobbyVM = new HobbyVM(hobby);
            return View("~/Views/ApplicationOrganization/Hobbys/HobbyPhoneEdit.cshtml", hobbyVM);

        }

        /// <summary>
        /// 更改头像
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SavehobbyPhone(string id)
        {
            UploadPhone updatePhone = new UploadPhone(_hostingEnv);
            BusinessImage image = new BusinessImage();
            var file = Request.Form.Files.FirstOrDefault();
            var hobby = await _hobbyExtension.GetAll().Where(x=>x.ID==Guid.Parse(id)).FirstOrDefaultAsync();

            //保存文件
            image.UploadPath = updatePhone.PhoneNewUpload(file, "User");

            image.UploaderID = hobby.ID;
            image.RelevanceObjectID = hobby.ID;
            _businessImageExtension.AddAndSave(image);
            hobby.Avatar = image;
            _hobbyExtension.EditAndSave(hobby);
            return Json(true);
        }
    }
}
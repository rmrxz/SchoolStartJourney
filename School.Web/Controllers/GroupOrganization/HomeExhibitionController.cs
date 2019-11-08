using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Common.JsonModels;
using School.DataAccess.Common;
using School.DataAccess.SqlServer;
using School.Entities.GroupOrganization;
using School.ViewModels.GroupOrganization.HomeExhibitions;

namespace School.Web.Controllers.GroupOrganization
{
    public class HomeExhibitionController : Controller
    {
        private readonly IDataExtension<HomeExhibition> _homeExhibitionRepository;
        public HomeExhibitionController(IDataExtension<HomeExhibition> homeExhibitionRepository)
        {
            this._homeExhibitionRepository = homeExhibitionRepository;
        }

        public IActionResult Index()
        {
            return View("~/Views/GroupOrganization/HomeExhibition/Index.cshtml");
        }

        public async Task<IActionResult> List(string keywork, string listPageParaJson)
        {
            var listPagePara = new ListPageParameter();
            if (listPageParaJson != null && listPageParaJson != "")
            {
                listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            }
            var heVMCollection = new List<HomeExhibitionVM>();
            var homeExhibitionCollection = new List<HomeExhibition>();
            if (!String.IsNullOrEmpty(keywork))
            {
                Expression<Func<HomeExhibition, bool>> condtion = x =>
                  x.Name.Contains(keywork) ||
                  x.Activity.Name.Contains(keywork) ||
                  x.Activity.User.Name.Contains(keywork);
                homeExhibitionCollection = await _homeExhibitionRepository.GetAll().Include(x => x.Activity).Include(x => x.Activity.User).
                    Include(x => x.Avatar).Where(condtion).OrderBy(x => x.CreateDateTime).ToListAsync();
            }
            else
            {
                homeExhibitionCollection = await _homeExhibitionRepository.GetAll().Include(x => x.Activity).Include(x => x.Activity.User).
                    Include(x => x.Avatar).OrderBy(x => x.CreateDateTime).ToListAsync();
            }

            var homeExhibitionPageList = IQueryableExtensions.ToPaginatedList(homeExhibitionCollection.AsQueryable(), listPagePara.PageIndex, listPagePara.PageSize);

            foreach (var item in homeExhibitionPageList)
            {
                heVMCollection.Add(new HomeExhibitionVM(item));
            }
            var pageGroup = PagenateGroupRepository.GetItem(homeExhibitionPageList, 5, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("~/Views/GroupOrganization/HomeExhibition/_List.cshtml", heVMCollection);
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> StartUp(Guid id)
        {
            var entity = await _homeExhibitionRepository.GetAll().Where(x => x.ID == id).FirstOrDefaultAsync();
            if (entity.IsUse)
            {
                return Json(new { isOK = false, message = "已是启用状态" });
            }
            var homeExhibitions = await _homeExhibitionRepository.GetAll().Where(x => x.IsUse).ToListAsync();
            if (homeExhibitions.Count >= 4)
            {
                return Json(new { isOK = false, message = "呈现数据数量已超过最大数量，请禁用一些数据再启用" });
            }
            entity.IsUse = true;
            _homeExhibitionRepository.EditAndSave(entity);
            return Json(new { isOK = true, message = "启用成功" });
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Disable(Guid id)
        {
            var entity = await _homeExhibitionRepository.GetAll().Where(x => x.ID == id).FirstOrDefaultAsync();
            if (!entity.IsUse)
            {
                return Json(new { isOK = false, message = "已是禁用状态" });
            }
            entity.IsUse = false;
            _homeExhibitionRepository.EditAndSave(entity);
            return Json(new { isOK = true, message = "禁用成功" });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _homeExhibitionRepository.GetAll().Where(x => x.ID == id).FirstOrDefaultAsync();
            _homeExhibitionRepository.DeleteAndSave(entity);
            return Json(new { isOK = true, message = "删除成功" });
        }
    }
}
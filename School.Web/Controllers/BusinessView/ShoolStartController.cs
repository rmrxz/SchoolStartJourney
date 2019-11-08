using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.DataAccess.SqlServer;
using School.Entities.GroupOrganization;
using School.ViewModels.GroupOrganization;
using School.ViewModels.GroupOrganization.ActivityTerms;
using School.ViewModels.GroupOrganization.HomeExhibitions;
using School.Web.Common;

namespace School.Web.Controllers.BusinessView
{
    public class ShoolStartController : Controller
    {
        private readonly int pageSize = 6;

        private readonly IDataExtension<AnAssociation> _anassociationExtension;
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;
        private readonly IDataExtension<HomeExhibition> _homeExhibitionRepository;
        private readonly IDataExtension<AnAssociationAndUser> _anassociationAndUserExtension;

        public ShoolStartController(IDataExtension<AnAssociation> anassociationExtension, IDataExtension<AnAssociationAndUser> anassociationAndUserExtension,
            IDataExtension<ActivityTerm> activityTermExtension,
            IDataExtension<HomeExhibition> homeExhibitionRepository)
        {
            this._anassociationExtension = anassociationExtension;
            this._anassociationAndUserExtension = anassociationAndUserExtension;
            this._activityTermExtension = activityTermExtension;
            this._homeExhibitionRepository = homeExhibitionRepository;
        }
        public async Task<IActionResult> Index()
        {
            var activityTremList = await _activityTermExtension.GetAll().OrderBy(x => x.CreateDateTime).Include(x=>x.Avatar).Where(x=>x.AnAssociation!=null).ToListAsync();
            activityTremList = activityTremList.Skip(0).Take(6).ToList();
            var activityVMList = new List<ActivityTermVM>();
            foreach (var item in activityTremList)
            {
                activityVMList.Add(new ActivityTermVM(item));
            }
            var activityPerList = await _activityTermExtension.GetAll().OrderBy(x => x.CreateDateTime).Include(x=>x.Avatar).Where(x => x.AnAssociation == null).ToListAsync();
            activityPerList = activityPerList.Skip(0).Take(3).ToList(); ;
            var activityVMPerList = new List<ActivityTermVM>();
            foreach (var item in activityPerList)
            {
                activityVMPerList.Add(new ActivityTermVM(item));
            }
            var sowingMaps = await _homeExhibitionRepository.GetAll().OrderBy(x => x.CreateDateTime).Include(x=>x.Avatar).Where(x=>x.IsUse).ToListAsync();
            sowingMaps = sowingMaps.Skip(0).Take(4).ToList();
            //社团活动
            ViewBag.activityVMList = activityVMList;
            //个人活动
            ViewBag.activityVMPerList = activityVMPerList;
            //轮播图
            var sowingMapList = new List<HomeExhibitionVM>();
            foreach (var item in sowingMaps)
            {
                sowingMapList.Add(new HomeExhibitionVM(item));
            }
            ViewBag.sowingMapList = sowingMapList;
            return PartialView("~/Views/BusinessView/ShoolStart/Index.cshtml");
        }

        public IActionResult Phone()
        {

            return View("~/Views/BusinessView/ShoolStart/Phone.cshtml");
        }

        //public IActionResult Personal()
        //{
        //    return View("../../Views/BusinessView/ShoolStart/Personal");
        //}

        public IActionResult InformationSetUp()
        {
            return View("~/Views/BusinessView/ShoolStart/InformationSetUp.cshtml");
        }

        public async Task<IActionResult> SingAndExchange(int index)
        {
            var datas = await _anassociationExtension.GetAll().Include(x => x.Avatar).ToListAsync();

            datas = datas.Skip(index * pageSize).Take(pageSize).ToList();
            var flag = false;
            var anAssociations = new List<AnAssociationVM>();
            if (datas.Count() == 0)
            {
                datas = (await _anassociationExtension.GetAllAsyn()).Take(pageSize).ToList();
                flag = true;
            }
            foreach (var item in datas)
            {
                var anVm = new AnAssociationVM(item) { AnAssociationNum = (await _anassociationAndUserExtension.GetAll().Where(x => x.AnAssociationId == item.ID).ToListAsync()).Count };
                anAssociations.Add(anVm);
            }
            return Json(new { Items = anAssociations, ifLastIndex = flag });
        }

    }
}
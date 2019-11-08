using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.Common.JsonModels;
using School.DataAccess;
using School.DataAccess.Common;
using School.DataAccess.SqlServer;
using School.Entities.BusinessOrganization;
using School.ViewModels.BusinessOrganization;

namespace School.Web.Controllers.BusinessOrganization
{
    public class PersonController : Controller
    {
        private readonly IEntityRepository<Person> _PersonRepository;
        private readonly IDataExtension<Person> _PersonExtension;

        public PersonController(IEntityRepository<Person> repository, IDataExtension<Person> personExtension)
        {
            _PersonRepository = repository;
            _PersonExtension = personExtension;
        }

        /// <summary>
        /// 数据管理的入口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var boCollection = await _PersonExtension.GetAllAsyn();
            var boVMCollection = new List<PersonVM>();
            foreach (var bo in boCollection)
            {
                var boVM = new PersonVM(bo);
                boVMCollection.Add(boVM);
            }
            var pageSize = 10;
            var pageIndex = 1;
            var personList = boVMCollection.OrderBy(x => x.Name).FirstOrDefault();
            // 处理分页
            var personCollectionPageList = IQueryableExtensions.ToPaginatedList(boVMCollection.AsQueryable<PersonVM>(), pageIndex, pageSize);
            var personCollections = new List<PersonVM>();
            foreach (var personCollection in personCollectionPageList)
            {
                personCollections.Add(personCollection);
            }

            //提取当前分页关联的分页器实例
            var pageGroup = PagenateGroupRepository.GetItem<PersonVM>(personCollectionPageList, 3, pageIndex);
            ViewBag.PageGroup = pageGroup;

            var listPageParameter = new ListPageParameter()
            {
                PageIndex = personCollectionPageList.PageIndex,
                Keyword = "",
                PageSize = personCollectionPageList.PageSize,
                ObjectTypeID ="",
                ObjectAmount = personCollectionPageList.TotalCount,
                SortDesc = "Default",
                SortProperty = "UserName",
                PageAmount = 0,
                SelectedObjectID = ""
            };
            ViewBag.PageParameter = listPageParameter;
            return View("~/Views/BusinessOrganization/Person/Index.cshtml", personCollections);
        }

        /// <summary>
        /// 根据关键词检索人员数据集合，返回给前端页面
        /// </summary>
        /// <param name="keywork"></param>
        /// <returns></returns>
        public async Task<IActionResult> List(string keywork, string listPageParaJson)
        {
            var listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);
            var boVMCollection = new List<PersonVM>();
            if (!String.IsNullOrEmpty(keywork))
            {
                Expression<Func<Person, bool>> condtion = x =>//Contains(参数字符串是否包含于string对象中)
                   x.Name.Contains(keywork) ||
                   x.FixedTelephone.Contains(keywork);
                var personCollection = await _PersonExtension.FindByAsyn(condtion);
                foreach (var bo in personCollection)
                {
                    boVMCollection.Add(new PersonVM(bo));
                }
            }
            else
            {
                var personCollection = await _PersonExtension.GetAllAsyn();
                foreach (var bo in personCollection)
                {
                    boVMCollection.Add(new PersonVM(bo));
                }
            }

            var personCollectionPageList = IQueryableExtensions.ToPaginatedList(boVMCollection.AsQueryable<PersonVM>(), listPagePara.PageIndex, listPagePara.PageSize);

            var personCollections = new List<PersonVM>();
            foreach (var personCollection in personCollectionPageList)
            {
                personCollections.Add(personCollection);
            }
            var pageGroup = PagenateGroupRepository.GetItem<PersonVM>(personCollectionPageList, 3, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            return View("../../Views/BusinessOrganization/Person/_List", personCollections);

        }


        /// <summary>
        /// 保存人员数据
        /// </summary>
        /// <param name="perVM"></param>
        /// <returns></returns>
        public async Task<IActionResult> Save([Bind("ID,IsNew,Name,Email,FixedTelephone,Description,SortCode")]PersonVM perVM)
        {
            var hasDuplicateNamePerson = await _PersonExtension.HasInstanceAsyn(x => x.Name == perVM.Name);
            if (hasDuplicateNamePerson && perVM.IsNew)//判断是否已存在该人员
            {
                ModelState.AddModelError("", "人员名重复，无法添加");//添加指定的错误信息
                return View("../../Views/BusinessOrganization/Person/CreateOrEdit", perVM);
            }
            var bo = new Person();
            if (!perVM.IsNew)
            {
                bo = await _PersonRepository.GetSingleAsyn(perVM.ID);
            }
            perVM.MapToBo(bo);
            var saveStatus = await _PersonRepository.AddOrEditAndSaveAsyn(bo);
            if (saveStatus)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "数据保存出现异常，无法处理，请联系开发部人员。");
                return View("../../Views/BusinessOrganization/Person/CreateOrEdit", perVM);
            }
        }


        /// <summary>
        /// 增或者编辑人员数据的处理
        /// </summary>
        /// <param name="id">人员对象的ID属性值，如果这个值在系统中找不到具体的对象，则看成是新建对象。</param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(Guid id)
        {
            var isNew = false;
            var bo = await _PersonRepository.GetSingleAsyn(id);
            if (bo == null)
            {
                bo = new Person();
                bo.Name = "";
                bo.Description = "";
                bo.SortCode = "";
                isNew = true;
            }
            var personVM = new PersonVM(bo);
            personVM.IsNew = isNew;
            return View("~/Views/BusinessOrganization/Person/CreateOrEdit.cshtml", personVM);

        }

        /// <summary>
        /// 以局部页的方式的方式，构建明细数据的处理
        /// </summary>
        /// <param name="id">人员对象的ID属性值</param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(Guid id)
        {
            var person = await _PersonRepository.GetSingleAsyn(id);
            var personVM = new PersonVM(person);
            return PartialView("~/Views/BusinessOrganization/Person/_Detail.cshtml", personVM);

        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _PersonRepository.DeleteAndSaveAsyn(id);
            return Json(status);
        }
    }
}
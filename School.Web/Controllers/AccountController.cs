using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using School.Common.LogonDataOperation;
using School.Common.ViewModelComponents;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.GroupOrganization;
using School.ViewModels.ApplicationOrganization;
using School.Web.Common;

namespace School.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataExtension<ApplicationUser> _userExtension;
        private readonly IDataExtension<BusinessImage> _businessImageExtension;
        private readonly IDataExtension<UserFriend> _userFriendExtension;
        public AccountController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IDataExtension<ApplicationUser> userExtension,
            IDataExtension<BusinessImage> businessImageExtension,
            IDataExtension<UserFriend> userFriendExtension)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._userExtension = userExtension;
            this._businessImageExtension = businessImageExtension;
            this._userFriendExtension = userFriendExtension;
        }

        /// <summary>
        /// 登录注册界面
        /// </summary>
        /// <returns></returns>
        public IActionResult LoginRegister(string path)
        {
            ViewBag.isPath = true;
            if (path == "Register")
            {
                ViewBag.isPath = false;
            }
            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="jsonLogonInformation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string jsonLogonInformation)
        {
            var logonVM = Newtonsoft.Json.JsonConvert.DeserializeObject<LogonInformation>(jsonLogonInformation);
            var user = await _userManager.FindByNameAsync(logonVM.UserName);
            if (User == null)
            {
                return Json(new { result = false, message = "不存在该用户" });
            }
            if (!user.TwoFactorEnabled)
            {
                return Json(new { result = false, message = "该用户已被禁用，请联系管理员解封" });
            }
            //var user = await _userExtension.GetSingleAsyn(x => x.UserName == logonVM.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(logonVM.UserName, logonVM.Password, true, lockoutOnFailure: true);
                if (result.Succeeded)//验证成功
                {
                    var returnUrl = Url.Action("Index", "ShoolStart");
                    return Json(new { result = true, message = returnUrl });
                }

                else
                {
                    return Json(new { result = false, message = "用户密码错误，请检查后重新处理。" });
                }
            }
            return Json(new { result = false, message = "无法执行登录操作，请仔细检查后重新处理。" });
        }

        /// <summary>
        /// 普通读者用户资料注册管理
        /// </summary>
        /// <param name="jsonRegisetrInformation"></param>
        /// <returns></returns>
        public async Task<IActionResult> Register(string jsonRegisterInformation)
        {
            var validateMessage = new ValidateMessage();
            var loUser = Newtonsoft.Json.JsonConvert.DeserializeObject<LogonInformation>(jsonRegisterInformation);//获取前端数据放到用户视图模型里边
            if (loUser.ConfirmPassword != loUser.Password)
            {
                validateMessage.IsOK = false;
                validateMessage.ValidateMessageItems.Add(
                     new ValidateMessageItem
                     {
                         IsPropertyName = false,
                         MessageName = "Succeed",
                         Message = "密码和确认密码不一致",
                     });
                return Json(validateMessage);
            }
            if (ModelState.IsValid)
            {
                var isNewUser = await _userManager.FindByNameAsync(loUser.UserName);
                if (isNewUser != null)
                {
                    validateMessage.IsOK = false;
                    validateMessage.ValidateMessageItems.Add(
                         new ValidateMessageItem
                         {
                             IsPropertyName = false,
                             MessageName = "Succeed",
                             Message = "登录名重复",
                         });
                    return Json(validateMessage);
                }
                var user = new ApplicationUser()
                {
                    Name = loUser.UserName,
                    UserName = loUser.UserName,
                    TwoFactorEnabled = true,
                    Email = loUser.Email,
                    NormalizedUserName = loUser.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Avatar = await _businessImageExtension.GetAll().Where(x => x.Name == "头像" && x.IsSystem).FirstOrDefaultAsync(),
                    RegisterTime = DateTime.Now,
                    Power= AnJurisdiction.Ordinary,

                };
                _userExtension.Add(user);
                user.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, loUser.Password);
                await _userExtension.SaveAsyn();

                validateMessage.IsOK = true;
                validateMessage.ValidateMessageItems.Add(
                       new ValidateMessageItem
                       {
                           IsPropertyName = false,
                           MessageName = "Succeed",
                           Message = "注册成功，请登录"
                       });

                return Json(validateMessage);
            }
            else
            {
                validateMessage.IsOK = false;
                var errCollection = from errKey in ModelState.Keys
                                    from errMessage in ModelState[errKey].Errors
                                    where ModelState[errKey].Errors.Count > 0
                                    select (new { errKey, errMessage.ErrorMessage });

                foreach (var errItem in errCollection)
                {
                    var vmItem = new ValidateMessageItem()
                    {
                        IsPropertyName = true,
                        MessageName = errItem.errKey,
                        Message = errItem.ErrorMessage
                    };
                    validateMessage.ValidateMessageItems.Add(vmItem);
                }
                return Json(validateMessage);
            }
        }

        ///// <summary>
        ///// 登录后台界面
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IActionResult> BackstageLogin()
        //{

        //}

        /// <summary>
        /// 退出登录状态
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Cancellation()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "ShoolStart");
        }


        /// <summary>
        /// 添加用户x => x.UserID == nowUser.Value &&
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var nowUser = User.Claims.FirstOrDefault();
            if (nowUser == null)
            {
                return Json(new { isOK = false, message = "您未登录，请登录后再执行该操作" });
            }
            if (nowUser.Value == id.ToString())
            {
                return Json(new { isOK = false, message = "不可添加自己为好友" });
            }
            if (user == null)
            {
                return Json(new { isOK = false, message = "好友添加失败，不存在这个用户" });
            }
            var entity = await _userFriendExtension.GetAll().Include(x => x.Friend).Where(x => x.Friend.Id == id.ToString() && x.UserID.ToString() == nowUser.Value).FirstOrDefaultAsync();
            if (entity != null)
            {
                return Json(new { isOK = false, message = "你的好友列表里已存在该用户，不可重复添加" });
            }
            var userFriend = new UserFriend()
            {
                UserID = nowUser.Value,
                Friend = user
            };
            _userFriendExtension.AddAndSave(userFriend);
            return Json(new { isOK = true, message = "添加成功" });
        }

    }
}
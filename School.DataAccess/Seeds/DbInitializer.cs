using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.BusinessOrganization;
using School.Entities.GroupOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace School.DataAccess.Seeds
{
    //种子数据在这里添加
    /// <summary>
    /// 构建一个初始化原始数据的组件，用于程序启动的时候执行一些数据初始化的操作
    /// </summary>
    public static class DbInitializer
    {
        static EntityDbContext _Context;
        /// <summary>
        /// 人员默认头像
        /// </summary>
        public static BusinessImage imagesHead { get; set; }

        /// <summary>
        /// 其它业务默认图片
        /// </summary>
        public static BusinessImage imagesDefault { get; set; }

        public static void Initialize(EntityDbContext context)
        {
            _Context = context;
            context.Database.EnsureCreated();//创建数据库，如果创建了，则不再创建
            _Images();
            _PersonAndDepartment();
            _UserAndBusiness();

        }

        public static void _Images()
        {
            #region 添加图片
            if (_Context.BusinessImages.Any())
                return;
            imagesHead = new BusinessImage() { Name = "头像", Description = "", DisplayName = "人员默认头像", UploadPath = "/images/Common/Default/head_portrait.gif", IsSystem = true };
            imagesDefault = new BusinessImage() { Name = "默认图片", Description = "", DisplayName = "业务数据默认图片", UploadPath = "/images/Common/Default/no_image.png", IsSystem = true };
            var imgs = new List<BusinessImage>() { imagesHead, imagesDefault };
            foreach (var item in imgs)
            {
                _Context.BusinessImages.Add(item);
            }
            _Context.SaveChanges();
            #endregion
        }

        /// <summary>
        /// 人员和部门
        /// </summary>
        public static void _PersonAndDepartment()
        {

            if (_Context.Departments.Any())
                return;
            var dept01 = new Department() { Name = "总经办", Description = "", SortCode = "01" };
            var dept02 = new Department() { Name = "综合管理办公室", Description = "", SortCode = "02" };
            var dept03 = new Department() { Name = "开发部", Description = "", SortCode = "03" };
            var dept04 = new Department() { Name = "营运部", Description = "", SortCode = "04" };
            var dept0401 = new Department() { Name = "客户响应服务组", Description = "", SortCode = "0401" };
            var dept0402 = new Department() { Name = "客户需求分析组", Description = "", SortCode = "0402" };
            var dept0403 = new Department() { Name = "应用设计开发组", Description = "", SortCode = "0403" };
            var dept05 = new Department() { Name = "市场部", Description = "", SortCode = "05" };
            var dept06 = new Department() { Name = "品管部", Description = "", SortCode = "06" };
            var dept0601 = new Department() { Name = "营运部驻场服务组", Description = "", SortCode = "0601" };
            var dept0602 = new Department() { Name = "开发部驻场服务组", Description = "", SortCode = "0602" };
            dept01.ParentDepartment = dept01;
            dept02.ParentDepartment = dept02;
            dept03.ParentDepartment = dept03;
            dept04.ParentDepartment = dept04;
            dept0401.ParentDepartment = dept04;
            dept0402.ParentDepartment = dept04;
            dept0403.ParentDepartment = dept04;
            dept05.ParentDepartment = dept05;
            dept06.ParentDepartment = dept06;
            dept0601.ParentDepartment = dept06;
            dept0602.ParentDepartment = dept06;

            var depts = new List<Department>() { dept01, dept02, dept03, dept04, dept0401, dept0402, dept0403, dept05, dept06, dept0601, dept0602 };
            foreach (var item in depts)
                _Context.Departments.Add(item);
            _Context.SaveChanges();

            if (_Context.Persons.Any())
                return;
            var persons = new List<Person>()
            {
                new Person() { Name="刘虎军", FixedTelephone="15107728899", SortCode="01001", Description="请补充个人简介", Department=dept01,Avatar=imagesHead },
                new Person() { Name="魏小花", FixedTelephone="13678622345", SortCode="01002", Description="请补充个人简介",Department=dept02,Avatar=imagesHead },
                new Person() { Name="李文慧", FixedTelephone="13690251923", SortCode="01003", Description="请补充个人简介",Department=dept02,Avatar=imagesHead },
                new Person() { Name="张江的", FixedTelephone="13362819012", SortCode="01004", Description="请补充个人简介",Department=dept03,Avatar=imagesHead },
                new Person() { Name="萧可君", FixedTelephone="13688981234", SortCode="01005", Description="请补充个人简介",Department=dept03,Avatar=imagesHead },
                new Person() { Name="魏铜生", FixedTelephone="18398086323", SortCode="01006", Description="请补充个人简介",Department=dept03,Avatar=imagesHead },
                new Person() { Name="刘德华", FixedTelephone="13866225636", SortCode="01007", Description="请补充个人简介",Department=dept03,Avatar=imagesHead },
                new Person() { Name="魏星亮", FixedTelephone="13872236091", SortCode="01008", Description="请补充个人简介",Department=dept04,Avatar=imagesHead },
                new Person() { Name="潘家富", FixedTelephone="13052366213", SortCode="01009", Description="请补充个人简介",Department=dept0401,Avatar=imagesHead },
                new Person() { Name="黎温德", FixedTelephone="13576345509", SortCode="01010", Description="请补充个人简介",Department=dept0401,Avatar=imagesHead },
                new Person() { Name="邓淇升", FixedTelephone="13709823456", SortCode="01011", Description="请补充个人简介" ,Department=dept0402,Avatar=imagesHead },
                new Person() { Name="谭冠希", FixedTelephone="18809888754", SortCode="01012", Description="请补充个人简介" ,Department=dept0403,Avatar=imagesHead },
                new Person() { Name="陈慧琳", FixedTelephone="13172038023", SortCode="01013", Description="请补充个人简介" ,Department=dept05,Avatar=imagesHead },
                new Person() { Name="祁华钰", FixedTelephone="15107726987", SortCode="01014", Description="请补充个人简介" ,Department=dept06,Avatar=imagesHead },
                new Person() { Name="胡德财", FixedTelephone="13900110988", SortCode="01015", Description="请补充个人简介" ,Department=dept0601,Avatar=imagesHead },
                new Person() { Name="吴富贵", FixedTelephone="15087109921", SortCode="01016", Description="请补充个人简介" ,Department=dept0602,Avatar=imagesHead }
            };
            foreach (var person in persons)
            {
                _Context.Persons.Add(person);
            }
            _Context.SaveChanges();

        }

        /// <summary>
        /// 用户和其对应业务的初始数据
        /// </summary>
        public static void _UserAndBusiness()
        {

            #region 用户管理
            //var applicationUsers = new List<ApplicationUser>()
            //{
            if (_Context.ApplicationUsers.Any())
                return;
            var user01 = new ApplicationUser() { Name = "我的光", UserName = "zhangjl", NormalizedUserName = "zhangjl", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-159), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行", Power = AnJurisdiction.Founder };
            var user02 = new ApplicationUser() { Name = "对方正在输入...", UserName = "lingh", NormalizedUserName = "lingh", Sex = false, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-53), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行", Power = AnJurisdiction.Admin };
            var user03 = new ApplicationUser() { Name = "彭", UserName = "pengkf", NormalizedUserName = "pengkf", Sex = false, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-73), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行", Power = AnJurisdiction.Admin };
            var user04 = new ApplicationUser() { Name = "张江的", UserName = "zhangjd", NormalizedUserName = "zhangjd", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-103), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user05 = new ApplicationUser() { Name = "萧可君", UserName = "xiaokj", NormalizedUserName = "xiaokj", Sex = false, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-233), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user06 = new ApplicationUser() { Name = "魏铜生", UserName = "weits", NormalizedUserName = "weits", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-104), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user07 = new ApplicationUser() { Name = "刘德华", UserName = "liudh", NormalizedUserName = "liudh", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-213), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user08 = new ApplicationUser() { Name = "魏星亮", UserName = "weixl", NormalizedUserName = "weixl", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-54), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user09 = new ApplicationUser() { Name = "潘家富", UserName = "panjh", NormalizedUserName = "panjh", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-83), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user10 = new ApplicationUser() { Name = "黎温德", UserName = "liwd", NormalizedUserName = "liwd", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-45), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user11 = new ApplicationUser() { Name = "邓淇升", UserName = "degnqs", NormalizedUserName = "degnqs", Sex = false, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-364), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user12 = new ApplicationUser() { Name = "谭冠希", UserName = "tanggx", NormalizedUserName = "tanggx", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-273), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user13 = new ApplicationUser() { Name = "陈慧琳", UserName = "chenhl", NormalizedUserName = "chenhl", Sex = false, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-293), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user14 = new ApplicationUser() { Name = "祁华钰", UserName = "qihy", NormalizedUserName = "qihy", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-433), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user15 = new ApplicationUser() { Name = "胡德财", UserName = "hudc", NormalizedUserName = "hudc", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-203), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var user16 = new ApplicationUser() { Name = "吴福泉", UserName = "wufq", NormalizedUserName = "wufq", Sex = true, MobileNumber = "15278886321", Email = "123@qq.com", UserAddress = "柳州职业技术学院", LockoutEnabled = true, SecurityStamp = Guid.NewGuid().ToString(), Avatar = imagesHead, RegisterTime = DateTime.Now.AddDays(-123), QQ = "1172211234", School = "柳州职业技术学院", Description = "欢迎来到校园启行" };
            var Appusers = new List<ApplicationUser>() { user01, user02, user03, user04, user05, user06, user07, user08, user09, user10, user11, user12, user13, user14, user15, user16 };
            foreach (var user in Appusers)
            {
                _Context.ApplicationUsers.Add(user);
                //添加加密密码
                user.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "123@abc");
            }
            _Context.SaveChanges();
            #endregion

            #region 添加社团
            //if (_Context.AnAssociations.Any())
            //    return;
            var an01 = new AnAssociation() { Name = "光凌摄影协会", Description = "光凌摄影协会", SchoolAddress = "柳州职业技术学院32652", User = user01, Avatar = imagesDefault };
            var an02 = new AnAssociation() { Name = "电子信息工程协会", Description = "电子信息工程协会", SchoolAddress = "柳州职业技术学院aaga", User = user02, Avatar = imagesDefault };
            var an03 = new AnAssociation() { Name = "骑行社", Description = "我是毕业项目小组a32", SchoolAddress = "柳州职业技术学院ag4ew3", User = user03, Avatar = imagesDefault };
            var an04 = new AnAssociation() { Name = "书法社", Description = "书法社", SchoolAddress = "柳州职业技术学院32a", User = user04, Avatar = imagesDefault };
            var an05 = new AnAssociation() { Name = "舞蹈社", Description = "舞蹈社", SchoolAddress = "柳州职业技术学院a23", User = user05, Avatar = imagesDefault };
            var an06 = new AnAssociation() { Name = "信息技术协会", Description = "这是一个充满温暖的地方", SchoolAddress = "柳州国际职业技术学院", User = user06, Avatar = imagesDefault };
            var an07 = new AnAssociation() { Name = "青年志愿者", Description = "这是一个充满力量的地方", SchoolAddress = "柳州国际职业技术学院", User = user07, Avatar = imagesDefault };
            var an08 = new AnAssociation() { Name = "动漫制作协会", Description = "这是一个充满惬意的地方", SchoolAddress = "柳州国际职业技术学院", User = user08, Avatar = imagesDefault };
            var an09 = new AnAssociation() { Name = "心灵社", Description = "心灵社", SchoolAddress = "柳州国际职业技术学院附属特级辛辛小学", User = user09, Avatar = imagesDefault };
            var an10 = new AnAssociation() { Name = "金字塔学社", Description = "金字塔学社", SchoolAddress = "柳州国际职业技术学院特级实验室", User = user01, Avatar = imagesDefault };
            var an11 = new AnAssociation() { Name = "篮球协会", Description = "我酷我炫我爱萌，么么哒", SchoolAddress = "柳州国际职业技术学院附属人民医院精神科", User = user02, Avatar = imagesDefault };
            var an12 = new AnAssociation() { Name = "国际标准舞协会", Description = "这是一个充满力量的地方", SchoolAddress = "柳州国际职业技术学院112", User = user03, Avatar = imagesDefault };
            var an13 = new AnAssociation() { Name = "梅花桩拳协会", Description = "这是一个充满惬意的地方", SchoolAddress = "柳州国际职业技术学院a323", User = user04, Avatar = imagesDefault };
            var an14 = new AnAssociation() { Name = "排球协会", Description = "这是一个充满温暖的地方", SchoolAddress = "柳州国际职业技术学院", User = user05, Avatar = imagesDefault };
            var an15 = new AnAssociation() { Name = "乒乓球协会", Description = "这是一个充满力量的地方", SchoolAddress = "柳州国际职业技术学院", User = user06, Avatar = imagesDefault };
            var an16 = new AnAssociation() { Name = "跆拳道协会", Description = "跆拳道协会", SchoolAddress = "柳州职业技术学院323", User = user07, Avatar = imagesDefault };
            var an17 = new AnAssociation() { Name = "网球协会", Description = "网球协会", SchoolAddress = "柳州职业技术学院1213", User = user08, Avatar = imagesDefault };
            var an18 = new AnAssociation() { Name = "灯谜协会", Description = "灯谜协会", SchoolAddress = "柳州职业技术学院agew", User = user09, Avatar = imagesDefault };
            var an19 = new AnAssociation() { Name = "饮食文化协会", Description = "这是一个充满温暖的地方", SchoolAddress = "柳州国际职业技术学院", User = user01, Avatar = imagesDefault };
            var an20 = new AnAssociation() { Name = "[WILL]动漫协会", Description = "这是一个充满力量的地方", SchoolAddress = "柳州国际职业技术学院", User = user01, Avatar = imagesDefault };

            var anDataS = new List<AnAssociation> { an01, an02, an03, an04, an05, an06, an07, an08, an09, an10, an11, an12, an13, an14, an15, an16, an17, an18, an19, an20, };
            foreach (var anData in anDataS)
                _Context.AnAssociations.Add(anData);
            _Context.SaveChanges();

            //添加社团人员
            if (_Context.AnAssociationAndUsers.Any())
                return;
            var anAssociationAndUsers = new List<AnAssociationAndUser>()
            {
                //an01社团的人数
               new AnAssociationAndUser{ AnAssociationId=an01.ID,User=user01,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an01.ID,User=user02,AnJurisdictionManager=AnJurisdiction.Admin },
               new AnAssociationAndUser{ AnAssociationId=an01.ID,User=user03,AnJurisdictionManager=AnJurisdiction.Ordinary },
               new AnAssociationAndUser{ AnAssociationId=an01.ID,User=user04,AnJurisdictionManager=AnJurisdiction.Ordinary },
               new AnAssociationAndUser{ AnAssociationId=an01.ID,User=user05,AnJurisdictionManager=AnJurisdiction.Ordinary},

               //an02社团的人数
               new AnAssociationAndUser{ AnAssociationId=an02.ID,User=user01,AnJurisdictionManager=AnJurisdiction.Ordinary },
               new AnAssociationAndUser{ AnAssociationId=an02.ID,User=user02,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an02.ID,User=user03,AnJurisdictionManager=AnJurisdiction.Ordinary },
               new AnAssociationAndUser{ AnAssociationId=an02.ID,User=user04,AnJurisdictionManager=AnJurisdiction.Ordinary },
               new AnAssociationAndUser{ AnAssociationId=an02.ID,User=user05,AnJurisdictionManager=AnJurisdiction.Ordinary},

                //an03社团的人数
               new AnAssociationAndUser{ AnAssociationId=an03.ID,User=user01,AnJurisdictionManager=AnJurisdiction.Ordinary },
               new AnAssociationAndUser{ AnAssociationId=an03.ID,User=user02,AnJurisdictionManager=AnJurisdiction.Admin },
               new AnAssociationAndUser{ AnAssociationId=an03.ID,User=user03,AnJurisdictionManager=AnJurisdiction.Founder },


                //an04社团的人数
               new AnAssociationAndUser{ AnAssociationId=an04.ID,User=user01,AnJurisdictionManager=AnJurisdiction.Ordinary },
               new AnAssociationAndUser{ AnAssociationId=an04.ID,User=user02,AnJurisdictionManager=AnJurisdiction.Admin },
               new AnAssociationAndUser{ AnAssociationId=an04.ID,User=user04,AnJurisdictionManager=AnJurisdiction.Founder },

                //an05社团的人数
               new AnAssociationAndUser{ AnAssociationId=an05.ID,User=user01,AnJurisdictionManager=AnJurisdiction.Ordinary },
               new AnAssociationAndUser{ AnAssociationId=an05.ID,User=user02,AnJurisdictionManager=AnJurisdiction.Admin },
               new AnAssociationAndUser{ AnAssociationId=an05.ID,User=user05,AnJurisdictionManager=AnJurisdiction.Founder },

               new AnAssociationAndUser{ AnAssociationId=an06.ID,User=user06,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an07.ID,User=user07,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an08.ID,User=user08,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an09.ID,User=user09,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an10.ID,User=user01,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an11.ID,User=user02,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an12.ID,User=user03,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an13.ID,User=user04,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an14.ID,User=user05,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an15.ID,User=user06,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an16.ID,User=user07,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an17.ID,User=user08,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an18.ID,User=user09,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an19.ID,User=user01,AnJurisdictionManager=AnJurisdiction.Founder },
               new AnAssociationAndUser{ AnAssociationId=an20.ID,User=user01,AnJurisdictionManager=AnJurisdiction.Founder },
            };

            foreach (var anAssociationAndUser in anAssociationAndUsers)
                _Context.AnAssociationAndUsers.Add(anAssociationAndUser);
            _Context.SaveChanges();
            #endregion

            #region 添加活动

            //添加活动
            if (_Context.ActivityTerms.Any())
                return;
            var ac01 = new ActivityTerm() { Name = "柳州大龙潭游玩活动", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user01, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault,AnAssociation= an01 };
            var ac02 = new ActivityTerm() { Name = "柳州动物园游玩", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user02, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault, AnAssociation = an02 };
            var ac03 = new ActivityTerm() { Name = "环江骑行", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user03, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault, AnAssociation = an03 };
            var ac04 = new ActivityTerm() { Name = "百里徒步", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac05 = new ActivityTerm() { Name = "骑行活动", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac06 = new ActivityTerm() { Name = "观光文庙", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user03, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault, AnAssociation = an03 };
            var ac07 = new ActivityTerm() { Name = "吃遍谷阜街", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user03, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault, AnAssociation = an03 };
            var ac08 = new ActivityTerm() { Name = "柳州花果山", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user03, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac09 = new ActivityTerm() { Name = "阳光柳侯公园", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user03, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac10 = new ActivityTerm() { Name = "梯田三江下， 悠然见南山", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user01, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault, AnAssociation = an01 };
            var ac11 = new ActivityTerm() { Name = "安魂曲的足迹——一路西行", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user01, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac12 = new ActivityTerm() { Name = "柳城知青城", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user01, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac13 = new ActivityTerm() { Name = "融水民族体育公园", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user01, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(29), StartDataTime = DateTime.Now.AddDays(30), EndDataTime = DateTime.Now.AddDays(31), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac14 = new ActivityTerm() { Name = "欣赏柳州壮族风情", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(11), StartDataTime = DateTime.Now.AddDays(12), EndDataTime = DateTime.Now.AddDays(14), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac15 = new ActivityTerm() { Name = "刘三姐山歌文化之旅", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(11), StartDataTime = DateTime.Now.AddDays(12), EndDataTime = DateTime.Now.AddDays(14), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac16 = new ActivityTerm() { Name = "原始生态旅游", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(11), StartDataTime = DateTime.Now.AddDays(12), EndDataTime = DateTime.Now.AddDays(14), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac17 = new ActivityTerm() { Name = "工农业旅游", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(11), StartDataTime = DateTime.Now.AddDays(12), EndDataTime = DateTime.Now.AddDays(14), Status = ActivityStatus.未开始, Avatar = imagesDefault,AnAssociation= an04 };
            var ac18 = new ActivityTerm() { Name = "都乐园烧烤", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(11), StartDataTime = DateTime.Now.AddDays(12), EndDataTime = DateTime.Now.AddDays(14), Status = ActivityStatus.未开始, Avatar = imagesDefault, AnAssociation = an04 };
            var ac19 = new ActivityTerm() { Name = "钓鱼岛真人CS", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(11), StartDataTime = DateTime.Now.AddDays(12), EndDataTime = DateTime.Now.AddDays(14), Status = ActivityStatus.未开始, Avatar = imagesDefault, AnAssociation = an04 };
            var ac20 = new ActivityTerm() { Name = "观光千亩湖", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(11), StartDataTime = DateTime.Now.AddDays(12), EndDataTime = DateTime.Now.AddDays(14), Status = ActivityStatus.未开始, Avatar = imagesDefault };
            var ac21 = new ActivityTerm() { Name = "真人CS活动-水弹枪", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(-2), StartDataTime = DateTime.Now.AddDays(-1), EndDataTime = DateTime.Now.AddDays(0), Status = ActivityStatus.已取消, Avatar = imagesDefault };
            var ac22 = new ActivityTerm() { Name = "古庙文化交流活动", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user02, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(-4), StartDataTime = DateTime.Now.AddDays(-3), EndDataTime = DateTime.Now.AddDays(-2), Status = ActivityStatus.已取消, Avatar = imagesDefault };
            var ac23 = new ActivityTerm() { Name = "柳江划船", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user03, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(-4), StartDataTime = DateTime.Now.AddDays(-3), EndDataTime = DateTime.Now.AddDays(-2), Status = ActivityStatus.已结束, Avatar = imagesDefault };
            var ac24 = new ActivityTerm() { Name = "真人cs-电子枪", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user02, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(-4), StartDataTime = DateTime.Now.AddDays(-3), EndDataTime = DateTime.Now.AddDays(-2), Status = ActivityStatus.已结束, Avatar = imagesDefault };
            var ac25 = new ActivityTerm() { Name = "柳州龙潭", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(-4), StartDataTime = DateTime.Now.AddDays(-3), EndDataTime = DateTime.Now.AddDays(-2), Status = ActivityStatus.已结束, Avatar = imagesDefault };
            var ac26 = new ActivityTerm() { Name = "融安玻璃栈道", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user04, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(-4), StartDataTime = DateTime.Now.AddDays(-3), EndDataTime = DateTime.Now.AddDays(-2), Status = ActivityStatus.已结束, Avatar = imagesDefault };
            var ac27 = new ActivityTerm() { Name = "柳州园博园", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user01, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(1 - 4), StartDataTime = DateTime.Now.AddDays(-3), EndDataTime = DateTime.Now.AddDays(-2), Status = ActivityStatus.已结束, Avatar = imagesDefault };
            var ac28 = new ActivityTerm() { Name = "君武森林公园", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user01, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(-4), StartDataTime = DateTime.Now.AddDays(-3), EndDataTime = DateTime.Now.AddDays(-2), Status = ActivityStatus.已结束, Avatar = imagesDefault };
            var ac29 = new ActivityTerm() { Name = "柳州滨江湿地生态公园", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user01, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(-4), StartDataTime = DateTime.Now.AddDays(-3), EndDataTime = DateTime.Now.AddDays(-2), Status = ActivityStatus.已结束, Avatar = imagesDefault };
            var ac30 = new ActivityTerm() { Name = "刘三姐山歌文化之旅2", Description = "人生不可能是一场说走就走的旅行 ，但心怀远方的人，一定要时刻做着说走就走的准备！", User = user01, MaxNumber = 150, Address = "柳州市龙潭公园", SignDataTime = DateTime.Now.AddDays(-4), StartDataTime = DateTime.Now.AddDays(-2), EndDataTime = DateTime.Now.AddDays(-1), Status = ActivityStatus.已结束, Avatar = imagesDefault };
            var acDatas = new List<ActivityTerm> { ac01, ac02, ac03, ac04 , ac05 , ac06 , ac07 , ac08 , ac09 , ac10 , ac11 , ac12 , ac13 , ac14 , ac15 , ac16 , ac17 , ac18 , ac19 ,
            ac20,ac21,ac22,ac23,ac24,ac25,ac26,ac26,ac27,ac28,ac29,ac30};
            foreach (var acData in acDatas)
                _Context.ActivityTerms.Add(acData);
            _Context.SaveChanges();

            //添加活动人员
            if (_Context.ActivityUsers.Any())
                return;
            var activityUsers = new List<ActivityUser>()
            {
                //ac01活动的人数user01
               new ActivityUser{ ActivityTermId=ac01.ID,User=user01},
               new ActivityUser{ ActivityTermId=ac01.ID,User=user02},
               new ActivityUser{ ActivityTermId=ac01.ID,User=user03},
               new ActivityUser{ ActivityTermId=ac01.ID,User=user04},
               new ActivityUser{ ActivityTermId=ac01.ID,User=user05},

               //ac02活动的人数
               new ActivityUser{ ActivityTermId=ac02.ID,User=user01},
               new ActivityUser{ ActivityTermId=ac02.ID,User=user02},
               new ActivityUser{ ActivityTermId=ac02.ID,User=user03},
               new ActivityUser{ ActivityTermId=ac02.ID,User=user04},
               new ActivityUser{ ActivityTermId=ac02.ID,User=user05},

                //ac03活动的人数
               new ActivityUser{ ActivityTermId=ac03.ID,User=user01},
               new ActivityUser{ ActivityTermId=ac03.ID,User=user02},
               new ActivityUser{ ActivityTermId=ac03.ID,User=user03},


                //ac04活动的人数
               new ActivityUser{ActivityTermId=ac04.ID,User=user01},
               new ActivityUser{ActivityTermId=ac04.ID,User=user02},
               new ActivityUser{ActivityTermId=ac04.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac05.ID,User=user02},
               new ActivityUser{ ActivityTermId=ac05.ID,User=user03},
               new ActivityUser{ ActivityTermId=ac05.ID,User=user01},
               new ActivityUser{ ActivityTermId=ac05.ID,User=user05},
               new ActivityUser{ ActivityTermId=ac05.ID,User=user04},


               new ActivityUser{ ActivityTermId=ac06.ID,User=user01},
               new ActivityUser{ ActivityTermId=ac06.ID,User=user02},
               new ActivityUser{ ActivityTermId=ac06.ID,User=user04},
               new ActivityUser{ ActivityTermId=ac06.ID,User=user05},
               new ActivityUser{ ActivityTermId=ac06.ID,User=user03},


               new ActivityUser{ ActivityTermId=ac07.ID,User=user01},
               new ActivityUser{ ActivityTermId=ac07.ID,User=user02},
               new ActivityUser{ ActivityTermId=ac07.ID,User=user03},

               new ActivityUser{ ActivityTermId=ac08.ID,User=user02},
               new ActivityUser{ ActivityTermId=ac08.ID,User=user01},
               new ActivityUser{ ActivityTermId=ac08.ID,User=user04},
               new ActivityUser{ ActivityTermId=ac08.ID,User=user05},
               new ActivityUser{ ActivityTermId=ac08.ID,User=user03},

               new ActivityUser{ ActivityTermId=ac09.ID,User=user01},
               new ActivityUser{ ActivityTermId=ac09.ID,User=user02},
               new ActivityUser{ ActivityTermId=ac09.ID,User=user04},
               new ActivityUser{ ActivityTermId=ac09.ID,User=user05},
               new ActivityUser{ ActivityTermId=ac09.ID,User=user03},

               new ActivityUser{ ActivityTermId=ac10.ID,User=user03},
               new ActivityUser{ ActivityTermId=ac10.ID,User=user02},
               new ActivityUser{ ActivityTermId=ac10.ID,User=user01},

               new ActivityUser{ ActivityTermId=ac11.ID,User=user01},

               new ActivityUser{ ActivityTermId=ac12.ID,User=user01},

               new ActivityUser{ ActivityTermId=ac13.ID,User=user01},

               new ActivityUser{ ActivityTermId=ac14.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac15.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac16.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac17.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac18.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac19.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac20.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac21.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac22.ID,User=user02},

               new ActivityUser{ ActivityTermId=ac23.ID,User=user03},

               new ActivityUser{ ActivityTermId=ac24.ID,User=user02},

               new ActivityUser{ ActivityTermId=ac25.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac26.ID,User=user04},

               new ActivityUser{ ActivityTermId=ac27.ID,User=user01},

               new ActivityUser{ ActivityTermId=ac28.ID,User=user01},

               new ActivityUser{ ActivityTermId=ac29.ID,User=user01},

               new ActivityUser{ ActivityTermId=ac30.ID,User=user01},
            };

            foreach (var activityUser in activityUsers)
                _Context.ActivityUsers.Add(activityUser);
            _Context.SaveChanges();
            #endregion

            #region 爱好 Hobby
            #region
            if (_Context.Hobbys.Any())
                return;
            var ah01 = new Hobby() { Name = "唱歌", Description = "我是爱好介绍1", SortCode = "01", Avatar = imagesDefault };
            var ah02 = new Hobby() { Name = "跳舞", Description = "我是爱好介绍2", SortCode = "02", Avatar = imagesDefault };
            var ah03 = new Hobby() { Name = "骑行", Description = "我是爱好介绍3", SortCode = "03", Avatar = imagesDefault };
            var ah04 = new Hobby() { Name = "拍照", Description = "我是爱好介绍4", SortCode = "04", Avatar = imagesDefault };
            var ah05 = new Hobby() { Name = "旅游", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah06 = new Hobby() { Name = "看电影", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah07 = new Hobby() { Name = "听歌", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah08 = new Hobby() { Name = "小说", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah09 = new Hobby() { Name = "乒乓球", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah10 = new Hobby() { Name = "篮球", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah11 = new Hobby() { Name = "足球", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah12 = new Hobby() { Name = "羽毛球", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah13 = new Hobby() { Name = "网球", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah14 = new Hobby() { Name = "明星", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah15 = new Hobby() { Name = "看书", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah16 = new Hobby() { Name = "吃喝", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah17 = new Hobby() { Name = "新闻", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah18 = new Hobby() { Name = "逗比族", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah19 = new Hobby() { Name = "逛空间", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };
            var ah20 = new Hobby() { Name = "僵小鱼", Description = "我是爱好介绍5", SortCode = "05", Avatar = imagesDefault };

            var ahobby = new List<Hobby> { ah01, ah02, ah03, ah04, ah05,ah06,ah07,ah08,ah09,ah10,ah11,ah12,ah13,ah14,ah15, ah16, ah17, ah18,
            ah19,ah20};
            foreach (var hobby in ahobby)
                _Context.Hobbys.Add(hobby);
            _Context.SaveChanges();
            #endregion
            //每个用户对应爱好 ApplicationUserAndHobby
            if (_Context.ApplicationUserAndHobbys.Any())
                return;
            var applicationUserAndHobbys = new List<ApplicationUserAndHobby>()
            {
                //用户爱好ah01
                new ApplicationUserAndHobby{User=user01,Hobby=ah01 },
                new ApplicationUserAndHobby{User=user01,Hobby=ah02 },
                new ApplicationUserAndHobby{User=user01,Hobby=ah03 },
                new ApplicationUserAndHobby{User=user01,Hobby=ah04 },
                new ApplicationUserAndHobby{User=user01,Hobby=ah05 },

                 //用户爱好ah02
                new ApplicationUserAndHobby{User=user02,Hobby=ah01 },
                new ApplicationUserAndHobby{User=user02,Hobby=ah02 },
                new ApplicationUserAndHobby{User=user02,Hobby=ah03 },

                 //用户爱好ah03
                new ApplicationUserAndHobby{ User=user03,Hobby=ah01},
                new ApplicationUserAndHobby{ User=user03,Hobby=ah02},
                new ApplicationUserAndHobby{ User=user03,Hobby=ah03},

                 //用户爱好ah04
                new ApplicationUserAndHobby{ User=user04,Hobby=ah01},
                new ApplicationUserAndHobby{ User=user04,Hobby=ah02},
                new ApplicationUserAndHobby{ User=user04,Hobby=ah03},

                 //用户爱好ah05
                new ApplicationUserAndHobby{ User=user05,Hobby=ah01},
                new ApplicationUserAndHobby{ User=user05,Hobby=ah02},
                new ApplicationUserAndHobby{ User=user05,Hobby=ah03},
            };

            foreach (var applicationUserAndHobby in applicationUserAndHobbys)
                _Context.ApplicationUserAndHobbys.Add(applicationUserAndHobby);
            _Context.SaveChanges();

            #endregion
        }
    }
}

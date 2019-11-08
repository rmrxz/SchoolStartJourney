using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using School.DataAccess;
using School.DataAccess.Seeds;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.BusinessOrganization;
using School.Entities.GroupOrganization;
using School.Web.Common;

namespace School.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加 EF Core 框架
            services.AddDbContext<EntityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // 添加微软自己的用户登录令牌资料
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<EntityDbContext>();
            //.AddDefaultTokenProviders();

            // Add framework services.
            services.AddMvc();

            // 配置 Identity
            services.Configure<IdentityOptions>(options =>
            {
                // 密码策略的常规设置
                options.Password.RequireDigit = true;            // 是否需要数字字符
                options.Password.RequiredLength = 6;             // 必须的长度
                options.Password.RequireNonAlphanumeric = false;  // 是否需要非拉丁字符，如%，@ 等
                options.Password.RequireUppercase = false;        // 是否需要大写字符
                options.Password.RequireLowercase = true;        // 是否需要小写字符

                // 登录尝试锁定策略
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                //// Cookie 设置
                //options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(15);
                //options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";   // 缺省的登录路径
                //options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOut"; // 注销以后的路径

                // 其它的一些设置
                options.User.RequireUniqueEmail = true;
            });
            //要使用数据操作的话，要通过注入才能使用哪些方法 
            #region 域控制器相关的依赖注入服务清单
            //Person
            services.AddTransient<IEntityRepository<Person>, EntityRepository<Person>>();
            services.AddTransient<IEntityRepository<Department>, EntityRepository<Department>>();
            services.AddTransient<IDataExtension<Person>, DataExtension<Person>>();
            services.AddTransient<IDataExtension<Department>, DataExtension<Department>>();
            //User和个人业务
            services.AddTransient<IDataExtension<ApplicationUser>, DataExtension<ApplicationUser>>();
            services.AddTransient<IEntityRepository<Hobby>, EntityRepository<Hobby>>();
            services.AddTransient<IDataExtension<Hobby>, DataExtension<Hobby>>(); 
            services.AddTransient<IEntityRepository<ApplicationUserAndHobby>, EntityRepository<ApplicationUserAndHobby>>();
            services.AddTransient<IDataExtension<ApplicationUserAndHobby>, DataExtension<ApplicationUserAndHobby>>();
            services.AddTransient<IDataExtension<ApplicationUser>, DataExtension<ApplicationUser>>();
            services.AddTransient<IDataExtension<MessageNotification>, DataExtension<MessageNotification>>();
            services.AddTransient<IEntityRepository<MessageNotification>, EntityRepository<MessageNotification>>();

            //社团业务
            services.AddTransient<IDataExtension<ActivityTerm>, DataExtension<ActivityTerm>>();
            services.AddTransient<IEntityRepository<ActivityTerm>, EntityRepository<ActivityTerm>>();
            services.AddTransient<IEntityRepository<AnAssociation>, EntityRepository<AnAssociation>>();
            services.AddTransient<IDataExtension<AnAssociation>, DataExtension<AnAssociation>>();
            services.AddTransient<IDataExtension<ActivityUser>, DataExtension<ActivityUser>>();
            services.AddTransient<IEntityRepository<ActivityUser>, EntityRepository<ActivityUser>>();
            services.AddTransient<IDataExtension<AnAssociationAndUser>, DataExtension<AnAssociationAndUser>>();
            services.AddTransient<IEntityRepository<UserFriend>, EntityRepository<UserFriend>>();
            services.AddTransient<IDataExtension<UserFriend>, DataExtension<UserFriend>>();
            services.AddTransient<IDataExtension<ActivityComment>, DataExtension<ActivityComment>>();
            services.AddTransient<IDataExtension<HomeExhibition>, DataExtension<HomeExhibition>>();

            //上传文件
            services.AddTransient<IDataExtension<BusinessImage>, DataExtension<BusinessImage>>();
            services.AddTransient<IEntityRepository<BusinessImage>, EntityRepository<BusinessImage>>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, EntityDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Home/Error");
                app.UseExceptionHandler("/Home/NotLogin");
            }

            app.UseStaticFiles();
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=ShoolStart}/{action=Index}/{id?}");
            });
            DbInitializer.Initialize(context);
        }
    }
}

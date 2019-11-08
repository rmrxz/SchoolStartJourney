using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Entities.ApplicationOrganization;
using School.Entities.Attachments;
using School.Entities.BusinessOrganization;
using School.Entities.GroupOrganization;


namespace School.DataAccess.SqlServer
{
    public class EntityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {
        }
        #region 人员和部门
        public DbSet<Person> Persons { get; set; }
        public DbSet<Department> Departments { get; set; }
        #endregion

        #region 用户和功能
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<AnAssociationAndUser> AnAssociationAndUsers { get; set; }
        public DbSet<ApplicationUserAndHobby> ApplicationUserAndHobbys { get; set; }
        public DbSet<FollowAndAssociation> FollowAndAssociations { get; set; }
        public DbSet<FollowEvent> FollowEvents { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
        public DbSet<Hobby> Hobbys { get; set; }
        public DbSet<MessageNotification> MessageNotifications { get; set; }
        #endregion

        #region 业务活动
        public DbSet<ActivityTerm> ActivityTerms { get; set; }
        public DbSet<ActivityUser> ActivityUsers { get; set; }
        public DbSet<AnAssociation> AnAssociations { get; set; }
        public DbSet<ActivityComment> Comments { get; set; }

        public DbSet<HomeExhibition> HomeExhibitions { get; set; }
        #endregion

        #region 文件
        public DbSet<BusinessFile> BusinessFiles { get; set; }
        public DbSet<BusinessImage> BusinessImages { get; set; }
        public DbSet<BusinessVideo> BusinessVideos { get; set; }
        #endregion



        /// <summary>
        /// 如果不需要 DbSet<T> 所定义的属性名称作为数据库表的名称，可以在下面的位置自己重新定义
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Person>().ToTable("Person");
            base.OnModelCreating(modelBuilder);

        }
    }
}

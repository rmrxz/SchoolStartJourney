using School.DataAccess.Common;
using School.DataAccess.SqlServer;
using School.DataAccess.SqlServer.Utilities;
using School.Entities;
using School.Common.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace School.DataAccess
{
    public interface IEntityRepository<T> where T : class,IEntityBase, new()
    {
        EntityDbContext EntitiesContext { get; set; }
        /// <summary>
        /// 持久化数据
        /// </summary>
        void Save();

        /// <summary>
        /// 无限制提取业务
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();


       
        /// <summary>
        /// 根据对象的ID提取具体的对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetSingle(Guid id);


        /// <summary>
        /// 添加对象到内存中的数据集中
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);


        /// <summary>
        /// 编辑内存中对应的数据集的对象
        /// </summary>
        /// <param name="entity"></param>
        void Edit(T entity);


        /// <summary>
        /// 根据内存中对应的数据集是否存在，自动决定采取添加或者编辑方法处理传入的对象
        /// </summary>
        /// <param name="entity"></param>
        void AddOrEdit(T entity);

        /// <summary>
        /// 根据内存中对应的数据集是否存在，自动决定采取添加或者编辑方法处理传入的对象，并直接持久化。
        /// </summary>
        /// <param name="entity"></param>
        void AddOrEditAndSave(T entity);

        /// <summary>
        /// 根据对象ID来执行删除，并根据所定义的删除依赖关系检查删除操作执行后的结果，返回相应的执行状态模型供前端应用使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="relevanceOperations"></param>
        /// <returns></returns>
        DeleteStatus DeleteAndSave(Guid id, List<object> relevanceOperations);

        /// <summary>
        /// 根据对象 ID 判断在数据库中是否存在相应的对象数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool HasInstance(Guid id);

        #region 异步方法定义
        Task<T> GetSingleAsyn(Guid id);
        Task<T> GetSingleAsyn(Guid id, params Expression<Func<T, object>>[] includeProperties);
        Task<bool> HasInstanceAsyn(Guid id);

        Task<bool> AddOrEditAndSaveAsyn(T entity);//添加或者编辑数据
        Task<DeleteStatusModel> DeleteAndSaveAsyn(Guid id);

        #endregion
    }
}

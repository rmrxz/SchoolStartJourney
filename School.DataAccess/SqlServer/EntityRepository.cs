using Microsoft.EntityFrameworkCore;
using School.Common.JsonModels;
using School.DataAccess.Common;
using School.DataAccess.SqlServer.Utilities;
using School.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace School.DataAccess.SqlServer
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, IEntityBase, new()
    {
        readonly EntityDbContext _entitiesContext;

        public EntityDbContext EntitiesContext { get; set; }

        public EntityRepository(EntityDbContext context)
        {
            _entitiesContext = context;
        }

        public virtual void Save()
        {
            try
            {
                _entitiesContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // 获取错误信息集合
                var errorMessages = ex.Message;
                var itemErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " Error: ", itemErrorMessage);
                throw new DbUpdateException(exceptionMessage, ex);
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            return _entitiesContext.Set<T>();
        }


        /// <summary>
        /// 根据对象的ID提取具体的对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetSingle(Guid id)
        {
            return GetAll().FirstOrDefault(x => x.ID == id);
        }

     





        /// <summary>
        /// 添加对象到内存中的数据集中
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            _entitiesContext.Set<T>().Add(entity);
        }

   
        /// <summary>
        /// 编辑内存中对应的数据集的对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Edit(T entity)
        {
            _entitiesContext.Set<T>().Update(entity);
        }

     
        /// <summary>
        /// 根据内存中对应的数据集是否存在，自动决定采取添加或者编辑方法处理传入的对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual void AddOrEdit(T entity)
        {
            var p = GetAll().FirstOrDefault(x => x.ID == entity.ID);
            if (p == null)
            {
                Add(entity);
            }
            else
            {
                Edit(entity);
            }
        }

        /// <summary>
        /// 根据内存中对应的数据集是否存在，自动决定采取添加或者编辑方法处理传入的对象，并直接持久化。
        /// </summary>
        /// <param name="entity"></param>
        public virtual void AddOrEditAndSave(T entity)
        {
            AddOrEdit(entity);
            Save();
        }

     
        /// <summary>
        /// 根据对象ID来执行删除，并根据所定义的删除依赖关系检查删除操作执行后的结果，返回相应的执行状态模型供前端应用使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="relevanceOperations"></param>
        /// <returns></returns>
        public virtual DeleteStatus DeleteAndSave(Guid id, List<object> relevanceOperations)
        {
            var deleteStatus = new DeleteStatus();
            var returnStatus = true;
            var returnMessage = "";
            var bo = GetSingle(id);

            if (bo == null)
            {
                returnStatus = false;
                returnMessage = "你所删除的数据不存在，如果确定不是数据逻辑错误原因，请将本情况报告系统管理人员。";
                deleteStatus.Initialize(returnStatus, returnMessage);
            }
            else
            {
                #region 处理关联关系

                var i = 0;
                foreach (var deleteOperationObject in relevanceOperations)
                {
                    var deleteProperty = deleteOperationObject.GetType().GetProperties().Where(pn => pn.Name == "CanDelete").FirstOrDefault();
                    var itCanDelete = (bool)deleteProperty.GetValue(deleteOperationObject);

                    var messageProperty = deleteOperationObject.GetType().GetProperties().Where(pn => pn.Name == "OperationMessage").FirstOrDefault();
                    var messageValue = messageProperty.GetValue(deleteOperationObject) as string;

                    if (!itCanDelete)
                    {
                        returnStatus = false;
                        returnMessage += (i++) + "、" + messageValue + "。\n";
                    }
                }

                #endregion

                if (returnStatus)
                {
                    try
                    {
                        _entitiesContext.Set<T>().Remove(bo);
                        deleteStatus.Initialize(returnStatus, returnMessage);
                    }
                    catch (DbUpdateException)
                    {
                        returnStatus = false;
                        returnMessage = "无法删除所选数据，其信息正被使用，如果确定不是数据逻辑错误原因，请将本情况报告系统管理人员。";
                        deleteStatus.Initialize(returnStatus, returnMessage);
                    }
                }
                else
                {
                    deleteStatus.Initialize(returnStatus, returnMessage);
                }
            }
            return deleteStatus;
        }

        /// <summary>
        /// 根据对象 ID 判断在数据库中是否存在相应的对象数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasInstance(Guid id)
        {
            var dbSet = _entitiesContext.Set<T>();
            return dbSet.Any(x => x.ID == id);
        }


        #region 异步方法定义
     
        public virtual async Task<T> GetSingleAsyn(Guid id)
        {
            var dbSet = _entitiesContext.Set<T>();
            var result = await dbSet.FirstOrDefaultAsync(x => x.ID == id);
            return result;
        }
        public virtual async Task<T> GetSingleAsyn(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> dbSet = _entitiesContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    dbSet = dbSet.Include(includeProperty);
                }
            }

            var result = await dbSet.FirstOrDefaultAsync(x => x.ID == id);
            return result;
        }
        
        public virtual async Task<bool> HasInstanceAsyn(Guid id)
        {
            return await _entitiesContext.Set<T>().AnyAsync(x => x.ID == id);
        }
       
        public virtual async Task<bool> AddOrEditAndSaveAsyn(T entity)//添加或者编辑数据
        {
            var dbSet = _entitiesContext.Set<T>();
            var hasInstance = await dbSet.AnyAsync(x => x.ID == entity.ID);
            if (hasInstance) dbSet.Update(entity);
            else await dbSet.AddAsync(entity);
            try
            {
                await _entitiesContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        public virtual async Task<DeleteStatusModel> DeleteAndSaveAsyn(Guid id)
        {
            var result = new DeleteStatusModel() { IsOK = true, Message = "删除操作成功！" };
            var hasIstance = await HasInstanceAsyn(id);
            if (!hasIstance)
            {
                result.IsOK = false;
                result.Message = "不存在所指定的数据，无法执行删除操作！";
            }
            else
            {
                var tobeDeleteItem = await GetSingleAsyn(id);
                try
                {
                    _entitiesContext.Set<T>().Remove(tobeDeleteItem);
                    _entitiesContext.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    result.IsOK = false;
                    result.Message = ex.Message;
                }
            }
            return result;
        }

        #endregion
    }
}

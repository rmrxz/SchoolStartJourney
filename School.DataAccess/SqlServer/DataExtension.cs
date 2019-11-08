using Microsoft.EntityFrameworkCore;
using School.Common.JsonModels;
using School.DataAccess.Common;
using School.DataAccess.SqlServer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace School.DataAccess.SqlServer
{
    
    public class DataExtension<T> : IDataExtension<T> where T : class, new()
    {
        readonly EntityDbContext _entitiesContext;

        public EntityDbContext EntitiesContext { get; set; }

        public DataExtension(EntityDbContext context)
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
        /// 除了提取本身的对象数据集合外，还提取包含根据表达式提取关联的的对象的集合，
        /// </summary>
        /// <param name="includeProperties">需要直接提取关联类集合数据的表达式集合，通过逗号隔开</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entitiesContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includePropertie in includeProperties)
                {
                    query = query.Include(includePropertie);
                }
            }
            return query;
        }


        /// <summary>
        ///  第一个是判断的条件，除了提取本身的对象数据集合外，还提取包含根据表达式提取关联的的对象的集合
        /// </summary>
        /// <param name="predicate">需要直接提取关联类集合数据的表达式集合，通过逗号隔开<</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetAllMultiCondition(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entitiesContext.Set<T>().Where(predicate);
            if (includeProperties != null)
            {
                foreach (var includePropertie in includeProperties)
                {
                    query = query.Include(includePropertie);
                }
            }
            return query;
        }


        /// <summary>
        /// 根据 Lambda 表达式提取具体的对象，实际上是提取满足表达式限制的集合的第一个对象集合
        /// </summary>
        /// <param name="predicate">布尔条件的 Lambda 表达式</param>
        /// <returns></returns>
        public virtual T GetSingleBy(Expression<Func<T, bool>> predicate)
        {
            return _entitiesContext.Set<T>().Where(predicate).FirstOrDefault(predicate);
        }

        /// <summary>
        /// 根据 Lambda 表达式提取对象集合
        /// </summary>
        /// <param name="predicate">布尔条件的 Lambda 表达式</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _entitiesContext.Set<T>().Where(predicate);
        }



        /// <summary>
        /// 按照指定的属性进行分页，提取分页后的对象集合，在本框架中，通常使用 SortCode
        /// </summary>
        /// <typeparam name="TKey">分页所依赖的属性</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页对象的数量</param>
        /// <param name="keySelector">指定分页依赖属性的 Lambda 表达式</param>
        /// <returns></returns>
        public virtual PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector)
        {
            return Paginate(pageIndex, pageSize, keySelector, null);
        }
        public virtual PaginatedList<T> PaginateDescend<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector)
        {
            return PaginateDescend(pageIndex, pageSize, keySelector, null);
        }


        /// <summary>
        /// 按照指定的属性进行分页，提取分页后的对象的集合(升序)
        /// </summary>
        /// <typeparam name="TKey">分页所依赖的属性</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页对象的数量</param>
        /// <param name="keySelector">指定分页依赖属性的 Lambda 表达式</param>
        /// <param name="predicate">对象集合过滤 Lambda 表达式</param>
        /// <param name="includeProperties">指定的扩展对象属性的表达式集合，通过逗号隔离</param>
        /// <returns></returns>
        public virtual PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = GetAllIncluding(includeProperties).OrderBy(keySelector);
            query = (predicate == null) ? query : query.Where(predicate);
            return query.ToPaginatedList(pageIndex, pageSize);
        }

        public virtual PaginatedList<T> PaginateDescend<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = GetAllIncluding(includeProperties).OrderByDescending(keySelector);
            query = (predicate == null) ? query : query.Where(predicate);
            return query.ToPaginatedList(pageIndex, pageSize);
        }
        public virtual PaginatedList<T> Paginate(Expression<Func<T, bool>> predicate, ref ListPageParameter pagePara)
        {
            return Paginate(predicate, ref pagePara, null);
        }
        public virtual PaginatedList<T> Paginate(Expression<Func<T, bool>> predicate, ref ListPageParameter pagePara, params Expression<Func<T, object>>[] includeProperties)
        {
            var pageIndex = 1;
            var pageSize = 18;

            pageIndex = pagePara.PageIndex;
            pageSize = pagePara.PageSize;


            #region 根据属性名称确定排序的属性的 lambda 表达式 

            var sortPropertyName = pagePara.SortProperty;
            var target = Expression.Parameter(typeof(object));
            var type = typeof(T);
            var castTarget = Expression.Convert(target, type);
            var propertyArray = sortPropertyName.Split('.');
            var getPropertyValue = Expression.Property(castTarget, propertyArray[0]);
            for (var i = 0; i < propertyArray.Count(); i++)
            {
                if (i > 0)
                {
                    getPropertyValue = Expression.Property(getPropertyValue, propertyArray[i]);
                }
            }
            var sortExpession = Expression.Lambda<Func<T, object>>(getPropertyValue, target);

            #endregion

            PaginatedList<T> boCollection;
            if (pagePara.SortDesc.ToLower() == "default" || pagePara.SortDesc == "")
            {
                boCollection = Paginate(pageIndex, pageSize, sortExpession, predicate, includeProperties);
            }
            else
            {
                boCollection = PaginateDescend(pageIndex, pageSize, sortExpession, predicate, includeProperties);
            }

            pagePara.PageAmount = boCollection.TotalPageCount;
            pagePara.PageIndex = boCollection.PageIndex;
            pagePara.ObjectAmount = ((predicate == null) ? GetAll() : GetAll().Where(predicate)).Count();

            return boCollection;
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
        /// 添加对象到内存中的数据集中，并直接持久化。
        /// </summary>
        /// <param name="entity"></param>
        public virtual void AddAndSave(T entity)
        {
            Add(entity);
            Save();
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
        /// 编辑内存中对应的数据集的对象，并直接持久化。
        /// </summary>
        /// <param name="entity"></param>
        public virtual void EditAndSave(T entity)
        {
            Edit(entity);
            Save();
        }

        /// <summary>
        /// 批量编辑并持久化处理的操作，下面参数说明以 Persons 为例
        /// </summary>
        /// <param name="predicate">过滤条件，例如：x=>x.Name=="张三"</param>
        /// <param name="newValueExpression">修改值：x2=> new Person {Name="李四"}</param>
        /// 实际例子：_Service.EditAndSaveBy(x => x.Name == "测试数据", x1 => new SystemWorkSection { Description="用于更改的编辑说明" });
        public virtual void EditAndSaveBy(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> newValueExpression)
        {

        }


        /// <summary>
        /// 删除内存中对应的数据集的对象。
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            _entitiesContext.Set<T>().Remove(entity);
        }

        /// <summary>
        /// 删除内存中对应的数据集的对象，并直接持久化。
        /// </summary>
        /// <param name="entity"></param>
        public virtual void DeleteAndSave(T entity)
        {
            Delete(entity);
            Save();
        }

        /// <summary>
        /// 根据条件执行批量删除并持久化的操作
        /// </summary>
        /// <param name="predicate">条件表达式，例如：x=>x.Name=="abc"，满足的数据对象都将被删除</param>
        public virtual void DeleteAndSaveBy(Expression<Func<T, bool>> predicate)
        {
            var dbSet = _entitiesContext.Set<T>();
            var toBeDeleteItems = dbSet.Where(predicate);
            foreach (var toBeDeleteItem in toBeDeleteItems)
            {
                dbSet.Remove(toBeDeleteItem);
            }
            Save();
        }

       

        /// <summary>
        /// 根据条件判断在数据库中是否存在相应的对象数据记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool HasInstance(Expression<Func<T, bool>> predicate)
        {
            var dbSet = _entitiesContext.Set<T>();
            return dbSet.Any(predicate);
        }

        /// <summary>
        /// 将IQueryable数组转换成List
        /// </summary>
        /// <param name="arrays"></param>
        /// <returns></returns>
        public List<T> ArrayConvert(IQueryable<T> arrays)
        {
            var arrayList = new List<T>();
            foreach (var array in arrays)
            {
                arrayList.Add(array);
            }
            return arrayList;
        }

        #region 异步方法定义
        public virtual async Task<bool> SaveAsyn()
        {
            await _entitiesContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<T> GetSingleAsyn(Expression<Func<T, bool>> predicate)
        {
            var dbSet = _entitiesContext.Set<T>();
            var result = await dbSet.FirstOrDefaultAsync(predicate);
            return result;
        }

        public virtual async Task<IQueryable<T>> GetAllAsyn()
        {
            var dbSet = _entitiesContext.Set<T>();
            var result = await dbSet.ToListAsync();
            return result.AsQueryable<T>();
        }
        public virtual async Task<IQueryable<T>> GetAllIncludingAsyn(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entitiesContext.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includePropertie in includeProperties)
                {
                    query = query.Include(includePropertie);
                }
            }
            var result = await query.ToListAsync();
            return result.AsQueryable();
        }

        //public virtual async Task<T> GetSingleAsyn(Guid id)
        //{
        //    var dbSet = _entitiesContext.Set<T>();
        //    var result = await dbSet.FirstOrDefaultAsync(x => x.ID == id);
        //    return result;
        //}
        //public virtual async Task<T> GetSingleAsyn(Guid id, params Expression<Func<T, object>>[] includeProperties)
        //{
        //    IQueryable<T> dbSet = _entitiesContext.Set<T>();
        //    if (includeProperties != null)
        //    {
        //        foreach (var includeProperty in includeProperties)
        //        {
        //            dbSet = dbSet.Include(includeProperty);
        //        }
        //    }

        //    var result = await dbSet.FirstOrDefaultAsync(x => x.ID == id);
        //    return result;
        //}
        public virtual async Task<IQueryable<T>> FindByAsyn(Expression<Func<T, bool>> predicate)
        {
            var result = await _entitiesContext.Set<T>().Where(predicate).ToListAsync();
            return result.AsQueryable();
        }
        //public virtual async Task<bool> HasInstanceAsyn(Guid id)
        //{
        //    return await _entitiesContext.Set<T>().AnyAsync(x => x.ID == id);
        //}
        public virtual async Task<bool> HasInstanceAsyn(Expression<Func<T, bool>> predicate)//根据Lambda表达式查询是否有该数据，返回bool类型
        {
            return await _entitiesContext.Set<T>().AnyAsync(predicate);
        }
        //public virtual async Task<bool> AddOrEditAndSaveAsyn(T entity)//添加或者编辑数据
        //{
        //    var dbSet = _entitiesContext.Set<T>();
        //    var hasInstance = await dbSet.AnyAsync(x => x.ID == entity.ID);
        //    if (hasInstance) dbSet.Update(entity);
        //    else await dbSet.AddAsync(entity);
        //    try
        //    {
        //        await _entitiesContext.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return false;
        //    }
        //}

        //public virtual async Task<DeleteStatusModel> DeleteAndSaveAsyn(Guid id)
        //{
        //    var result = new DeleteStatusModel() { IsOK = true, Message = "删除操作成功！" };
        //    var hasIstance = await HasInstanceAsyn(id);
        //    if (!hasIstance)
        //    {
        //        result.IsOK = false;
        //        result.Message = "不存在所指定的数据，无法执行删除操作！";
        //    }
        //    else
        //    {
        //        var tobeDeleteItem = await GetSingleAsyn(id);
        //        try
        //        {
        //            _entitiesContext.Set<T>().Remove(tobeDeleteItem);
        //            _entitiesContext.SaveChanges();
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            result.IsOK = false;
        //            result.Message = ex.Message;
        //        }
        //    }
        //    return result;
        //}

        #endregion
    }
}

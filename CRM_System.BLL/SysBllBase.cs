using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM_System.DAL;
using System.Linq.Expressions;
using CRM_System.Model;
using System.Reflection;
using PagedList;

namespace CRM_System.BLL
{
    public abstract class SysBllBase<T> where T : class,new()
    {

        public abstract RepositoryBase<T> repository { get; set; }

        // public ComSQLRepository dbsql = new ComSQLRepository();

        public CRM_SystemEntities dbSession = EFContextFactory.GetCurrentDbContext();//.GetCurrentDbSession();


        public virtual T Insert(T entity)
        {
            return repository.Insert(entity);
        }

        public T Update(T entity)
        {
            repository.Update(entity);
            return entity;
        }

        public T Update(T entity, string[] param)
        {
            repository.Update(entity, param);
            return entity;
        }

        public void UpdateE(T entity)
        {
            repository.UpdateE(entity);
        }

        public void Delete(T entity)
        {
            repository.Delete(entity);
        }

        public T Find(params object[] keyValues)
        {
            return repository.Find(keyValues);
        }

        public T GetModel(Expression<Func<T, bool>> where)
        {
            return repository.GetModel(where);
        }

        public List<T> FindAll()
        {
            return repository.FindAll();
        }

        public List<T> FindByParam(Expression<Func<T, bool>> where)
        {
            return repository.FindByParam(where);
        }
        public object GetMax<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> MaxCoum)
        {
            return repository.GetMax(where, MaxCoum);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="desc">是否升序</param>
        /// <returns></returns>
        public List<T> FindByParam<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy)
        {
            return repository.FindByParam(where, orderBy);
        }
        public List<T> FindByParamDesc<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy)
        {
            return repository.FindByParamDesc(where, orderBy);
        }
        public List<T> FindByParam(Expression<Func<T, bool>> where, string orderBy, bool desc, int count)
        {
            return repository.FindByParam(where, orderBy, desc, count);
        }

        public List<T> GetPage<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize)
        {
            return repository.GetPage(where, orderBy, pageIndex, pageSize);
        }

        public List<T> GetPageDec<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize)
        {
            return repository.GetPageDec(where, orderBy, pageIndex, pageSize);
        }


        public List<T> GetPage<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, ref int Count)
        {
            return repository.GetPage(where, orderBy, pageIndex, pageSize, ref Count);
        }
        //属性赋值
        public void BindValue(object obj1, object obj2, string[] noparam)
        {
            Type t = obj2.GetType();
            foreach (PropertyInfo p in t.GetProperties())
            {
                if (noparam.Contains(p.Name))
                {
                    continue;
                }
                obj1.GetType().GetProperty(p.Name).SetValue(obj1, p.GetValue(obj2, null), null);
            }
        }

        /// <summary>
        /// 3.0 根据条件删除
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns></returns>
        public int DelBy(Expression<Func<T, bool>> delWhere)
        {
            return repository.DelBy(delWhere);
        }

         /// <summary>
        /// 6.0 分页查询 + List<T> GetPagedList<TKey>
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件 lambda表达式</param>
        /// <param name="orderBy">排序 lambda表达式</param>
        /// <param name="type">排序方式，1：升序、2：降序</param>
        /// <returns></returns>
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, int isDesc, Expression<Func<T, TKey>> thenBy, int isThenDesc)
        {
            return repository.GetPagedList(pageIndex, pageSize, whereLambda, orderBy, isDesc, thenBy, isThenDesc);
        }
           
        /// <summary>
        /// 根据LIST和一页几个ITEM获取分页总数
        /// </summary>
        /// <param name="PageList"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public int GetPageCount(List<T> PageList, int PageCount)
        {
            return repository.GetPageCount(PageList, PageCount);
        }

        /// <summary>
        /// 根据LIST和时间格式转换
        /// </summary>
        /// <param name="PageList"></param>
        /// <param name="DateFormat"></param>
        /// <returns></returns>
        public string GetJsoninfo(List<T> PageList, string DateFormat = "")
        {
            return repository.GetJsoninfo(PageList, DateFormat);
        }
          /// <summary>
        /// 避免循环嵌套转换
        /// </summary>
        /// <param name="PageList"></param>
        /// <returns></returns>
        public string GetJsoninfoNoFor(List<T> PageList)
        {
            return repository.GetJsoninfoNoFor(PageList);

        }
    }
}

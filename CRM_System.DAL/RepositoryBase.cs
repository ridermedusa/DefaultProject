using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Linq.Expressions;
using CRM_System.Model;
using System.Reflection;
using System.Data.Entity.Validation;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using PagedList;

namespace CRM_System.DAL
{
    public abstract class RepositoryBase<T> where T : class, new()
    {

        public CRM_SystemEntities context;

        //提供IOC注入方式接口
        public RepositoryBase(CRM_SystemEntities context)
        {
            this.context = context;
        }

        //测试用
        public RepositoryBase()
        {
            this.context = EFContextFactory.GetCurrentDbContext();
        }

        #region IRepository<T> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Create()
        {
            return context.Set<T>().Create();
        }
        /// <summary>
        /// 更新成员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Update(T entity)
        {
            try
            {
                context.Set(entity.GetType()).Attach(entity);
                context.Entry<T>(entity).State = System.Data.EntityState.Modified;
                context.SaveChanges();
                //执行验证业务
                //context.Entry<T>(entity).GetValidationResult();
                /* if (context.Entry<T>(entity).State == EntityState.Modified)
                     try
                     {
                         context.SaveChanges();
                     }
                     catch (Exception ex)
                     {

                     }*/
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 更新成员：遍历字段类型
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateE(T entity)
        {
            PropertyInfo[] pros = typeof(T).GetProperties();
            List<string> props = new List<string>();
            foreach (var item in pros)
            {
                if (item.Name != "ID")
                    props.Add(item.Name);
            }
            Update(entity, props.ToArray());
        }
        /// <summary>
        /// 更新成员 按照字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="param"></param>
        public void Update(T entity, string[] param)
        {
            context.Set<T>().Attach(entity);
            var entry = context.Entry(entity);
            foreach (string item in param)
            {
                entry.Property(item).IsModified = true;
            }
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 向数据库内添加内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Insert(T entity)
        {
            context.Set<T>().Add(entity);
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return entity;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }
        public T Find(params object[] keyValues)
        {
            return context.Set<T>().Find(keyValues);
        }
        public T GetModel(Expression<Func<T, bool>> where)
        {
            return context.Set<T>().Where(where).FirstOrDefault();
        }
        public List<T> FindAll()
        {
            return context.Set<T>().ToList();
        }
        public List<T> FindByParam(Expression<Func<T, bool>> where)
        {
            string sqlyj = context.Set<T>().Where(where).ToString();
            return context.Set<T>().Where(where).ToList();
        }
        public List<T> FindByParam<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy)
        {
            return context.Set<T>().Where(where).OrderBy(orderBy).ToList();
        }
        public List<T> FindByParamDesc<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy)
        {
            return context.Set<T>().Where(where).OrderByDescending(orderBy).ToList();
        }
        public List<T> FindByParam(Expression<Func<T, bool>> where, string orderBy, bool desc, int count)
        {
            //return context.Set<T>().Where(where).OrderBy(orderBy, desc).Take(count).ToList();
            return null;
        }

        public List<T> GetPage<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize)
        {
            return context.Set<T>().Where(where).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<T> GetPageDec<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize)
        {
            return context.Set<T>().Where(where).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public object GetModelList(string sql)
        {
            ComSQLRepository comsql = new ComSQLRepository();
            object o  = comsql.GetSingle(sql.ToString());
            return o;
        }

        public List<T> GetPage<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, ref int Count)
        {
            var q = context.Set<T>().Where(where).OrderByDescending(orderBy);
            Count = q.Count();
            return q.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public object GetMax<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> MaxCoum)
        {
            return context.Set<T>().Where(where).Select(MaxCoum).Max();
        }
        /// <summary>
        /// 3.0 根据条件删除
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns></returns>
        public int DelBy(Expression<Func<T, bool>> delWhere)
        {
            //3.1查询要删除的数据
            List<T> listDeleting = context.Set<T>().Where(delWhere).ToList();
            //3.2将要删除的数据 用删除方法添加到 EF 容器中
            listDeleting.ForEach(u =>
            {
                context.Set<T>().Attach(u);//先附加到 EF容器
                context.Set<T>().Remove(u);//标识为 删除 状态
            });
            //3.3一次性 生成sql语句到数据库执行删除
            return context.SaveChanges();
        }

        #region 6.0 分页查询 + List<T> GetPagedList<TKey>
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
            if (isDesc == 1)
            {
                if (isThenDesc == 1)
                {
                    return context.Set<T>().Where(whereLambda).OrderBy(orderBy).ThenBy(thenBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return context.Set<T>().Where(whereLambda).OrderBy(orderBy).ThenByDescending(thenBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            else
            {
                if (isThenDesc == 1)
                {
                    return context.Set<T>().Where(whereLambda).OrderByDescending(orderBy).ThenBy(thenBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return context.Set<T>().Where(whereLambda).OrderByDescending(orderBy).ThenByDescending(thenBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
        }
        #endregion

        /// <summary>
        /// 根据LIST和一页几个ITEM获取分页总数
        /// </summary>
        /// <param name="PageList"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public int GetPageCount(List<T> PageList, int PageCount)
        {
            return Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(PageList.Count()) / PageCount));
        }
        /// <summary>
        /// 根据LIST和时间格式转换
        /// </summary>
        /// <param name="PageList"></param>
        /// <param name="DateFormat"></param>
        /// <returns></returns>
        public string GetJsoninfo(List<T> PageList, string DateFormat = "")
        {
            if (!string.IsNullOrEmpty(DateFormat))
            {
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = DateFormat;
                return JsonConvert.SerializeObject(PageList, Newtonsoft.Json.Formatting.Indented, timeFormat);
            }
            else
            {
                return JsonConvert.SerializeObject(PageList);
            }
        }
        /// <summary>
        /// 避免循环嵌套转换
        /// </summary>
        /// <param name="PageList"></param>
        /// <returns></returns>
        public string GetJsoninfoNoFor(List<T> PageList)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return JsonConvert.SerializeObject(PageList, jsSettings);
        }
        #endregion

    }
}

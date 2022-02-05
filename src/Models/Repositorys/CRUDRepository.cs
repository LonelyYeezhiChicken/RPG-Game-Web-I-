using Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Models.Repositorys
{
    public interface ICRUD<TEntity>
    {
        /// <summary>
        /// 讀取所有資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IList<TEntity> Load();
        /// <summary>
        /// 依照條件做查詢
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<TEntity> Read(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 建立單筆
        /// </summary>
        /// <param name="entity"></param>
        void Create(TEntity entity);
        /// <summary>
        /// 單筆更新
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        /// <summary>
        /// 刪除單筆
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
    }

    public class CRUDRepository<TEntity> : ICRUD<TEntity> where TEntity : class
    {
        /// <summary>
        /// 讀取所有資料
        /// </summary>
        /// <returns></returns>
        public IList<TEntity> Load()
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                return db.Set<TEntity>().ToList();
            }
        }
        /// <summary>
        /// 依照條件做查詢
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IList<TEntity> Read(Expression<Func<TEntity, bool>> predicate)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                return db.Set<TEntity>().Where(predicate).ToList();
            }
        }
        /// <summary>
        /// 新增一筆
        /// </summary>
        /// <param name="entity"></param>
        public void Create(TEntity entity)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    db.Set<TEntity>().Add(entity);
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 更新一筆Entity內容。
        /// </summary>
        /// <param name="entity">要更新的內容</param>
        public void Update(TEntity entity)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    db.Entry<TEntity>(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 刪除一筆
        /// </summary>
        /// <param name="entity">要被刪除的Entity。</param>
        public void Delete(TEntity entity)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    db.Entry<TEntity>(entity).State = EntityState.Deleted;
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
    }
}

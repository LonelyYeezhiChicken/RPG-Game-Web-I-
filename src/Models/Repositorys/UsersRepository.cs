using Models.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Models.Repositorys
{
    public interface IRoles
    {
        /// <summary>
        /// 取得God權限以外的使用者
        /// </summary>
        /// <returns></returns>
        IList<AspNetUsers> Load();
        /// <summary>
        ///  由取得Email取得角色
        /// </summary>
        /// <param name="email">信箱</param>
        /// <returns></returns>
        AspNetUsers LoadUserByEmail(string email);
        /// <summary>
        /// 查詢 User 權限
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="Key2"></param>
        /// <returns></returns>
        IList<T> GetUserRoles<T>(string account);
        /// <summary>
        /// 檢查角色是否存在
        /// </summary>
        /// <param name="userRole">權限角色</param>
        /// <returns>是否存在(存在回復true)</returns>
        bool CheckUserRole(string userRole);
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="entity"></param>
        void AddUserRole(AspNetRoles entity);
        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="entity"></param>
        void EditUserRole(AspNetRoles uuid);
        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userRoleId"></param>
        void AddUser(AspNetUsers entity, string[] userRoleId);
        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="entity"></param>
        void EditUser(AspNetUsers entity, string[] userRoleId);
        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="uuid">uuid</param>
        /// <param name="passwordHash">密碼</param>
        /// <param name="securityStamp">安全碼</param>
        void ResetPasword(string uuid, string passwordHash, string securityStamp);
    }

    public class UsersRepository : IRoles
    {
        /// <summary>
        /// 取得God權限以外的使用者
        /// </summary>
        /// <returns></returns>
        public IList<AspNetUsers> Load()
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                return db.AspNetUsers
                .Include("AspNetRoles")
                .Include("AspNetUserClaims")
                .Include("AspNetUserLogins")
                .Where(x => x.AspNetRoles.Where(y => y.Name != "God").Any()).ToList();
            }
        }
        /// <summary>
        ///  由取得Email取得角色
        /// </summary>
        /// <param name="email">信箱</param>
        /// <returns></returns>
        public AspNetUsers LoadUserByEmail(string email)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                return db.AspNetUsers
                .Include("AspNetRoles")
                .Include("AspNetUserClaims")
                .Include("AspNetUserLogins")
                .Where(x => x.AspNetRoles.Where(y => y.Name != "God").Any() && x.Email == email).FirstOrDefault();
            }
        }
        /// <summary>
        /// 查詢 User 權限
        /// </summary>
        /// <param name="key1"></param>
        /// <returns></returns>
        public IList<T> GetUserRoles<T>(string account)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                return db.AspNetUsers
                .Include("AspNetRoles")
                .Include("AspNetUserClaims")
                .Include("AspNetUserLogins")
                .Where(x => x.Email == account)
                .Cast<T>().ToList();
            }
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="entity"></param>
        public void AddUserRole(AspNetRoles entity)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    db.Set<AspNetRoles>().Add(entity);
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 檢查角色是否存在
        /// </summary>
        /// <param name="userRole">權限角色</param>
        /// <returns>是否存在(存在回復true)</returns>
        public bool CheckUserRole(string userRole)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                return db.Set<AspNetRoles>()
                    .Where(x => string.Compare(x.Name, userRole, true) == 0).Any();
            }
        }
        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="entity"></param>
        public void EditUserRole(AspNetRoles entity)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    db.Entry<AspNetRoles>(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userRoleId"></param>
        public void AddUser(AspNetUsers entity, string[] userRoleId)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    foreach (var id in userRoleId)
                    {
                        entity.AspNetRoles.Add(db.AspNetRoles.Find(id));
                    }
                    db.AspNetUsers.Add(entity);
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="entity"></param>
        public void EditUser(AspNetUsers entity, string[] userRoleId)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {

                    db.Entry(entity).State = EntityState.Modified;

                    var aspNetRoles = db.AspNetUsers
                                     .Include("AspNetRoles")
                                     .First(x => x.Id == entity.Id);
                    aspNetRoles.AspNetRoles = new List<AspNetRoles>();
                    foreach (var id in userRoleId)
                    {
                        aspNetRoles.AspNetRoles.Add(db.AspNetRoles.Find(id));
                    }
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="uuid">uuid</param>
        /// <param name="passwordHash">密碼</param>
        /// <param name="securityStamp">安全碼</param>
        public void ResetPasword(string uuid, string passwordHash, string securityStamp)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    var entity = db.AspNetUsers.Find(uuid);
                    entity.PasswordHash = passwordHash;
                    entity.SecurityStamp = securityStamp;
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
    }
}

using Models.Dtos;
using Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Models.Repositorys
{
    public interface IMessageBoardRepository
    {
        /// <summary>
        /// 建立留言板文章
        /// </summary>
        /// <param name="entity"></param>
        void Create(MessageBoard entity);
        /// <summary>
        /// 已讀
        /// </summary>
        /// <param name="id">文章id</param>
        /// <param name="readerId">讀者id</param>
        void Read(int id, string readerId, DateTime readTime);
        /// <summary>
        /// 批量軟刪除
        /// </summary>
        /// <param name="idList">需刪除的id列表</param>
        void Disable(List<int> idList);
        /// <summary>
        /// 批量刪除
        /// </summary>
        /// <param name="idList">需刪除的id列表</param>
        void Delete(List<int> idList);
        /// <summary>
        /// 使用狀態
        /// 取得資料列表
        /// </summary>
        /// <param name="status">狀態(是否已讀)</param>
        /// <param name="pageNo">第幾頁</param>
        /// <param name="count">取出筆數</param>
        /// <param name="totalPage">總頁數</param>
        /// <returns>回傳不包含內文</returns>
        List<MessageBoardDto> LoadByStatus(bool isRead, int pageNo, int count);
        /// <summary>
        /// 取得單筆資料
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        MessageBoard GetMessageBoardById(int id);
    }

    public class MessageBoardRepository : IMessageBoardRepository
    {
        /// <summary>
        /// 建立留言板文章
        /// </summary>
        /// <param name="entity"></param>
        public void Create(MessageBoard entity)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    db.Set<MessageBoard>().Add(entity);
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 已讀
        /// </summary>
        /// <param name="id">文章id</param>
        /// <param name="readerId">讀者id</param>
        public void Read(int id, string readerId, DateTime readTime)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    var editData = db.MessageBoard.Find(id);
                    editData.ReaderId = readerId;
                    editData.IsRead = true;
                    editData.ReadTime = readTime;
                    db.SaveChanges();
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 批量軟刪除
        /// </summary>
        /// <param name="idList">需刪除的id列表</param>
        public void Disable(List<int> idList)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    StringBuilder comm = new StringBuilder();
                    comm.Append($@"Update [MessageBoard] Set [IsEnable] = 1 Where ");
                    foreach (int dataId in idList)
                    {
                        comm.Append($@"[Id] = {dataId} OR ");
                    }
                    comm.Remove(comm.Length - 3, 3);
                    db.Database.ExecuteSqlCommand(comm.ToString());
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 批量刪除
        /// </summary>
        /// <param name="idList">需刪除的id列表</param>
        public void Delete(List<int> idList)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    StringBuilder comm = new StringBuilder();
                    comm.Append($@"DELETE FROM [MessageBoard] Where ");
                    foreach (int dataId in idList)
                    {
                        comm.Append($@"[Id] = {dataId} OR ");
                    }
                    comm.Remove(comm.Length - 3, 3);
                    db.Database.ExecuteSqlCommand(comm.ToString());
                    tx.Complete();
                }
            }
        }
        /// <summary>
        /// 使用狀態
        /// 取得資料列表
        /// </summary>
        /// <param name="status">狀態(是否已讀)</param>
        /// <param name="pageNo">第幾頁</param>
        /// <param name="count">取出筆數</param>
        /// <returns>回傳不包含內文</returns>
        public List<MessageBoardDto> LoadByStatus(bool isRead, int pageNo, int count)
        {
            List<MessageBoardDto> boardDtos = new List<MessageBoardDto>();
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                IQueryable<MessageBoard> query = db.MessageBoard.Where(x => x.IsEnable == true && x.IsRead == isRead);

                //回傳總筆數
                int totalCount = query.Count();
                int totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalCount) / count));

                var datas = query.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Title,
                    x.CreatTime,
                    x.IsRead,
                    x.ReadTime,
                    x.IsEnable
                }).OrderByDescending(x => x.CreatTime).Skip((pageNo - 1) * count).Take(count).ToList();

                foreach (var data in datas)
                {
                    boardDtos.Add(new MessageBoardDto
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Title = data.Title,
                        CreatTime = data.CreatTime,
                        IsRead = data.IsRead,
                        ReadTime = data.ReadTime == null ? DateTime.Now : (DateTime)data.ReadTime,
                        TotalPage = totalPage
                    });
                }
            }
            return boardDtos;
        }
        /// <summary>
        /// 取得單筆資料
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public MessageBoard GetMessageBoardById(int id)
        {
            using (AdIdentityEntities db = new AdIdentityEntities())
            {
                return db.MessageBoard.Find(id);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Cashes
{
    public interface ICaheDto
    {
        /// <summary>
        /// 存入
        /// </summary>
        /// <param name="data"></param>
        void Set(string data);
        /// <summary>
        /// 取出
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string Get(string name);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        void Add(string data);
    }

    public class CaheDto
    {
    }
}

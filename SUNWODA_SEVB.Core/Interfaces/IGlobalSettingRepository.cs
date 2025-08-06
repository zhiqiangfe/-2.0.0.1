using SUNWODA_SEVB.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Core.Interfaces
{
    public interface IGlobalSettingRepository : IRepository<GlobalSettingModel>
    {
        /// <summary>
        /// 通过ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GlobalSettingModel?> GetByIDAsync(int id);

        /// <summary>
        /// 通过变量名称获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<GlobalSettingModel?> GetByNameAsync(string name);

        /// <summary>
        /// 通过变量名称获取配置值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<dynamic?> GetSettingValueAsync(string name);

        /// <summary>
        /// 更新配置值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> UpdateSettingValueAsync(string name, dynamic value);

        /// <summary>
        /// 字符串值转指定类型值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        //dynamic StringToAny(string type, string value);
    }
}

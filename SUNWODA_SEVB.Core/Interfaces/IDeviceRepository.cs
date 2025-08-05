using SUNWODA_SEVB.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Core.Interfaces
{
    public interface IDeviceRepository : IRepository<DeviceModel>
    {
        /// <summary>
        /// 通过ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceModel?> GetByIDAsync(int id);

        /// <summary>
        /// 通过设备编码获取
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<List<DeviceModel>> GetByNumberAsync(string number);

        /// <summary>
        /// 通过设备名称获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<DeviceModel>> GetByNameAsync(string name);

        /// <summary>
        /// 通过基地名称获取
        /// </summary>
        /// <param name="baseName"></param>
        /// <returns></returns>
        Task<List<DeviceModel>> GetByBaseNameAsync(string baseName);

        /// <summary>
        /// 通过拉线名称获取
        /// </summary>
        /// <param name="lineName"></param>
        /// <returns></returns>
        Task<List<DeviceModel>> GetByLineNameAsync(string lineName);
    }
}

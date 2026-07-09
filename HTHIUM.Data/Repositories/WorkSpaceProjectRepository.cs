using SqlSugar;
using HTHIUM.Core.Interfaces.Data;
using HTHIUM.Core.Models.Data;
using HTHIUM.Data.Models;

namespace HTHIUM.Data.Repositories
{
    public class WorkSpaceProjectRepository : MappingRepository<WorkSpaceProjectModel, WorkSpaceProject>, IWorkSpaceProjectRepository
    {
        public WorkSpaceProjectRepository(ISqlSugarClient db) : base(db) { }

        public async Task<WorkSpaceProjectModel?> GetByIDAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public WorkSpaceProjectModel? GetByID(int id)
        {
            return GetById(id);
        }

        public async Task<WorkSpaceProjectModel?> GetByVMClassNameAsync(string vmClassName)
        {
            return await GetAsync(model => model.VMClassName == vmClassName);
        }

        public WorkSpaceProjectModel? GetByVMClassName(string vmClassName)
        {
            return Get(model => model.VMClassName == vmClassName);
        }

        public async Task<bool> GetIsEnabledAsync(string vmClassName)
        {
            var model = await GetByVMClassNameAsync(vmClassName);
            return model != null ? model.IsEnabled : false;
        }

        public bool GetIsEnabled(string vmClassName)
        {
            var model = GetByVMClassName(vmClassName);
            return model != null ? model.IsEnabled : false;
        }

        public async Task<bool> UpdateIsEnabledAsync(string vmClassName, bool isEnabled)
        {
            var model = await GetByVMClassNameAsync(vmClassName);
            if (model != null)
            {
                model.IsEnabled = isEnabled;
                return await UpdateAsync(model);
            }
            else
            {
                return false;
            }
        }

        public bool UpdateIsEnabled(string vmClassName, bool isEnabled)
        {
            var model = GetByVMClassName(vmClassName);
            if (model != null)
            {
                model.IsEnabled = isEnabled;
                return Update(model);
            }
            else
            {
                return false;
            }
        }
    }
}

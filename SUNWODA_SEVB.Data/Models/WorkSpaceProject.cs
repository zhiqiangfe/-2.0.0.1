using SqlSugar;

namespace SUNWODA_SEVB.Data.Models
{
    //[SugarIndex("workspace_project_vm_class_name", nameof(VMClassName), OrderByType.Desc, true)]
    [SugarTable("workspace_project", "应用表")]
    public class WorkSpaceProject
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        [SugarColumn(ColumnName = "vm_class_name", ColumnDescription = "视图模型类名")]
        public string VMClassName { get; set; } = null!;

        [SugarColumn(ColumnName = "is_enabled", ColumnDescription = "是否启用")]
        public bool IsEnabled { get; set; }

        [SugarColumn(ColumnName = "is_init_show", ColumnDescription = "是否初始化显示")]
        public bool IsInitShow { get; set; }

    }
}

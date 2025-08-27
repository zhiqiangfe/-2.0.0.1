using SqlSugar;

namespace SUNWODA_SEVB.Data.Models
{
    //[SugarIndex("global_setting_name", nameof(Name), OrderByType.Desc)]
    [SugarTable("global_setting", "全局配置表")]
    public class GlobalSetting
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [SugarColumn(ColumnName = "name", ColumnDescription = "变量名称")]
        public string Name { get; set; } = null!;
        [SugarColumn(ColumnName = "value", ColumnDescription = "值")]
        public string Value { get; set; } = null!;
        [SugarColumn(ColumnName = "type", ColumnDescription = "变量类型")]
        public string Type { get; set; } = null!;
        [SugarColumn(ColumnName = "unit", ColumnDescription = "单位", IsNullable = true)]
        public string Unit { get; set; } = null!;
        [SugarColumn(ColumnName = "role_rank", ColumnDescription = "用户权限")]
        public int RoleRank { get; set; }
        [SugarColumn(ColumnName = "remark", ColumnDescription = "注释", IsNullable = true)]
        public string Remark { get; set; } = null!;

    }
}

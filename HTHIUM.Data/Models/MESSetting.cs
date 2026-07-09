using SqlSugar;

namespace HTHIUM.Data.Models
{
    [SugarTable("mes_setting", "MES配置表")]
    public class MESSetting
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        /// <summary>
        /// 配置方案名称，用于区分不同的MES系统或环境（如：生产线A的MES, 测试环境MES）
        /// </summary>
        [SugarColumn(ColumnName = "profile_name", ColumnDescription = "配置方案名称")]
        public string ProfileName { get; set; } = null!;

        /// <summary>
        /// 配置项的键 (e.g., "BaseUrl", "ApiKey", "Timeout")
        /// </summary>
        [SugarColumn(ColumnName = "key", ColumnDescription = "配置键")]
        public string Key { get; set; } = null!;

        [SugarColumn(ColumnName = "value", ColumnDescription = "值")]
        public string Value { get; set; } = null!;

        [SugarColumn(ColumnName = "type", ColumnDescription = "变量类型")]
        public string Type { get; set; } = null!;

        [SugarColumn(ColumnName = "remark", ColumnDescription = "注释", IsNullable = true)]
        public string Remark { get; set; } = null!;
    }
}

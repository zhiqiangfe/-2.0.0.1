using SqlSugar;

namespace SUNWODA_SEVB.Data.Models
{
    //[SugarIndex("plc_address_config_parameter_name", nameof(ParameterName), OrderByType.Desc, true)]
    [SugarTable("plc_address_config", "PLC读写地址配置表")]
    public class PLCAddressConfig
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [SugarColumn(ColumnName = "plc_id", ColumnDescription = "Plc ID")]
        public int PLCID { get; set; }
        [SugarColumn(ColumnName = "plc_rw_id", ColumnDescription = "地址段ID")]
        public int PLCRWID { get; set; }
        [SugarColumn(ColumnName = "category_id", ColumnDescription = "类别ID")]
        public int CategoryID { get; set; }
        [SugarColumn(ColumnName = "parameter_name", ColumnDescription = "参数名称")]
        public string ParameterName { get; set; } = null!;
        [SugarColumn(ColumnName = "type", ColumnDescription = "数据类型")]
        public string Type { get; set; } = null!;
        [SugarColumn(ColumnName = "length", ColumnDescription = "数据长度")]
        public ushort Length { get; set; }
        [SugarColumn(ColumnName = "adress", ColumnDescription = "地址")]
        public string Address { get; set; } = null!;
        [SugarColumn(ColumnName = "uint", ColumnDescription = "单位", IsNullable = true)]
        public string Unit { get; set; } = null!;
        [SugarColumn(ColumnName = "remark", ColumnDescription = "注释", IsNullable = true)]
        public string Remark { get; set; } = null!;
        [SugarColumn(ColumnName = "is_monitor", ColumnDescription = "是否监测")]
        public bool IsMonitor { get; set; }

    }
}

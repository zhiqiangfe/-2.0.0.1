using SqlSugar;

namespace HTHIUM.Data.Models
{
    // 2. 使用 [SugarTable] 标记此类对应的数据表名
    //[SugarIndex("plc_rw_config_name", nameof(Name), OrderByType.Desc, true)]
    [SugarTable("plc_rw_config", "PLC读写区域设置")]
    public class PLCRWConfig
    {       
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        [SugarColumn(ColumnName = "name", ColumnDescription = "地址段名称")]
        public string Name { get; set; } = null!;

        [SugarColumn(ColumnName = "plc_id", ColumnDescription = "Plc ID")]
        public int PLCID { get; set; }

        [SugarColumn(ColumnName = "area_name", ColumnDescription = "区域名称")]
        public string AreaName { get; set; } = null!;

        [SugarColumn(ColumnName = "start_address", ColumnDescription = "起始地址")]
        public string StartAddress { get; set; } = null!;

        [SugarColumn(ColumnName = "length", ColumnDescription = "地址段长度")]
        public int Length { get; set; }

        [SugarColumn(ColumnName = "rw_mode", ColumnDescription = "读写类型")]
        public string RWMode { get; set; } = null!;

        [SugarColumn(ColumnName = "cycle", ColumnDescription = "读写周期(ms)")]
        public int Cycle { get; set; }

        [SugarColumn(ColumnName = "address_type", ColumnDescription = "区分标签还是地址块", DefaultValue = "1")]
        public int AddressType { get; set; }

        [SugarColumn(ColumnName = "is_enable", ColumnDescription = "是否启用地址段")]
        public bool IsEnable { get; set; }

    }
}

using SqlSugar;

namespace SUNWODA_SEVB.Data.Models
{
    //[SugarIndex("device_name", nameof(Name), OrderByType.Desc, true)]
    [SugarTable("device", "设备表")]
    public class Device
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [SugarColumn(ColumnName = "number", ColumnDescription = "设备编号")]
        public string Number { get; set; } = null!;
        [SugarColumn(ColumnName = "name", ColumnDescription = "设备名称")]
        public string Name { get; set; } = null!;
        [SugarColumn(ColumnName = "base_name", ColumnDescription = "所在基地名称")]
        public string BaseName { get; set; } = null!;
        [SugarColumn(ColumnName = "line_name", ColumnDescription = "所在拉线名称")]
        public string LineName { get; set; } = null!;
        [SugarColumn(ColumnName = "remark", ColumnDescription = "注释", IsNullable = true)]
        public string Remark { get; set; } = null!;
        //public Device() { }
        //public Device(string number, string name, string baseName, string lineName)
        //{
        //    Number = number;
        //    Name = name;
        //    BaseName = baseName;
        //    LineName = lineName;
        //}
    }
}

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.Models
{
    //[SugarIndex("global_setting_name", nameof(Name), OrderByType.Desc)]
    [SugarTable("global_setting", "全局配置表")]
    public class GlobalSettingModel
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
        [SugarColumn(ColumnName = "remark", ColumnDescription = "注释", IsNullable = true)]
        public string Remark { get; set; } = null!;
        //public GlobalSetting() { }
        //public GlobalSetting(string name, string value, string type)
        //{
        //    Name = name;
        //    Value = value;
        //    Type = type;
        //}
    }
}

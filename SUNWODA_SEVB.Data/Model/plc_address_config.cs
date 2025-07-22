using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// plc_address_config:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class plc_address_config
    {
        #region Model
        private int _id;
        private int _plc_config_id;
        private int _plc_rw_config_id;
        private int _category_id;
        private string? _parameter_name;
        private string? _type;
        private int _length;
        private string? _address;
        private string? _unit;
        private string? _remark;
        private int _is_monitor;

        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        public int plc_config_id
        {
            set { _plc_config_id = value; }
            get { return _plc_config_id; }
        }

        public int plc_rw_config_id
        {
            set { _plc_rw_config_id = value; }
            get { return _plc_rw_config_id; }
        }

        public int category_id
        {
            set { _category_id = value; }
            get { return _category_id; }
        }

        public string? parameter_name
        {
            set { _parameter_name = value; }
            get { return _parameter_name; }
        }

        public string? type
        {
            set { _type = value; }
            get { return _type; }
        }

        public int length
        {
            get { return _length; }
            set { _length = value; }
        }

        public string? address
        {
            set { _address = value; }
            get { return _address; }
        }

        public string? unit
        {
            set { _unit = value; }
            get { return _unit; }
        }

        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        public int is_monitor
        {
            set { _is_monitor = value; }
            get { return _is_monitor; }
        }
        #endregion
    }
}

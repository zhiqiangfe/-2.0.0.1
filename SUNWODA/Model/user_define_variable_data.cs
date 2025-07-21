using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// user_define_variable_data:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class user_define_variable_data
    {
        public user_define_variable_data() { }

        #region Model
        private int _variable_id;
        private string? _value_name;
        private string? _value_type;
        private string? _value;
        private string? _remark;
        private DateTime _data_time;

        /// <summary>
        ///
        /// </summary>
        public int variable_id
        {
            set { _variable_id = value; }
            get { return _variable_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? value_name
        {
            set { _value_name = value; }
            get { return _value_name; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? value_type
        {
            set { _value_type = value; }
            get { return _value_type; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? value
        {
            set { _value = value; }
            get { return _value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        /// on update current_timestamp()
        /// </summary>
        public DateTime data_time
        {
            set { _data_time = value; }
            get { return _data_time; }
        }
        #endregion Model
    }
}

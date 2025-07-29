using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// user_define_variable:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class user_define_variable
    {
        public user_define_variable() { }

        #region Model
        private int _id;
        private int _belong_id;
        private string? _variable_name;
        private string? _value;
        private string? _unit;
        private string? _description;
        private string? _value_type;
        private string? _remark;
        private DateTime _datatime;

        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        public int belong_id
        {
            set { _belong_id = value; }
            get { return _belong_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? variable_name
        {
            set { _variable_name = value; }
            get { return _variable_name; }
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
        public string? unit
        {
            set { _unit = value; }
            get { return _unit; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? description
        {
            set { _description = value; }
            get { return _description; }
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
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        /// on update current_timestamp()
        /// </summary>
        public DateTime datatime
        {
            set { _datatime = value; }
            get { return _datatime; }
        }
        #endregion Model
    }
}

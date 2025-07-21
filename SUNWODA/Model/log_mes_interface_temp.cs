using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// log_mes_interface_temp:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class log_mes_interface_temp
    {
        public log_mes_interface_temp() { }

        #region Model
        private int _id;
        private DateTime _logdate;
        private string? _method;
        private string? _input_json;
        private string? _output_json;
        private DateTime? _start_time;
        private DateTime? _end_time;
        private string? _consuming_time;
        private string? _success_flag;
        private int? _isdeal;

        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime logdate
        {
            set { _logdate = value; }
            get { return _logdate; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? method
        {
            set { _method = value; }
            get { return _method; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? input_json
        {
            set { _input_json = value; }
            get { return _input_json; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? output_json
        {
            set { _output_json = value; }
            get { return _output_json; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? start_time
        {
            set { _start_time = value; }
            get { return _start_time; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? end_time
        {
            set { _end_time = value; }
            get { return _end_time; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? consuming_time
        {
            set { _consuming_time = value; }
            get { return _consuming_time; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? success_flag
        {
            set { _success_flag = value; }
            get { return _success_flag; }
        }

        /// <summary>
        ///
        /// </summary>
        public int? isDeal
        {
            set { _isdeal = value; }
            get { return _isdeal; }
        }
        #endregion Model
    }
}

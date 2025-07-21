using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.Model
{
    [Serializable]
    public partial class log_web_interface
    {
        public log_web_interface() { }

        #region Model
        private int _id;
        private DateTime _logdate;
        private string? _method;
        private string? _input_json;
        private string? _output_json;
        private DateTime _start_time;
        private DateTime _end_time;
        private string? _consuming_time;

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
        [Description("记录时间")]
        public DateTime logdate
        {
            set { _logdate = value; }
            get { return _logdate; }
        }

        /// <summary>
        /// 入参
        /// </summary>
        [Description("方法名")]
        public string? method
        {
            set { _method = value; }
            get { return _method; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("入参")]
        public string? input_json
        {
            set { _input_json = value; }
            get { return _input_json; }
        }

        /// <summary>
        /// 回参
        /// </summary>
        [Description("出参")]
        public string? output_json
        {
            set { _output_json = value; }
            get { return _output_json; }
        }

        [Description("开始时间")]
        public DateTime start_time
        {
            set { _start_time = value; }
            get { return _start_time; }
        }

        [Description("结束时间")]
        public DateTime end_time
        {
            set { _end_time = value; }
            get { return _end_time; }
        }

        [Description("耗时(ms)")]
        public string? consuming_time
        {
            set { _consuming_time = value; }
            get { return _consuming_time; }
        }
        #endregion Model
    }
}

using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// log_mes_interface_paramvalue:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class log_mes_interface_paramvalue
    {
        public log_mes_interface_paramvalue() { }

        #region Model
        private int _id;
        private DateTime _logdate;
        private string? _success_flag;
        private string? _groupcode;
        private string? _operatorid;
        private string? _devicesn;
        private string? _productsn;
        private string? _monumber;
        private string? _testresult;
        private int? _paramcode;
        private string? _paramname;
        private string? _paramvalue;
        private string? _paramresult;
        private string? _paramunit;

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
        public string? success_flag
        {
            set { _success_flag = value; }
            get { return _success_flag; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? groupCode
        {
            set { _groupcode = value; }
            get { return _groupcode; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? operatorId
        {
            set { _operatorid = value; }
            get { return _operatorid; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? deviceSn
        {
            set { _devicesn = value; }
            get { return _devicesn; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? productSn
        {
            set { _productsn = value; }
            get { return _productsn; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? moNumber
        {
            set { _monumber = value; }
            get { return _monumber; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? testResult
        {
            set { _testresult = value; }
            get { return _testresult; }
        }

        /// <summary>
        ///
        /// </summary>
        public int? paramCode
        {
            set { _paramcode = value; }
            get { return _paramcode; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? paramName
        {
            set { _paramname = value; }
            get { return _paramname; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? paramValue
        {
            set { _paramvalue = value; }
            get { return _paramvalue; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? paramResult
        {
            set { _paramresult = value; }
            get { return _paramresult; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? paramUnit
        {
            set { _paramunit = value; }
            get { return _paramunit; }
        }
        #endregion Model
    }
}

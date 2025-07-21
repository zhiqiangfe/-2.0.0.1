using System;
using System.ComponentModel;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// log4net:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class log_operation
    {
        public log_operation() { }

        #region Model
        private int _id;
        private DateTime _logdate;
        private string? _operationMsg;
        private string? _userName;
        private string? _softwareName;

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
        ///
        /// </summary>
        [Description("操作信息")]
        public string? operationMsg
        {
            set { _operationMsg = value; }
            get { return _operationMsg; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("操作用户")]
        public string? userName
        {
            set { _userName = value; }
            get { return _userName; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("软件名称")]
        public string? softwareName
        {
            set { _softwareName = value; }
            get { return _softwareName; }
        }
        #endregion Model
    }
}

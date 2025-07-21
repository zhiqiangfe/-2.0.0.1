using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// input_variable_history:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class input_variable_history
    {
        public input_variable_history() { }

        #region Model
        private int _id;
        private string? _equipment_id;
        private string? _sendparamid;
        private string? _uploadparamid;
        private string? _paramname;
        private string? _type;
        private string? _paramvalueratio;
        private string? _model;
        private string? _historymaxvalue;
        private string? _historyminvalue;
        private string? _historystandardvalue;
        private string? _changemonitorvalue;
        private string? _actualvalue;
        private string? _bycelloutputvalue;
        private DateTime _datatime;
        private string? _logfrom = "download";
        private string? _downloadremark = "download";

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
        public string? equipment_id
        {
            set { _equipment_id = value; }
            get { return _equipment_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? SendParamID
        {
            set { _sendparamid = value; }
            get { return _sendparamid; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? UploadParamID
        {
            set { _uploadparamid = value; }
            get { return _uploadparamid; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? ParamName
        {
            set { _paramname = value; }
            get { return _paramname; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? Type
        {
            set { _type = value; }
            get { return _type; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? ParamValueRatio
        {
            set { _paramvalueratio = value; }
            get { return _paramvalueratio; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? Model
        {
            set { _model = value; }
            get { return _model; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? HistoryMaxValue
        {
            set { _historymaxvalue = value; }
            get { return _historymaxvalue; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? HistoryMinValue
        {
            set { _historyminvalue = value; }
            get { return _historyminvalue; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? HistoryStandardValue
        {
            set { _historystandardvalue = value; }
            get { return _historystandardvalue; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? ChangeMonitorValue
        {
            set { _changemonitorvalue = value; }
            get { return _changemonitorvalue; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? ActualValue
        {
            set { _actualvalue = value; }
            get { return _actualvalue; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? BycellOutputValue
        {
            set { _bycelloutputvalue = value; }
            get { return _bycelloutputvalue; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime DataTime
        {
            set { _datatime = value; }
            get { return _datatime; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? LogFrom
        {
            set { _logfrom = value; }
            get { return _logfrom; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? DownloadRemark
        {
            set { _downloadremark = value; }
            get { return _downloadremark; }
        }
        #endregion Model
    }
}

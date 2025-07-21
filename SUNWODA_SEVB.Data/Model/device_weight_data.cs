using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_weight_data:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_weight_data
    {
        public device_weight_data() { }

        #region Model
        private int _id;
        private int? _number;
        private string? _messagetag;
        private DateTime _creat_time;
        private string? _weight;
        private string? _sn;
        private string? _data;

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
        public int? number
        {
            set { _number = value; }
            get { return _number; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? messagetag
        {
            set { _messagetag = value; }
            get { return _messagetag; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime creat_time
        {
            set { _creat_time = value; }
            get { return _creat_time; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? weight
        {
            set { _weight = value; }
            get { return _weight; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? sn
        {
            set { _sn = value; }
            get { return _sn; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? data
        {
            set { _data = value; }
            get { return _data; }
        }
        #endregion Model
    }
}

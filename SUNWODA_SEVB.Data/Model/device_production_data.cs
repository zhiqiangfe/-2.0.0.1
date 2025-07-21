using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_production_data:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_production_data
    {
        public device_production_data() { }

        #region Model
        private int _id;
        private string? _sn1;
        private string? _sn2;
        private string? _sn3;
        private DateTime? _time1;
        private DateTime? _time2;
        private DateTime? _time3;
        private string? _data1;
        private string? _data2;
        private string? _data3;

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
        public string? sn1
        {
            set { _sn1 = value; }
            get { return _sn1; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? sn2
        {
            set { _sn2 = value; }
            get { return _sn2; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? sn3
        {
            set { _sn3 = value; }
            get { return _sn3; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? time1
        {
            set { _time1 = value; }
            get { return _time1; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? time2
        {
            set { _time2 = value; }
            get { return _time2; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? time3
        {
            set { _time3 = value; }
            get { return _time3; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? data1
        {
            set { _data1 = value; }
            get { return _data1; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? data2
        {
            set { _data2 = value; }
            get { return _data2; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? data3
        {
            set { _data3 = value; }
            get { return _data3; }
        }
        #endregion Model
    }
}

using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_md_data:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_md_data
    {
        public device_md_data() { }

        #region Model
        private int _id;
        private DateTime? _creat_time;
        private string? _md;
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
        public DateTime? creat_time
        {
            set { _creat_time = value; }
            get { return _creat_time; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? md
        {
            set { _md = value; }
            get { return _md; }
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

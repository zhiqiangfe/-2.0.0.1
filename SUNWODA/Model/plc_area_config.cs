using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// plc_area_config:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class plc_area_config
    {
        public plc_area_config() { }

        #region Model
        private string? _area_name;
        private int? _area_length;
        private string? _brand;

        /// <summary>
        ///
        /// </summary>
        public string? area_name
        {
            set { _area_name = value; }
            get { return _area_name; }
        }

        /// <summary>
        ///
        /// </summary>
        public int? area_length
        {
            set { _area_length = value; }
            get { return _area_length; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? brand
        {
            set { _brand = value; }
            get { return _brand; }
        }
        #endregion Model
    }
}

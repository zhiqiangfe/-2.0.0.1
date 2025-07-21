using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// plc_brand:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class plc_brand
    {
        public plc_brand() { }

        #region Model
        private string? _brand;

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

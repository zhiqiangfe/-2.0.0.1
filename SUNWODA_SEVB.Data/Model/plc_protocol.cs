/**  版本信息模板在安装目录下，可自行修改。
* plc_protocol.cs
*
* 功 能： N/A
* 类 名： plc_protocol
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2022-03-11 11:31:14   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// plc_protocol:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class plc_protocol
    {
        public plc_protocol() { }

        #region Model
        private string? _protocol_name;
        private string? _model;
        private string? _remark;

        /// <summary>
        ///
        /// </summary>
        public string? protocol_name
        {
            set { _protocol_name = value; }
            get { return _protocol_name; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? model
        {
            set { _model = value; }
            get { return _model; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
    }
}

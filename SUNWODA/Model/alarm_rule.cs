using System;
namespace SUNWODA_SEVB.Data.Model
{
	/// <summary>
	/// alarm_rule:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class alarm_rule
	{
		public alarm_rule()
		{}
		#region Model
		private string? _equipment_id;
		private string? _upload_param_id;
		private int _alarm_level_id=1;
		private string? _alarm_content;
		private int? _plc_rw_config_id;
		private int? _plc_address_int;
		private int? _plc_address_bit;
		/// <summary>
		/// 
		/// </summary>
		public string? equipment_id
		{
			set{ _equipment_id=value;}
			get{return _equipment_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string? upload_param_id
		{
			set{ _upload_param_id=value;}
			get{return _upload_param_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int alarm_level_id
		{
			set{ _alarm_level_id=value;}
			get{return _alarm_level_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string? alarm_content
		{
			set{ _alarm_content=value;}
			get{return _alarm_content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? plc_rw_config_id
		{
			set{ _plc_rw_config_id=value;}
			get{return _plc_rw_config_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? plc_address_int
		{
			set{ _plc_address_int=value;}
			get{return _plc_address_int;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? plc_address_bit
		{
			set{ _plc_address_bit=value;}
			get{return _plc_address_bit;}
		}
		#endregion Model

	}
}


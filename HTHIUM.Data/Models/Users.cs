using SqlSugar;

namespace HTHIUM.Data.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    //[SugarIndex("users_name", nameof(UserName), OrderByType.Asc, true)]
    [SugarTable("Users", "用户表")]
    public class Users
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        [SugarColumn(ColumnName = "user_account", ColumnDescription = "用户账号")]
        public string UserAccount { get; set; } = null!;

        [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名")]
        public string UserName { get; set; } = null!;

        [SugarColumn(ColumnName = "password", ColumnDescription = "密码（应存储哈希值）")]
        public string Password { get; set; } = null!;

        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID")]
        public int RoleId { get; set; }

        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", IsNullable = true)]
        public string? Remark { get; set; }

        [SugarColumn(ColumnName = "created_time", ColumnDescription = "创建时间", IsNullable = true)]
        public DateTime? CreatedTime { get; set; }

        [SugarColumn(ColumnName = "last_login_time", ColumnDescription = "最后登录时间", IsNullable = true)]
        public DateTime? LastLoginTime { get; set; }

    }
}

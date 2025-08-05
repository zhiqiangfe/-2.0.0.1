namespace SUNWODA_SEVB.Core.Models.Data
{
    /// <summary>
    /// 用户表
    /// </summary>

    public class UsersModel
    {
        public int ID { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int RoleId { get; set; }

        public string? Remark { get; set; }

        public DateTime? LastLoginTime { get; set; }
        public UsersModel() { }

        public UsersModel(string userName, string password, int roleId)
        {
            UserName = userName;
            Password = password;
            RoleId = roleId;
            LastLoginTime = DateTime.Now;
        }
    }
}

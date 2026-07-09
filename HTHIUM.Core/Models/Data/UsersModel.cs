namespace HTHIUM.Core.Models.Data
{
    /// <summary>
    /// 用户表
    /// </summary>

    public class UsersModel
    {
        public int ID { get; set; }

        public string UserAccount { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int RoleId { get; set; }

        public string? Remark { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? LastLoginTime { get; set; }
        public UsersModel() { }

        public UsersModel(string userAccount, string userName, string password, int roleId)
        {
            UserAccount = userAccount;
            UserName = userName;
            Password = password;
            RoleId = roleId;
            CreatedTime = DateTime.Now;
            LastLoginTime = DateTime.Now;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SUNWODA_SEVB.Core.Common;

namespace SUNWODA_SEVB.Core.Models
{
    public class UserManagementModel : ModelBase
    {
        private string? _account;
        private string? _name;
        private string? _role;
        private Brush? _roleColorBrush;
        private DateTime? _createdDateTime;
        private DateTime? _lastLoginDateTime;

        public string? Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string? Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        public Brush? RoleColorBrush
        {
            get => _roleColorBrush;
            set => SetProperty(ref _roleColorBrush, value);
        }

        public DateTime? CreatedDateTime
        {
            get => _createdDateTime;
            set => SetProperty(ref _createdDateTime, value);
        }

        public DateTime? LastLoginDateTime
        {
            get => _lastLoginDateTime;
            set => SetProperty(ref _lastLoginDateTime, value);
        }
    }
}

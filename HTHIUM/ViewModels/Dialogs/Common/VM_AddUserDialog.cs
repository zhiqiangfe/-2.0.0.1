using System.Collections.ObjectModel;
using System.Windows.Input;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using HTHIUM.Core.Common;
using HTHIUM.Core.Common.Commands;
using HTHIUM.Core.Enumerations;
using HTHIUM.Core.Models;

namespace HTHIUM.ViewModels.Dialogs.Common
{
    public class VM_AddUserDialog : DialogViewModelBase, IDialogResultable<UserManagementModel?>
    {
        private int _currentUserRoleID;
        public int CurrentUserRoleID
        {
            get => _currentUserRoleID;
            set
            {
                SetProperty(ref _currentUserRoleID, value);
                CanAddUserRoleList?.Clear();
                foreach (UserRole ur in Enum.GetValues(typeof(UserRole)))
                {
                    if ((int)ur < _currentUserRoleID)
                    {
                        CanAddUserRoleList?.Add(RoleIDToString((int)ur));
                    }
                }
            }
        }

        private UserManagementModel? _result = new UserManagementModel();
        public UserManagementModel? Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public ObservableCollection<string>? CanAddUserRoleList { get; set; } =
            new ObservableCollection<string>();

        public Action? CloseAction { get; set; }

        public ICommand CloseCommand =>
            new RelayCommand(() =>
            {
                Result = null;
                CloseAction?.Invoke();
            });

        public ICommand AddUserCommand => new RelayCommand(AddUser);

        private void AddUser()
        {
            if (Result != null)
            {
                if (Result.Account != null && Result.Name != null && Result.Role != null)
                {
                    Result.CreatedDateTime = DateTime.Now;
                    CloseAction?.Invoke();
                }
                else
                {
                    Growl.WarningGlobal("请完整填写表单！");
                }
            }
            else
            {
                CloseAction?.Invoke();
            }
        }

        private string RoleIDToString(int id)
        {
            return id switch
            {
                (int)UserRole.Guest => "访客",
                (int)UserRole.Engineer => "工程师",
                (int)UserRole.Admin => "管理员",
                (int)UserRole.SuperAdmin => "超级管理员",
                _ => "访客",
            };
        }
    }
}

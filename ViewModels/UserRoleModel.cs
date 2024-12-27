namespace TaskManagementSystem.ViewModels
{
    public class UserRoleModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleSelection> Roles { get; set; } = new List<RoleSelection>();
    }

    public class RoleSelection
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}

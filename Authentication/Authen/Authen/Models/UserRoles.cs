namespace Authen.Models
{
    public class UserRoles
    {
        public int UserId { set; get; }
        public int RoleId { set; get; }
        public Users Users { set; get; } = null!;
        public Roles Roles { set; get; } = null!;
    }
}

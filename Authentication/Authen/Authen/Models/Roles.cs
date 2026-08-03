namespace Authen.Models
{
    public class Roles
    {
        public int Id { set; get; }
        public string RoleName { set; get; }
        public List<UserRoles> UserRoles { get; } = [];
    }
}

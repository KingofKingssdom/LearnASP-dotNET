namespace Authen.Models
{
    public class Users
    {
        public int Id { set; get; }
        public string Email { set; get; }
        public string FullName { set; get; }
        public string Password { set; get; }
        public List<UserRoles> UserRoles { get; } = [];
    }
}

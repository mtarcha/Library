namespace Library.Domain
{
    public class Role : Enumeration<Role>
    {
        public const string UserRoleName = "User";
        public const string AdminRoleName = "Administrator";

        public static Role User = new Role(1, UserRoleName);
        public static Role Admin = new Role(1, AdminRoleName);

        private Role(int id, string name) : base(id, name)
        {
        }
    }
}
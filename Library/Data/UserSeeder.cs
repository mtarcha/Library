namespace Library.Data
{
    public class UserSeeder
    {
        private readonly UserContext _userContext;

        public UserSeeder(UserContext userContext)
        {
            _userContext = userContext;
        }

        public void Seed()
        {
            _userContext.Database.EnsureCreated();
        }
    }
}
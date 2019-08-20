using System.Threading.Tasks;
using Library.Presentation.MVC.Models;
using RestEase;

namespace Library.Presentation.MVC.Clients
{
    public interface IUsersClient
    {
        [Post("api/register")]
        Task<Response<User>> Register([Body] RegisterUserModel model);

        [Post("api/login")]
        Task<Response<User>> Login([Body] LoginUserModel model);

        [Post("api/logoff")]

        Task LogOff();
    }
}
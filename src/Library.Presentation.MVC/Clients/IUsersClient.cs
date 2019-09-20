using System;
using System.Threading.Tasks;
using Library.Presentation.MVC.Models;
using RestEase;

namespace Library.Presentation.MVC.Clients
{
    public interface IUsersClient
    {
        [Get("users/{id}")]
        Task<User> GetUser([Path("id")] Guid id);

        [Post("users/add")]
        Task AddUser([Body] AddUserModel model);
    }
}
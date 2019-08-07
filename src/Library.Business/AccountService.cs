using Library.Business.DTO;
using Library.Domain;

namespace Library.Business
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public AccountService(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public void Register(Registration model)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var user = _entityFactory.CreateUser(model.UserName, model.DateOfBirth, Role.User);
                user.SetPassword(model.Password);
                uow.Users.Create(user);
                uow.Users.TrySignIn(model.UserName, model.Password, false);
            }
        }

        public bool TrySignIn(Login model)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                return uow.Users.TrySignIn(model.UserName, model.Password, model.RememberMe);
            }
        }

        public void LogOff()
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                uow.Users.SignOut();
            }
        }
    }
}
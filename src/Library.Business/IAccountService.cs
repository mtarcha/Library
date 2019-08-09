using Library.Business.DTO;

namespace Library.Business
{
    public interface IAccountService
    {
        void Register(Registration model);

        bool TrySignIn(Login model);

        void LogOff();
    }
}
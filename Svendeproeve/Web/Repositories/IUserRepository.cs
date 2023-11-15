using Svendeproeve.Objects.DomainObjects;
using Svendeproeve.Objects.DTOObjects;
namespace Svendeproeve.Repositories
{
    public interface IUserRepository
    {
        public void SignUp(UserSignUpDTO signUpDTO);
        public User SignIn(string loginName, string password);
        public User FindByID(int ID);
        public void Update(int id, UserSettingsDTO user);
        public void Delete(string loginName, string password);
    }
}

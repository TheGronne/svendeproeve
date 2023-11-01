using Svendeproeve.DomainObjects;
using Svendeproeve.SQLRepo;
using Svendeproeve.DTOObjects;

namespace Svendeproeve.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ISQLProvider provider;
        public UserRepository(ISQLProvider _provider)
        {
            provider = _provider;

            if (provider == null)
                throw new ArgumentNullException(nameof(ISQLProvider));
        }

        public void SignUp(UserSignUpDTO signUpDTO)
            => provider.Create($"INSERT INTO Users (Username, LoginName, Password) VALUES ('{signUpDTO.Username}', '{signUpDTO.LoginName}', '{signUpDTO.Password}'); ");
        

        public User SignIn(string loginName, string password)
        {
            var reader = provider.Read($"SELECT ID, Username FROM Users WHERE LoginName = '{loginName}' AND Password = '{password}'");       
            return ConvertToUser(reader);
        }

        public User FindByID(int ID)
        {
            var reader = provider.Read($"SELECT ID, Username FROM Users WHERE ID = {ID}");
            return ConvertToUser(reader);
        }

        public User ConvertToUser(dynamic reader)
        {
            List<User> resultList = new List<User>();
            while (reader.Read())
            {
                User entity = new User();
                entity.ID = (int)reader["ID"];
                entity.Username = (string)reader["Username"];
                resultList.Add(entity);
            }

            return resultList[0];
        }

        public void Update(int id, UserSettingsDTO user)
            => provider.Update($"UPDATE Users SET Username = '{user.Username}', LoginName = '{user.NewLoginName}', Password = '{user.NewPassword}' WHERE ID = {id} AND LoginName = '{user.OldLoginName}' AND Password = '{user.OldPassword}'");

        public void Delete(string loginName, string password)
            => provider.Delete($"Delete FROM Users WHERE LoginName = '{loginName}' AND Password = '{password}'");
    }
}

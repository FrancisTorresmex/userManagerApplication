
using userManagerApplication.Models;


namespace userManagerApplication.Repository.Interfaces
{
    public interface IUsersRepository<User>: IGenericRepository<User>
    {
        List<UserModel> GetAllUserAndRoles();
    }
}

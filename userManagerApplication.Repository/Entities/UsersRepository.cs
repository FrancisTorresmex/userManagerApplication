
using userManagerApplication.Repository.Interfaces;
using userManagerAplication.Models.Data;
using userManagerApplication.Models;

namespace userManagerApplication.Repository.Entities
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository<User>
    {
        private UserManagerAplicationContext _context;

        public UsersRepository(UserManagerAplicationContext context) : base(context)
        {
            _context = context;
        }

        public List<UserModel> GetAllUserAndRoles()
        {
            var users = _context.Users.Select(x => new UserModel
            {
                IdUser = x.IdUser,
                Email = x.Email,
                LastName = x.LastName,
                Name = x.Name,
                Phone = x.Phone == null ? "No phone" : x.Phone,
                RoleName = x.IdRoleNavigation == null ? "No role" : x.IdRoleNavigation.Name,
                Status = x.Status == true ? "Active" : "Inactive",
                DateAdmision = x.DateAdmision,
                InactiveDate = x.InactiveDate
            }).ToList();

            return users;
        }



    }
   
}

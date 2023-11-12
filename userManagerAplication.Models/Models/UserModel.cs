using userManagerAplication.Models.Data;

namespace userManagerApplication.Models
{
    public class UserModel
    {
        public int IdUser { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }

        public string? StatusName { get; set; }

        public bool Status { get; set; }

        public DateTime? DateAdmision { get; set; }

        public DateTime? InactiveDate { get; set; }

        public int? IdRole { get; set; }
        public string? RoleName { get; set; }
        public List<UsersRole> AllRoleLst { get; set; }
    }
}

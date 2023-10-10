using Microsoft.AspNetCore.Mvc;
using userManagerAplication.Models.Data;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUsersRepository<User> _repository;
        public AdminController(IUsersRepository<User> repository) 
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            List<UserModel> users = _repository.GetAllUserAndRoles();            
            return View("Index", users);
        }

        [HttpGet]
        public IActionResult GetUserData(int id)
        {
            var user = _repository.Get(id);
            return Json(user);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody]UserModel user)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (user != null)
                {
                    var userOldData = _repository.Get(user.IdUser);
                    userOldData.Status = user.Status == "Active" ? true : false;
                    userOldData.Email = user.Email;
                    userOldData.IdRole = Convert.ToInt16(user.IdRole);
                    userOldData.Phone = user.Phone;
                    userOldData.Name = user.Name;
                    userOldData.LastName = user.LastName;

                    _repository.Update(userOldData);

                }
                _repository.Save();

                response.Message = "Data updated correctly";
                response.Success = true;                

                return Json(response);

            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "Error";
                response.Success = false;

                return Json(response);
            }
            
        }


    }
}

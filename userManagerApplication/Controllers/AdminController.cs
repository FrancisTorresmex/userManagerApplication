using Azure;
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
            ResponseModel response = new ResponseModel();
            try
            {
                var user = _repository.Get(id);
                if (user != null)
                {
                    response.Data = user;
                    response.Message = "Data updated correctly";
                    response.Success = true;
                }
                else
                {
                    response.Error = "User not found";
                    response.Message = "An error occurred";
                    response.Success = false;
                }
                
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "An error occurred";
                response.Success = false;
            }
            
            return Json(response);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody]UserModel user)
        {
            ResponseModel response = new ResponseModel();
            UserModel userUpdated = null;
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
                    _repository.Save();

                    //get user with updated data
                    userUpdated = _repository.GetUserAndRol(user.IdUser);
                }

                response.Data = userUpdated;
                response.Message = "Data updated correctly";
                response.Success = true;                

            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "An error occurred";
                response.Success = false;                
            }

            return Json(response);

        }

        [HttpPut]
        public IActionResult InactiveUser([FromBody] InactiveUserModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                _repository.DeactivateUser(model.IdUser, model.Status);
                _repository.Save();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "An error occurred";
                response.Success = false;
            }

            return Json(response);
        }
    }
}

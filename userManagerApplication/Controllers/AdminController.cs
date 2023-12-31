﻿using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using userManagerAplication.Models.Data;
using userManagerApplication.Indentity;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    //[Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [Authorize(Policy = "AdminPolicy")]

    public class AdminController : Controller
    {
        private readonly IUsersRepository<User> _repository;
        private readonly IConfiguration _configuration;

        public AdminController(IUsersRepository<User> repository, IConfiguration configuration) 
        {
            _configuration = configuration;
            _repository = repository;
        }

        
        public IActionResult Index()
        {

            //var token = HttpContext.Session.GetString("Token"); //Se descarta sesión por cookie
            string token = HttpContext.Request.Cookies["Token"];

            //if (token == null)
            //    return RedirectToAction("Index", "Access");

            //var tokenHelper = new TokenHelper(_configuration);
            //if (tokenHelper.ValidateTokenAccess(token)) 
            //{
            List<UserModel> users = _repository.GetAllUserAndRoles();
            return View(users);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Access");
            //}

        }


        [Authorize(Policy = "AdminPolicy")]
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
                    userOldData.Status = user.StatusName == "Active" ? true : false;
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

        [HttpPost]
        public IActionResult AddUser([FromBody] UserModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (model != null)
                {
                    var user = new User 
                    { 
                        DateAdmision = DateTime.Now,
                        Email = model.Email,
                        InactiveDate = null,
                        IdRole = model.IdRole,
                        Name = model.Name,
                        LastName = model.LastName,
                        Password = model.Password,
                        Phone = model.Phone,
                        Status = true
                    };
                    _repository.Add(user);
                    _repository.Save();

                    response.Data = user;
                    response.Message = "User created successfully";
                    response.Success = true;
                }
                else
                {
                    response.Error = "An error occurred when creating the user. Empty parameter";
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
    }
}

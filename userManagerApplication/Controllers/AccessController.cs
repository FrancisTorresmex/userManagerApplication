using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using userManagerAplication.Models.Data;
using userManagerApplication.Helper;
using userManagerApplication.Indentity;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    [AllowAnonymous]

    public class AccessController : Controller
    {
        private readonly IUsersRepository<User> _repository;
        private readonly IConfiguration _configuration;

        public AccessController(IUsersRepository<User> repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([FromBody]LoginModel model) 
        {
            string isAdmin = "false";
            var response = new ResponseModel();
            try
            {
                var user = _repository.FindByEmail(model.Email);
                if (user != null) 
                {
                    var encryption = new Encryption();
                    var userPassEncrypt = encryption.Encrypt(model.Password);
                    if (userPassEncrypt == user.Password && userPassEncrypt != string.Empty)
                    {
                        
                        if (user.IdRole == 1) //rol admin
                            isAdmin = "true";

                        //inactivated or active user
                        if (user.Status) 
                        {
                            var keyJWT = _configuration["keyJWT"];
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.UTF8.GetBytes(keyJWT);

                            //cuerpo del token jwt
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                //Calims (lo que llevara el token como datos de email, id etc)
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                //new Claim(ClaimTypes.Role, user.RoleName),
                                new Claim(IdentityData.AdminUserClaimName, isAdmin) //policy en lugar de role
                                }),
                                //Expires = DateTime.UtcNow.AddMonths(1), //expira en un mes al crearse
                                Expires = DateTime.UtcNow.AddMinutes(10),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                         SecurityAlgorithms.HmacSha256Signature)
                            };

                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            var tokenString = tokenHandler.WriteToken(token);


                            //HttpContext.Session.SetString("Token", tokenString); //se descarta forma con session por cookie

                            //con la cookie, no sera necesario agregar el token en cada petición fetch, el navegador se encarga de eso. 
                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true, //solo se enviará a través de una conexión HTTPS
                                Secure = true,   //prevenir el acceso a través de JavaScript
                                Expires = DateTime.Now.AddMinutes(15)
                            };

                            Response.Cookies.Append("Token", tokenString, cookieOptions);
                            Response.Cookies.Append("IdUser", user.IdUser.ToString(), cookieOptions); //save user id session

                            response.Success = true;

                            if (isAdmin == "true")
                            {
                                response.Data = "Admin/";
                                //return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                response.Data = "Home/";
                            }


                            //return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            response.Error = "Ups! Inactive user, contact your system administrator or send an email to admin@correo.com";
                            response.Message = "An error occurred";
                            response.Success = false;
                        }


                    }
                    else
                    {
                        response.Error = "Invalid credentials";
                        response.Message = "An error occurred";
                        response.Success = false;
                    }
                    
                }
                else
                {
                    response.Error = "Invalid credentials";
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

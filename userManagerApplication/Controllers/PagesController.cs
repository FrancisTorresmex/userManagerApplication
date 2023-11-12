using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using userManagerAplication.Models.Data;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    public class PagesController : Controller
    {
        private readonly IGenericRepository<AccessScreen> _repositoryAccesScr;
        private readonly IGenericRepository<Screen> _repositoryScr;

        public PagesController(IGenericRepository<AccessScreen> repositoryAccesScr, IGenericRepository<Screen> repositoryScr)
        {
            _repositoryAccesScr = repositoryAccesScr;
            _repositoryScr = repositoryScr;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get pages that the user has access
        [HttpGet]
        public IActionResult GetAllUserPages()
        {
            var response = new ResponseModel();

            try
            {
                int idUser = Convert.ToInt16(HttpContext.Request.Cookies["IdUser"]);                
                var userAccess = _repositoryAccesScr.Find(x => x.IdUser == idUser, "IdScreenNavigation").ToList();
                var screens = userAccess.Select(x => new ScreenModel
                {
                    Id = (int)x.IdScreen,
                    Title = x.IdScreenNavigation.Name,
                    URL = x.IdScreenNavigation.Url,                    
                });

                response.Data = screens;
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

        //Get all pages and get pages that the user has access
        [HttpGet]
        public IActionResult GetAllUserAndAccessPages(int idUser)
        {
            var response = new ResponseModel();

            try
            {
                var scr = _repositoryScr.GetAll();
                var userAccess = _repositoryAccesScr.Find(x => x.IdUser == idUser, "IdScreenNavigation").ToList();
                var screens = scr.Select(x => new ScreenUserModel
                {
                    Id = (int)x.IdScreen,
                    Title = x.Name,
                    URL = x.Url,
                    UserAccess = userAccess.Any(y => y.IdScreen == x.IdScreen)
                });

                response.Data = screens;
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

        //Get all pages
        [HttpGet]
        public IActionResult GetAllPages()
        {
            var response = new ResponseModel();

            try
            {
                int idUser = Convert.ToInt16(HttpContext.Request.Cookies["IdUser"]);
                var userAccess = _repositoryScr.GetAll();
                var screens = userAccess.Select(x => new ScreenModel
                {
                    Id = (int)x.IdScreen,
                    Title = x.Name
                });

                response.Data = screens;
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
        public IActionResult UpdateUserPages([FromBody]List<UserAccessPageModel> model)
        {
            var response = new ResponseModel();
            try
            {
                var idUser = model.Select(x => x.IdUser).FirstOrDefault();
                var userAccess = _repositoryAccesScr.Find(x => x.IdUser == idUser, "IdScreenNavigation").ToList();

                //remove all access
                _repositoryAccesScr.DeleteList(userAccess);

                //add new access, if notScreens is true, there are no screens to add to the user 
                var notScreens = model.Any(x => x.IdScreen == -1);
                if (!notScreens)
                {
                    var lstAccess = model.Select(x => new AccessScreen
                    {
                        IdScreen = x.IdScreen,
                        IdUser = x.IdUser,
                    });
                    _repositoryAccesScr.AddList(lstAccess);
                }
               

                
                response.Message = "User access successfully updated";
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

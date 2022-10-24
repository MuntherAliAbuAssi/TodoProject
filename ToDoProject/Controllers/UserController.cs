using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using TODOProject.Core.Managers.interfaces;
using TODOProject.ViewModel.Dto;
using TODOProject.ViewModel.ViewModels;

namespace TODOProject.Controllers
{
    [ApiController]
    [Authorize]
    public class UserController : ApiBaseController
    {
        private readonly IUserManager _userManager;
        private readonly ILogger<UserController> _looger;

        public UserController (IUserManager userManager, ILogger<UserController> looger)
        {
            _userManager = userManager;
            _looger = looger; 
        }

        [Route("api/user/signUp")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] CreateUserDto userReg)
        {
            var res = _userManager.SignUp(userReg);
            return Ok(res);
        }

        [Route("api/user/login")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto user)
        {
            var res = _userManager.Login(user);
            return Ok(res);
        }

        [Route("api/user/fileretrive/profilepic")]
        [HttpGet]
        public IActionResult Retrive(string filename)
        {
            var folderPath = Directory.GetCurrentDirectory();
            folderPath = $@"{folderPath}\{filename}";
            var byteArray = System.IO.File.ReadAllBytes(folderPath);
            return File(byteArray, "image/jpeg", filename);
        }

        [Route("api/user/UpdateProfile")]
        [HttpPut]     
        public IActionResult UpdateProfile(UserModel request)
        { 
            var user = _userManager.UpdateProfile(LoggedInUser,request);
            return Ok(user);
        } 
        [Route("api/user/Delete")]
        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            _userManager.Delete(LoggedInUser, id);
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using TODOProject.Extensions;
using TODOProject.Core.Managers.interfaces;
using TODOProject.ViewModel.ViewModels;

namespace TODOProject.Controllers
{
    [ApiController]
    public class ApiBaseController : Controller
    {
        private UserModel _loggedInUser;

        protected UserModel LoggedInUser
        {
            get
            {
                if (_loggedInUser != null)
                {
                    return _loggedInUser;
                }

                Request.Headers.TryGetValue("Authorization", out StringValues Token);
                
                if (string.IsNullOrWhiteSpace(Token))
                {
                    _loggedInUser = null;
                    return _loggedInUser;
                }

                var ClaimId = User.Claims.FirstOrDefault(c => c.Type == "Id");

                if (ClaimId == null || ! int.TryParse(ClaimId.Value,out int id))
                {
                    throw new ServiceValidationException(401, "Invalid or expired token");
                }
                
                var commomManger = HttpContext.RequestServices.GetService(typeof(ICommonManager)) as ICommonManager;
               
                _loggedInUser = commomManger.GetUserRole(new UserModel { Id = id });

                return _loggedInUser;
            }
        }
        public ApiBaseController()
        {

        }
    }
}

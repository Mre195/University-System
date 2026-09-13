using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UniversitySystem.Api.Controllers
{
    public class BaseApiController : ControllerBase
    {

        [NonAction]
        public Guid GetMyId()
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid id);
            return (id != Guid.Empty) ? id : Guid.Empty;
        }

        [NonAction]
        public string GetMyRole()
        {
            return User.FindFirstValue(ClaimTypes.Role)?.ToString() ?? "";
        }

        [NonAction]
        public Guid GetId()
        {
            return GetMyId();
        }

        //public UserId LoggedInUserId
        //{
        //    get
        //    {
        //        return new UserId(GetMyId());
        //    }
        //}
    }
}

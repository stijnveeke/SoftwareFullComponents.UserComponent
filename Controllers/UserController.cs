using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoftwareFullComponent.UserComponent;
using SoftwareFullComponent.UserComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserLogicInterface _userLogic;
        public UserController(UserLogicInterface userLogic)
        {
            _userLogic = userLogic;
        }
        
        [HttpGet("")]
        public async Task<IEnumerable<UserRead>> GetUsers()
        {
            return await _userLogic.GetUsers();
        }
    }
}
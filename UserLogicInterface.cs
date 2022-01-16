using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftwareFullComponent.UserComponent.DTO;

namespace SoftwareFullComponent.UserComponent
{
    public interface UserLogicInterface
    {
        public Task<IEnumerable<UserRead>> GetUsers();
        public Task<UserRead> GetUser(string userId);
    }
}
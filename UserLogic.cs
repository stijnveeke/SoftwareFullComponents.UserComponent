using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using SoftwareFullComponent.UserComponent.DTO;

namespace SoftwareFullComponent.UserComponent
{
    public class UserLogic: UserLogicInterface
    {
        private readonly IAuth0ApiCalls _apiCalls;
        
        public UserLogic(IAuth0ApiCalls apiCalls)
        {
            _apiCalls = apiCalls;
        }
        
        public async Task<IEnumerable<UserRead>> GetUsers()
        {
            IEnumerable<UserRead> result = await _apiCalls.GetUsersAsync();
            return result;
        }

        public async Task<UserRead> GetUser(string userId)
        {
            UserRead result = await _apiCalls.GetUser(userId);
            return result;
        }
    }
}
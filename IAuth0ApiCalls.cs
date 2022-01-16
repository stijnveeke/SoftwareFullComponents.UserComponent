using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using SoftwareFullComponent.UserComponent.DTO;

namespace SoftwareFullComponent.UserComponent
{
    public interface IAuth0ApiCalls
    {
        public Task<IEnumerable<UserRead>> GetUsersAsync();
        public Task<UserRead> GetUser(string user_id);
    }
}
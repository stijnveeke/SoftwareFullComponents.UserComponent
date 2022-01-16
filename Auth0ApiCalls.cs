using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using SoftwareFullComponent.UserComponent.DTO;

namespace SoftwareFullComponent.UserComponent
{
    public class Auth0Credentials
    {
        public string client_secret { get; set; }
        public string client_id { get; set; }
        public string audience { get; set; }
        public string grant_type {get; set;}
    }

    public class Auth0Response
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }
    public class Auth0ApiCalls: IAuth0ApiCalls
    {
        private readonly IConfiguration _configuration;
        public Auth0ApiCalls(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<Auth0Response> GetAccessToken()
        {
            string client_id = _configuration.GetValue<string>("Auth0:ClientId");
            string client_secret = _configuration.GetValue<string>("Auth0:ClientSecret");
            string audience = _configuration.GetValue<string>("Auth0:Audience");
            var client = new RestClient();
            
            // Get access token
            Auth0Credentials authCredentials = new Auth0Credentials();
            authCredentials.client_id = client_id;
            authCredentials.client_secret = client_secret;
            authCredentials.audience = audience;
            authCredentials.grant_type = "client_credentials";
            
            var access_request = new RestRequest("https://softwarefull-testing.eu.auth0.com/oauth/token", Method.Post);
            access_request.AddBody(authCredentials,
                "application/json"
            );
            
            //Access token
            var accessResponse = await client.ExecuteAsync(access_request);
            return JsonSerializer.Deserialize<Auth0Response>(accessResponse.Content);
        }
        
        public async Task<IEnumerable<UserRead>> GetUsersAsync()
        {
            var client = new RestClient();
            Auth0Response authresponse = await this.GetAccessToken();
            
            //get users
            var request = new RestRequest("https://softwarefull-testing.eu.auth0.com/api/v2/users", Method.Get);
            request.AddHeader("authorization", $"{authresponse.token_type} {authresponse.access_token}");
            RestResponse response = await client.ExecuteAsync(request);

            IEnumerable<UserRead> users = JsonSerializer.Deserialize<IEnumerable<UserRead>>(response.Content);
            //users
            return users;
        }
        
        public async Task<UserRead> GetUser(string user_id)
        {
            var client = new RestClient();
            Auth0Response authresponse = await this.GetAccessToken();
            
            //get users
            string userIdSearchString = Uri.EscapeDataString($"user_id:\"{user_id}\"");
            var request = new RestRequest($"https://softwarefull-testing.eu.auth0.com/api/v2/users?q={userIdSearchString}&search_engine=v3", Method.Get);
            request.AddHeader("authorization", $"{authresponse.token_type} {authresponse.access_token}");
            RestResponse response = await client.ExecuteAsync(request);

            UserRead user = JsonSerializer.Deserialize<IEnumerable<UserRead>>(response.Content).First();
            //users
            return user;
        }
    }
}
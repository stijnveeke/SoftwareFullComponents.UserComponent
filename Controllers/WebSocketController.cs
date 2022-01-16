using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftwareFullComponent.UserComponent;
using SoftwareFullComponent.UserComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent.Controllers
{
    [ApiController]
    [Route("[controller]/ws")]
    public class WebSocketController : ControllerBase
    {

        private readonly ILogger<WebSocketController> _logger;
        private readonly UserLogicInterface _userLogic;

        public WebSocketController(ILogger<WebSocketController> logger, UserLogicInterface logic)
        {
            _logger = logger;
            _userLogic = logic;
        }

        [HttpGet("MultiUserCall")]
        public async Task RequestedUsers()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var ws = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var cts = new System.Threading.CancellationTokenSource();
                    ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Shaking hands: Waiting for user-ids"));
                    await ws.SendAsync(byteToSend, WebSocketMessageType.Text, false, cts.Token);
                    
                    var responseBuffer = new byte[1024];
                    var offset = 0;
                    var packet = 1024;

                    while (true)
                    {
                        ArraySegment<byte> byteRecieved = new ArraySegment<byte>(responseBuffer, offset, packet);
                        WebSocketReceiveResult response = await ws.ReceiveAsync(byteRecieved, cts.Token);
                        var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);

                        if (response.EndOfMessage)
                        {
                            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Agreeing to end this",
                                CancellationToken.None);
                            break;
                        }
                        
                        var userId = responseMessage;
                        var user = await _userLogic.GetUser(userId);
                        var userbytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user));
                        ArraySegment<byte> userToSendInBytes = new ArraySegment<byte>(userbytes);
                        await ws.SendAsync(userToSendInBytes, WebSocketMessageType.Text, false, cts.Token);
                    }
                }
            }
        }

        [HttpGet("User")]
        public async Task GetUsers()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                _logger.LogCritical($"Attempting to get list of users");
                using (var ws = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var users = await _userLogic.GetUsers();

                    foreach (var user in users)
                    {
                        _logger.LogInformation($"Recieved user id: {user.user_id}");
                        var msg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user));
                        await ws.SendAsync(new ArraySegment<byte>(msg, 0, msg.Length), WebSocketMessageType.Text, false, CancellationToken.None);
                    }
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure,"Finished", CancellationToken.None);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        [HttpGet("User/{userId}")]
        public async Task GetUser(string userId)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                _logger.LogInformation($"Attempting to get user id: {userId}");
                using (var ws = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var user = await _userLogic.GetUser(userId);
                    _logger.LogInformation($"Recieved user id: {user.user_id}");
                    var msg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user));
                    await ws.SendAsync(new ArraySegment<byte>(msg, 0, msg.Length), WebSocketMessageType.Text, false, CancellationToken.None);
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure,"Finished", CancellationToken.None);                    
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}
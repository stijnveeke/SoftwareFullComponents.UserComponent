﻿using System;
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

        [HttpGet("User")]
        public async Task GetUsers()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var ws = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var users = await _userLogic.GetUsers();

                    foreach (var user in users)
                    {
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
                using (var ws = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var user = await _userLogic.GetUser(userId);

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
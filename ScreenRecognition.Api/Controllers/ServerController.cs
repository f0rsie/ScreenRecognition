﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ScreenRecognition.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        [Route("StatusCheck")]
        [HttpGet]
        public async Task<bool> StatusCheck()
        {
            var taskResult = await Task.Run(() =>
            {
                return true;
            });

            return taskResult;
        }
    }
}

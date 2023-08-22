using System;
using System.ComponentModel.DataAnnotations;
using HowTo.Entities;
using HowTo.Entities.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HowTo.Controllers;

public class FakeAuthorizationController : Controller
{
    /// <summary>
    /// Временная авторизация для тестирования сервиса
    /// </summary>
    [HttpGet]
    [Route("api/fakeauth")]
    [ValidateModelState]
    public IActionResult GetAuthAsync([Required] [FromQuery] Guid userId,
        [Required] [FromQuery] string userName, [FromQuery] UserRole userRole)
    {
        HttpContext.Response.Cookies.Append(Constants.FakeAuthCookie, $"{userName}:{userId}:{userRole}",
            new CookieOptions
            {
                Domain = HttpContext.Request.Host.Host,
                Expires = DateTimeOffset.Now.AddDays(10)
            });
        
        return Ok(new User(userId, userName, userRole));
    }
}
﻿using Authentication.Application.DTOs;
using Authentication.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IUser userInterface) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserDTO userDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userInterface.RegisterUser(userDTO);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDTO loginDTO)
        {
            // check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userInterface.LoginUser(loginDTO);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId.IsNullOrEmpty())
                return BadRequest("Invalid user Id");
            var user = await userInterface.GetUserAsync(Guid.Parse(userId!));
            return user != null ? Ok(user) : NotFound();
        }
    }
}

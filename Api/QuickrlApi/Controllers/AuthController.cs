﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quickrl.Core.ApiModels;
using Quickrl.Core.ApiModels.AuthModels;
using Quickrl.Core.Interfaces;

namespace QuickrlApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            var result = await _authService.Register(model);

            if (result.Succeeded)
                return Ok();
            
            result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));

            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.Login(model);

            if (result == null)
                return Unauthorized();

            return Ok(result);
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var result = await _authService.ChangePassword(model);

            if (result)
                return Ok();

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("decode")]
        public IActionResult Decode([FromBody] AuthModel model)
        {
            return Ok(_authService.Decode(model));
        }

        [AllowAnonymous]
        [HttpPost("validate")]
        public IActionResult Validate([FromBody] AuthModel model)
        {
            try
            {
                return Ok(_authService.Validate(model));
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }
    }
}
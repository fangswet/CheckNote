﻿using CheckNote.Server.Services;
using CheckNote.Shared.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNote.Server.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService authService;

        public AuthenticationController(AuthenticationService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user) => (await authService.Register(user)).Result();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user) => (await authService.Login(user)).Result();

        public async Task<IActionResult> Logout() => (await authService.Logout()).Result();
    }
}

﻿using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController:ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTManager _jwtManager;
        public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager,
            IJWTManager jwtManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _jwtManager = jwtManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<IdentityUser>(model);
            var res = await _userManager.CreateAsync(user, model.Password);

            if(!res.Succeeded)
            {
                return BadRequest(res.Errors);
            }

            var role = await _roleManager.FindByNameAsync(model.Role);
            
            var roleAddingRes = await _userManager.AddToRoleAsync(user, role?.Name);

            if(!roleAddingRes.Succeeded)
            {
                return BadRequest(roleAddingRes.Errors);
            }

            return Ok(res);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<IdentityUser>(model);

            var signedUser = await _userManager.FindByEmailAsync(model.Email);
            var res = await _signInManager.PasswordSignInAsync(signedUser.UserName, model.Password, true, false);

            if(!res.Succeeded) 
            {
                return BadRequest("Login failed");
            }

            var token = await _jwtManager.GenerateToken(user);

            return Ok($"token = {token}");
        }

    }
}

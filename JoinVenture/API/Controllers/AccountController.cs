using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Service;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using SQLitePCL;

namespace API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController:ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly DataContext _context;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService,DataContext context)
        {
            _context = context;
            _tokenService = tokenService;
            _userManager = userManager;
            
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user,loginDto.Password);

            if(result)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
            {
                return BadRequest("User is already exist");
            }

            if(await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email is alreadt taken");
            }

            var user = new AppUser
            {
                ShowName = registerDto.ShowName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userData = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));


            var user = await _userManager.Users
                .Include(u => u.Orders).ThenInclude(o => o.BookedTicketPackages)
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Id == userData.Id);           

            if(user != null)
            {
                return Ok(user);
            }

            return BadRequest("Usr not Found!");
            
        }

        [Authorize]
        [HttpPost("edit")]
        public async Task<ActionResult<UserDto>> EditUser(UserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            if(user != null)
            {
                user.ShowName = userDto.ShowName;
                user.UserName = userDto.UserName;
                user.PhoneNumber = userDto.PhoneNumber;
                user.Mobile = userDto.Mobile;
                user.Address = userDto.Address;
                user.Email = userDto.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(userDto + "Update Successful!");
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { Errors = errors }); // Return a bad request with error details.

                }
            }
            else
            {
                return NotFound("User not found");
            }
                   
        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                ShowName = user.ShowName,
                Image = null,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName
            };
        }

    }
}
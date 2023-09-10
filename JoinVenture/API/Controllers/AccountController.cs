using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Service;
using Application.Interface;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Persistence;
using SQLitePCL;

namespace API.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AccountController:ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly DataContext _context;
        private readonly SaveUploadedFileService _fileService;
        private readonly IUserAccessor _userAccessor;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService,DataContext context,SaveUploadedFileService fileService,IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _fileService = fileService;
            _context = context;
            _tokenService = tokenService;
            _userManager = userManager;
            
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user == null) return Unauthorized("User Not Found");

            var result = await _userManager.CheckPasswordAsync(user,loginDto.Password);

            if(result)
            {
                return CreateUserObject(user);
            }

            return Unauthorized("Password in not valid");
        }



        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromForm] RegisterDto registerDto)
        {
            //Duplicate Checking
            if(await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
            {
                return BadRequest("User is already exist");
            }

            if(await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email is already taken");
            }


            //Image Handling
            string CloudFrontImagePath = await _fileService.SaveUploadedFileMethod(registerDto.MainImage);

            var user = new AppUser
            {
                ShowName = registerDto.ShowName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };
            var photo = new Photo
            {
                Url = CloudFrontImagePath,
                IsMain = true 
            };

            user.Photos.Add(photo);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest(result.Errors);
        }

        

        [UserProfileCache(1800)]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userData = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));


            var user = await _userManager.Users
                .Include(u => u.Orders).ThenInclude(o => o.BookedTicketPackages)
                .Include(u => u.Orders).ThenInclude(o => o.Tickets)
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Id == userData.Id);           

            if(user != null)
            {
                return Ok(user);
            }

            return BadRequest("User not Found!");
            
        }

        [Authorize(Roles = "Admin")]
        [UserListCache(1800)]
        [HttpGet("userlist")]
        public async Task<ActionResult> GetUserList()
        {

            var users = await _userManager.Users
                .Include(u => u.Orders).ThenInclude(o => o.BookedTicketPackages)
                .Include(u => u.Orders).ThenInclude(o => o.Tickets)
                .Include(u => u.Photos)
                .ToListAsync();

            if(users == null)
            {
                return BadRequest("Not getting any user!");
            }

            return Ok(users);
            
            
        }


        [HttpPost("edit")]
        public async Task<ActionResult<UserDto>> EditUser(UserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            if(user == null)
            {
                return BadRequest("User not found");
            }

            user.ShowName = userDto.ShowName;
            user.UserName = userDto.UserName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Mobile = userDto.Mobile;
            user.Address = userDto.Address;
            user.Email = userDto.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(userDto);
            }

            else
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors }); 
            }

            

                   
        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto(user.ShowName,_tokenService.CreateToken(user),user.Photos.FirstOrDefault().Url,user.UserName);
        }

    }
}
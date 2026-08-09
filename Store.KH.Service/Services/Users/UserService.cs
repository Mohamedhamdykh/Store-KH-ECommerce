using Microsoft.AspNetCore.Identity;
using Store.KH.Core.Dtos.Auth;
using Store.KH.Core.Entities.Identity;
using Store.KH.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Service.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<AppUser> userManager , 
            SignInManager<AppUser> signInManager,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

       

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
           var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) return null;

           var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return null;

            return new UserDto()
            {
                DisplayName = user.DiplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
            
        }

        public async Task<UserDto> RegisterAsync(RegisterDto RegisterDto)
        {
            if (await CheckEmailExistAsync(RegisterDto.Email)) return null;
            var user = new AppUser()
            {
                Email = RegisterDto.Email,
                DiplayName = RegisterDto.DisplayName,
                PhoneNumber = RegisterDto.PhoneNumber,
                UserName = RegisterDto.Email.Split("@")[0]
            };

            var result = await _userManager.CreateAsync(user,RegisterDto.Password);
            if (!result.Succeeded) return null;

            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DiplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
           return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}

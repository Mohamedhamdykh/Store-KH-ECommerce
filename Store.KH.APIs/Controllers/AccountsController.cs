using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.KH.APIs.Erorrs;
using Store.KH.APIs.Extenstions;
using Store.KH.Core.Dtos.Auth;
using Store.KH.Core.Entities.Identity;
using Store.KH.Core.Services.Contract;
using Store.KH.Service.Services.Tokens;
using System.Security.Claims;

namespace Store.KH.APIs.Controllers
{
    
    public class AccountsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(IUserService userService ,
            UserManager<AppUser> userManager ,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService ;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            if (user is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(CheckEmailExists(registerDto.Email).Result.Value)
            {
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"Email Is Already Exists !!"));
            }
            var user = await _userService.RegisterAsync(registerDto);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"Invalid Registeration !!"));
            return Ok(user);
        }
        [HttpGet("getCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto  >> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto()
            {
                DisplayName = user.DiplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUserAddress()
        {

            var user = await _userManager.FindByEmaliWithAddressAsync(User);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }
        [HttpPut("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto model)
        {

            var user = await _userManager.FindByEmaliWithAddressAsync(User);
            var address = _mapper.Map<AddressDto,Address>(model); 
            user.Address = address;
            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {

            //var user = await _userManager.FindByEmailAsync(email);
            //if (user is null) return false;
            //return true;

          return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}


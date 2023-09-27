using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using PokemonReviewApp.Services;
using AutoMapper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;
using System.Collections.Specialized;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user=new User();
        private readonly IConfiguration configuration;
        private readonly IUserService UserService;
        private readonly IMapper _mapper;
        private readonly IUserRepo _userrepo;
        User uss;

        public AuthController(IConfiguration Configuration, IUserService userService, IMapper mapper,IUserRepo Userrepo)
        {
            configuration = Configuration;
            UserService = userService;

            _mapper = mapper;
            _userrepo = Userrepo;
                        User uss;

        }

        [HttpGet("Returnhello")]
        [Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = UserService.GetMyName;

            return Ok("Helloss");
        }

        





        [HttpPost("Register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult  Register([FromBody] UserDto Request)
        {
            if (Request == null)
                return BadRequest(ModelState);


            CreatePasswordHash(Request.Password, out byte[] Password_Hash, out byte[] Password_Salt);          
            user.USerName = Request.UserName;
            user.SaltPassword = Password_Salt;
            user.HashPassword= Password_Hash;
            var RegMap = _mapper.Map<User>(user);

            if (!_userrepo.CreateUser(RegMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("SUccess");
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserDto Request)
        {
            CreatePasswordHash(Request.Password, out byte[] Password_Hash, out byte[] Password_Salt);
            uss= _userrepo.GetUser(Request.UserName);
            if (uss != null)
            {
               
            }
            else
            {
                return BadRequest("User Not Found.");
            }
            if(VerifyPasswordHash(Request.Password,uss.HashPassword,uss.SaltPassword ) )
            {
                //string Token = CreateToken(uss);
                var refreshToken = GenerateRefreshToken();
                SetRefreshToken(refreshToken);
                return Ok(refreshToken);
            }

            return BadRequest("WrongPass.");





        }

        private RefreshToken GenerateRefreshToken()
        {
            var refresh_token = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expired = DateTime.Now.AddMinutes(1),
                Created=DateTime.Now

            };

            return refresh_token;

        }

        private void SetRefreshToken(RefreshToken NewRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = NewRefreshToken.Expired.AddMinutes(1),
            };
            Response.Cookies.Append("refreshToken",NewRefreshToken.Token,cookieOptions);

            uss.RefreshToken = NewRefreshToken.Token;
            uss.TokenCreated = NewRefreshToken.Created;
            uss.TokenExpires = NewRefreshToken.Expired;
            _userrepo.changeRefresh(uss.RefreshToken, uss.TokenCreated, uss.TokenExpires, uss.USerName);
        }



      
        private string CreateToken(User user) 
        {
            List<Claim> Claimss = new List<Claim>
            {
                new Claim (ClaimTypes.Name,user.USerName)
            };
            var keyy = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("JWT:key").Value));

            var Credss = new SigningCredentials(keyy, SecurityAlgorithms.HmacSha512Signature);

            var Token = new JwtSecurityToken(
                claims: Claimss,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: Credss);

            var JWT =new JwtSecurityTokenHandler().WriteToken(Token);

                
                
            return JWT;
         }



        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"]; 

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            //else if (user.TokenExpires < DateTime.Now)
            //{
            //    return Unauthorized("Token expired.");
            //}

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private void CreatePasswordHash(string password ,out byte[] PasswordHash,out byte[] PasswordSalt)
        {
            using (var Hmac =new HMACSHA512( )) 
            {
                PasswordSalt = Hmac.Key;
                PasswordHash = Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

       



        private bool VerifyPasswordHash(string password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var Hmac=new HMACSHA512(PasswordSalt) )
            {
                var ComputedHash = Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return ComputedHash.SequenceEqual(PasswordHash);
            }
        }

    }
}

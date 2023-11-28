using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FamilyBudget.Backend.DTOs;
using FamilyBudget.DTOs;
using FamilyBudgetApplication.Auth;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetApplication.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FamilyBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticator _authenticator;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserRegistrant _userRegistrator;


        public AuthenticationController(IAuthenticator authenticator, IConfiguration configuration, ITokenGenerator tokenGenerator, IUserRegistrant userRegistrant)
        {
            _authenticator = authenticator;
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _userRegistrator = userRegistrant;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthorizationRequest userModel)
        {
            try
            {
                var user = await _authenticator.AuthenticateUser(userModel.UserName, userModel.Password);
                var token = _tokenGenerator.GenerateToken(userModel.UserName);
                return Ok(new AuthorizationResponse
                {
                    Token = _tokenGenerator.TokenToString(token),
                    Expiration = token.ValidTo,
                    Id = user.Id
                });
            }
            catch (AuthenticationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AuthorizationRequest userModel)
        {
            try
            {
                await _userRegistrator.RegisterUser(userModel.UserName, userModel.Password);
                return Ok(new { Status = "Success", Message = "User created successfully!" });
            }
            catch (RegistrationException regExc)
            {
                return BadRequest(regExc.Message);
            }
            catch (InnerQuietException innerEx)
            {
                return StatusCode(500, innerEx.Message);
            }
        }
    }
}
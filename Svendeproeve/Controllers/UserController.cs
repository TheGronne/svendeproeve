using Microsoft.AspNetCore.Mvc;
using Svendeproeve.Repositories;
using Svendeproeve.DTOObjects;
using Svendeproeve.DomainObjects;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Svendeproeve.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        public UserController(IUserRepository _userRepository)
        {
            userRepository = _userRepository;

            if (userRepository == null)
                throw new ArgumentNullException(nameof(IUserRepository));
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return userRepository.FindByID(id);
        }

        // GET api/<UserController>/signIn/loginName=a&password=b
        [HttpGet("signIn")]
        public User SignIn(string loginName, string password)
        {
            try
            {
                return userRepository.SignIn(loginName, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST api/<UserController>/signUp
        [HttpPost("signUp")]
        public IActionResult SignUp([FromBody] UserSignUpDTO signUpDTO)
        {
            try
            {
                userRepository.SignUp(signUpDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Must_be_unique"))
                    return Conflict("LoginName already taken");
                else
                    return BadRequest();
            }

            return Ok();
        }

        // PUT api/<UserController>/5...
        [HttpPut]
        public IActionResult Put(int id, string username, string newLoginName, string newPassword, string oldLoginName, string oldPassword)
        {
            try
            {
                var settings = new UserSettingsDTO(username, newLoginName, newPassword, oldLoginName, oldPassword);
                userRepository.Update(id, settings);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // DELETE api/<UserController>
        [HttpDelete]
        public IActionResult Delete(string loginName, string password)
        {
            try
            {
                userRepository.Delete(loginName, password);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

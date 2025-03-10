using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Middleware.GlobalExceptionHandler;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Service;

namespace HelloGreetingApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserBL _userBL;
        private readonly ILogger<UserRL> _logger;
        public UserController(IUserBL userBL, ILogger<UserRL> logger)
        {
            _userBL = userBL;
            _logger = logger;
        }
        /// <summary>
        /// Post Method for Login User
        /// </summary>
        /// <param name="userLoginModel"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(LoginUserModel userLoginModel)
        {
            try
            {
                var response = new ResponseModel<string>();
                var user = _userBL.Login(userLoginModel);
                if (user != null)
                {
                    response.Success = true;
                    response.Message = "User Login Successfully.";
                    response.Data = user;
                    return Ok(response);
                }
                return BadRequest("Invalid Credentials!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Id not found Error.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return NotFound(errorResponse);
            }
           
        }
        /// <summary>
        /// Postmethod to register the user
        /// </summary>
        /// <param name="registrationUserModel"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult Registration(RegistrationUserModel registrationUserModel)
        {
            try
            {
                var response = new ResponseModel<string>();
                var data = _userBL.Registration(registrationUserModel);


                response.Success = true;
                response.Message = "User registered Successfully.";
                response.Data = "";
                return Ok(response);
            }


            catch (Exception ex)
            {
                _logger.LogError("User Already Registered.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return NotFound(errorResponse);
            }

            
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(UserEntity userEntity)
        {
            return Ok(userEntity);
        }

    }
}
